using System.Security.Cryptography.Pkcs;
using System.Text;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Services.OrderPlacement;

public class OrderPlacementQueueService : IOrderPlacementProcessingService
{
	private const int FIVE_MINUTES = 5;
	private const int PROCESSING_TIMEOUT_MSEC = FIVE_MINUTES * 60 * 1000;

	private const int WARNING_HOURS = 2;
	private const int FAILURE_HOURS = 48;

	private readonly IOrderingProxy orderingProxy;
	private readonly IDistributorServiceLookup distributorServiceLookup;
	private readonly INotificationProxy notificationProxy;
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<OrderPlacementQueueService> logger;

	public OrderPlacementQueueService(IOrderingProxy orderingProxy, 
									IDistributorServiceLookup distributorServiceLookup, 
									INotificationProxy notificationProxy,
									IServiceProxy serviceProxy,
									ILogger<OrderPlacementQueueService> logger)
	{
		this.orderingProxy = orderingProxy;
		this.distributorServiceLookup = distributorServiceLookup;
		this.serviceProxy = serviceProxy;
		this.logger = logger;
		this.notificationProxy = notificationProxy;
	}
	
	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(OrderPlacementQueueService));
		
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!ProcessOrderQueue())
			{
				await Task.Delay(PROCESSING_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool ProcessOrderQueue()
	{ 
		var serviceLog = serviceProxy.CreateServiceLogFor(nameof(OrderPlacementQueueService));

		logger.LogInformation("Checking for queued orders that need sending");

		var pendingPurchaseOrders = orderingProxy.GetPendingPurchaseOrders();
		if (pendingPurchaseOrders.Count == 0)
		{
			logger.LogInformation("Nothing to do.");
			serviceProxy.CloseServiceLogFor(serviceLog);
			return false;
		}
		
		foreach (var orders in CreateDistributorOrdersFrom(pendingPurchaseOrders))
		{
			var success = AttemptToTransmitPurchaseOrder(orders);
			foreach (var order in orders.PurchaseOrders)
			{
				if (success)
				{
					order.SuccessDateTime = DateTime.Now;
				}
				
				order.Attempts++;
				order.LastAttemptDateTime = DateTime.Now;
				orderingProxy.SavePurchaseOrder((PurchaseDistributorOrder)order);
			}
		}

		serviceProxy.CloseServiceLogFor(serviceLog);
		return false;
	}

	private List<DistributorOrders> CreateDistributorOrdersFrom(List<PurchaseDistributorOrder> orders)
	{
		var matcher = new Dictionary<string, List<PurchaseDistributorOrder>>();
		foreach (var order in orders)
		{
			if (matcher.ContainsKey(order.PoNumber))
			{
				matcher[order.PoNumber].Add(order);
			}
			else
			{
				var list = new List<PurchaseDistributorOrder>();
				list.Add(order);
				matcher[order.PoNumber] = list;
			}
		}

		var assemblies = new List<DistributorOrders>();
		foreach (var key in matcher.Keys)
		{
			var assembly = new DistributorOrders();
			assembly.PoNumber = key;
			assemblies.Add(assembly);
			
			bool isFirst = true;
			foreach (var order in matcher[key])
			{
				if (isFirst)
				{
					assembly.DistributorName = order.DistributorName;
					assembly.DistributorLookupCode = order.Distributor.LookupCode;
					assembly.OrderDate = order.OrderDate;
					assembly.ShopifyOrderId = order.ShopifyOrderId;
					isFirst = false;
				}
				assembly.PurchaseOrders.Add(order);
			}
			
		}

		return assemblies;
	}

	private bool AttemptToTransmitPurchaseOrder(DistributorOrders orders)
	{
		try
		{
			var orderService = distributorServiceLookup.GetOrderServiceFor(orders.DistributorLookupCode);
			return orderService.PlaceOrder(orders);
		}
		catch (Exception ex)
		{
			logger.LogError($"Exception caught while attempting to send PO {orders.PoNumber}: {ex}");
			HandleNotificationsFor(orders);
		}

		return false;
	}

	private void HandleNotificationsFor(DistributorOrders orders)
	{
		var now = DateTime.Now;
		foreach (var order in orders.PurchaseOrders)
		{
			var difference = now.Subtract(order.CreateDate);
			if (now.CompareTo(order.CreateDate.AddHours(FAILURE_HOURS)) >= 0)
			{
				logger.LogError(
					$"PO {order.PoNumber} in record ID {order.Id} has expired the Failure time after {order.Attempts} attempts to deliver!");
				TransmitErrorMessage(order, Convert.ToInt32(difference.TotalHours));
				return;
			}

			int expectedAlertCount = difference.Hours / WARNING_HOURS;

			if (expectedAlertCount < order.Attempts)
			{
				logger.LogWarning(
					$"PO {order.PoNumber} in record ID {order.Id} has not transmitted after {order.Attempts} attempts to deliver!");
				TransmitWarningMessage(order, Convert.ToInt32(difference.TotalHours));
			}
		}
	}

	private void TransmitErrorMessage(IDistributorOrder order, int hours)
	{
		try
		{
			var subject = $"ERROR: Cannot send PO {order.PoNumber} to {order.DistributorName}";
			
			var body = new StringBuilder();
			body.Append($"ERROR: DELIVERY FAILED AFTER RETRYING {hours} HOURS!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"The following PO cannot be transmitted to {order.DistributorName} after trying for {order.Attempts} attempts.\r\n\r\n");
			body.Append(OrderWriter.FormatOrder(order));
			body.Append("\r\n");
			body.Append("The service will not attempt to deliver this any longer.  It requires human intervention to send the order.\r\n\r\n");
			
			logger.LogInformation($"Transmitting ERROR message that retrying has stopped for PO {order.PoNumber} in record ID {order.Id} after {order.Attempts} attempts to deliver.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
			
			order.FailureNotificationDateTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that retrying has stopped for PO {order.PoNumber} in record ID {order.Id} after {order.Attempts} attempts to deliver: {ex}");
		}
	}
	
	private void TransmitWarningMessage(IDistributorOrder order, int hours)
	{
		try
		{
			var subject = $"Warning: Problems sending PO {order.PoNumber} to {order.DistributorName}";

			var body = new StringBuilder();
			body.Append($"Warning: Deliver failed after retrying {hours} hours!  Is the distributor's service offline??  \r\n\r\n");
			body.Append($"The following PO has not yet transmitted to {order.DistributorName} after trying for {order.Attempts} attempts.\r\n\r\n");
			body.Append(OrderWriter.FormatOrder(order));
			body.Append("\r\n");
			body.Append("The service will continue attempting to deliver this PO.  Perhaps a call/email to the distributor's support is needed?\r\n\r\n");
			
			logger.LogInformation($"Transmitting warning message concerning retries for PO {order.PoNumber} in record ID {order.Id} after {order.Attempts} attempts to deliver.");
			notificationProxy.SendWarningErrorNotification(subject, body.ToString());
			
			order.FailureNotificationDateTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			logger.LogError($"Error transmitting WARNING message concerning retries for PO {order.PoNumber} in record ID {order.Id} after {order.Attempts} attempts to deliver: {ex}");
		}
	}
}