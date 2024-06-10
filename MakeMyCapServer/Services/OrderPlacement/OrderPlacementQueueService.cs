using System.Text;
using MakeMyCapServer.Distributors;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;

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

		var manualOrders = new List<ManualOrder>();
		foreach (var orders in CreateDistributorOrdersFrom(pendingPurchaseOrders))
		{
			var orderStatus = AttemptToTransmitPurchaseOrder(orders);
			foreach (var order in orders.PurchaseOrders)
			{
				if (orderStatus.Successful)
				{
					order.SuccessDateTime = DateTime.Now;
				}
				else
				{
					UpdateOrderWithOutOfStockValues(order, orderStatus.OutOfStockItems, orderingProxy.GetDistributorByCode(orders.DistributorLookupCode), manualOrders);
				}
				
				order.Attempts++;
				order.LastAttemptDateTime = DateTime.Now;
				orderingProxy.SavePurchaseOrder((PurchaseDistributorOrder)order);
			}
		}

		NotifyConcerningOutOfStockOrders(manualOrders);

		serviceProxy.CloseServiceLogFor(serviceLog);
		return false;
	}

	private void UpdateOrderWithOutOfStockValues(IDistributorOrder order, List<IOutOfStockItem> outOfStockItems, Distributor? distributor, List<ManualOrder> manualOrders)
	{
		foreach (var outOfStockItem in outOfStockItems)
		{
			if (outOfStockItem.DistributorSkuMap == null)
			{
				continue;
			}

			if (order.Quantity < outOfStockItem.AvailableQuantity)
			{
				// split the order to retrieve as many as possible
				var poLine = CloneForSplit(order, distributor!);
				poLine.Quantity = outOfStockItem.AvailableQuantity;
				order.Quantity -= poLine.Quantity;
				
				manualOrders.Add(new ManualOrder(outOfStockItem, order.PoNumber, order.ShopifyOrderId, distributor.Name));
				logger.LogInformation($"PO# {order.PoNumber} to {order.DistributorName} for Shopify order {order.ShopifyOrderId} line item for {order.Quantity} item(s) has been split and an order for {outOfStockItem.AvailableQuantity} created while marking the rest Out of Stock");
				order.FailureNotificationDateTime = DateTime.Now;

				orderingProxy.SavePurchaseOrder(poLine);
			}
			else
			{
				manualOrders.Add(new ManualOrder(outOfStockItem, order.PoNumber, order.ShopifyOrderId, distributor.Name));
				logger.LogInformation($"PO# {order.PoNumber} to {order.DistributorName} for Shopify order {order.ShopifyOrderId} line item for {order.Quantity} item(s) has been marked as failed since it is Out of Stock");
				order.FailureNotificationDateTime = DateTime.Now;
			}
		}
	}
	
	private static PurchaseDistributorOrder CloneForSplit(IDistributorOrder source, Distributor distributor)
	{
		var clone = new PurchaseDistributorOrder();
		clone.Distributor = distributor;
		clone.CreateDate = source.CreateDate;
		clone.Ponumber = source.PoNumber;
		clone.PoNumberSequence = source.PoNumberSequence;
		clone.ShopifyOrderId = source.ShopifyOrderId;
		clone.Sku = source.Sku;
		clone.Quantity = source.Quantity;
		clone.Style = source.Style;
		clone.Color = source.Color;
		clone.Size = source.Size;
		clone.SubmittedDateTime = source.CreateDate;
		clone.ShopifyName = source.ShopifyName;
		return clone;
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
				var orderDetail = order.ShopifyOrderId == null ? null : orderingProxy.GetOrderById((long)order.ShopifyOrderId);

				if (isFirst)
				{
					assembly.DistributorName = order.DistributorName;
					assembly.DistributorLookupCode = order.Distributor.LookupCode;
					assembly.OrderDate = order.OrderDate;
					assembly.ShopifyOrderId = order.ShopifyOrderId;
					assembly.ShopifyOrderNumber = orderDetail == null ? "" : orderDetail.OrderNumber;

					assembly.DeliverToName = orderDetail?.DeliverToName;
					assembly.DeliverToAddress1 = orderDetail?.DeliverToAddress1;
					assembly.DeliverToAddress2 = orderDetail?.DeliverToAddress2;
					assembly.DeliverToCity = orderDetail?.DeliverToCity;
					assembly.DeliverToStateProv = orderDetail?.DeliverToStateProv;
					assembly.DeliverToZipPC = orderDetail?.DeliverToZipPC;
					assembly.DeliverToCountry = orderDetail?.DeliverToCountry;

					assembly.OrderNotes = orderDetail?.OrderNotes;

					isFirst = false;
				}
				assembly.PurchaseOrders.Add(order);
			}
		}

		return assemblies;
	}

	private OrderStatus AttemptToTransmitPurchaseOrder(DistributorOrders orders)
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

		return new OrderStatus(false);
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
	
	private void NotifyConcerningOutOfStockOrders(List<ManualOrder> manualOrders)
	{
		if (manualOrders.Count == 0)
		{
			return;
		}
		
		try
		{
			var subject = "NOTICE: OUT OF STOCK: Please review and replace/order manually";
			
			var body = new StringBuilder();
			body.Append($"ERROR: The following item(s) have Out of Stock alerts!  Manual intervention is needed!!\r\n\r\n");

			foreach (var order in manualOrders)
			{
				body.Append($"PO: {order.PoNumber} to distributor: {order.DistributorName}     Shopify Id: {order.ShopifyOrderId}\r\n");
				body.Append($"    MMC Item Sku: {order.DistributorSkuMap.Sku}  ({order.Description})\r\n");
				var needsAttention = order.OrderedQuantity - order.AvailableQuantity;
				body.Append($"    Originally ordered: {order.OrderedQuantity}   Available/Reordered: {order.AvailableQuantity}   Needs Attention: {(needsAttention > 0 ? needsAttention : 0)}\r\n\r\n");
			}

			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting OutOfStock message: {ex}");
		}
	}
}