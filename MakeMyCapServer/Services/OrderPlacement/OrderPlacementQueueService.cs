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
		
		bool firstTime = true;
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
// TODO: CML - PREVENTING ORDERS needs to be fixed		
if (1 != 0) {		
logger.LogCritical("PREVENTING ORDERS FROM BEING SENT UNTIL READY");
return false;
}
		var serviceLog = serviceProxy.CreateServiceLogFor(nameof(OrderPlacementQueueService));

		logger.LogInformation("Checking for queued orders that need sending");

		var pendingPurchaseOrders = orderingProxy.GetPendingPurchaseOrders();
		if (pendingPurchaseOrders.Count == 0)
		{
			logger.LogInformation("Nothing to do.");
			serviceProxy.CloseServiceLogFor(serviceLog);
			return false;
		}

		foreach (var pendingPurchaseOrder in pendingPurchaseOrders)
		{
			if (AttemptToTransmitPurchaseOrder(pendingPurchaseOrder))
			{
				pendingPurchaseOrder.SuccessDateTime = DateTime.Now;
			}

			pendingPurchaseOrder.Attempts++;
			pendingPurchaseOrder.LastAttemptDateTime = DateTime.Now;
			orderingProxy.SavePurchaseOrder(pendingPurchaseOrder);
		}

		serviceProxy.CloseServiceLogFor(serviceLog);
		return false;
	}

	private bool AttemptToTransmitPurchaseOrder(PurchaseOrder purchaseOrder)
	{
		try
		{
			var orderService = distributorServiceLookup.GetOrderServiceFor(purchaseOrder.Distributor.LookupCode);
			return orderService.PlaceOrder(purchaseOrder);
		}
		catch (Exception ex)
		{
			logger.LogError($"Exception caught while attempting to send PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id}: {ex}");
			HandleNotificationsFor(purchaseOrder);
		}

		return false;
	}

	private void HandleNotificationsFor(PurchaseOrder purchaseOrder)
	{
		var now = DateTime.Now;
		var difference = now.Subtract(purchaseOrder.CreateDate);
		if (now.CompareTo(purchaseOrder.CreateDate.AddHours(FAILURE_HOURS)) >= 0)
		{
			logger.LogError($"PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} has expired the Failure time after {purchaseOrder.Attempts} attempts to deliver!");
			TransmitErrorMessage(purchaseOrder, Convert.ToInt32(difference.TotalHours));
			return;
		}

		int expectedAlertCount = difference.Hours / WARNING_HOURS;

		if (expectedAlertCount < purchaseOrder.Attempts)
		{
			logger.LogWarning($"PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} has not transmitted after {purchaseOrder.Attempts} attempts to deliver!");
			TransmitWarningMessage(purchaseOrder, Convert.ToInt32(difference.TotalHours));
		}
	}

	private void TransmitErrorMessage(PurchaseOrder purchaseOrder, int hours)
	{
		try
		{
			var subject = $"ERROR: Cannot send PO {purchaseOrder.PoNumber} to {purchaseOrder.Distributor.Name}";
			
			var body = new StringBuilder();
			body.Append($"ERROR: DELIVERY FAILED AFTER RETRYING {hours} HOURS!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"The following PO cannot be transmitted to {purchaseOrder.Distributor.Name} after trying for {purchaseOrder.Attempts} attempts.\r\n\r\n");
			body.Append(OrderWriter.FormatOrder(purchaseOrder));
			body.Append("\r\n");
			body.Append("The service will not attempt to deliver this any longer.  It requires human intervention to send the order.\r\n\r\n");
			
			logger.LogInformation($"Transmitting ERROR message that retrying has stopped for PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} after {purchaseOrder.Attempts} attempts to deliver.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
			
			purchaseOrder.FailureNotificationDateTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that retrying has stopped for PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} after {purchaseOrder.Attempts} attempts to deliver: {ex}");
		}
	}
	
	private void TransmitWarningMessage(PurchaseOrder purchaseOrder, int hours)
	{
		try
		{
			var subject = $"Warning: Problems sending PO {purchaseOrder.PoNumber} to {purchaseOrder.Distributor.Name}";

			var body = new StringBuilder();
			body.Append($"Warning: Deliver failed after retrying {hours} hours!  Is the distributor's service offline??  \r\n\r\n");
			body.Append($"The following PO has not yet transmitted to {purchaseOrder.Distributor.Name} after trying for {purchaseOrder.Attempts} attempts.\r\n\r\n");
			body.Append(OrderWriter.FormatOrder(purchaseOrder));
			body.Append("\r\n");
			body.Append("The service will continue attempting to deliver this PO.  Perhaps a call/email to the distributor's support is needed?\r\n\r\n");
			
			logger.LogInformation($"Transmitting warning message concerning retries for PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} after {purchaseOrder.Attempts} attempts to deliver.");
			notificationProxy.SendWarningErrorNotification(subject, body.ToString());
			
			purchaseOrder.FailureNotificationDateTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			logger.LogError($"Error transmitting WARNING message concerning retries for PO {purchaseOrder.Ponumber} in record ID {purchaseOrder.Id} after {purchaseOrder.Attempts} attempts to deliver: {ex}");
		}
	}
}