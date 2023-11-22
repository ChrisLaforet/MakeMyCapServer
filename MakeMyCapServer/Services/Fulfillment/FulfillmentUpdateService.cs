using System.Text;
using MakeMyCapServer.Distributors;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Background;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Shopify.Dtos.Fulfillment;
using ShopifyOrder = MakeMyCapServer.Shopify.Dtos.Fulfillment.Order;
using ShopifyFulfillment = MakeMyCapServer.Shopify.Dtos.Fulfillment.Fulfillment;
using DbOrder = MakeMyCapServer.Model.Order;
using IOrderService = MakeMyCapServer.Shopify.Services.IOrderService;

namespace MakeMyCapServer.Services.Fulfillment;

public sealed class FulfillmentUpdateService : IFulfillmentProcessingService
{
	private const int DEFAULT_DELAY_TIMEOUT_HOURS = 2;

	private readonly IOrderService orderService;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly IFulfillmentProxy fulfillmentProxy;
	private readonly IServiceProxy serviceProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<FulfillmentUpdateService> logger;
	private readonly IDistributorServiceLookup distributorServiceLookup;
	private readonly IOrderGenerator orderGenerator;

	// https://learn.microsoft.com/en-us/dotnet/standard/threading/how-to-listen-for-multiple-cancellation-requests
	private CancellationTokenSource immediateProcessingRequestedTokenSource = new CancellationTokenSource();

	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;

	public FulfillmentUpdateService(IOrderService orderService, 
									IProductSkuProxy productSkuProxy,
									IFulfillmentProxy fulfillmentProxy,
									IOrderGenerator orderGenerator,
									IServiceProxy serviceProxy, 
									INotificationProxy notificationProxy, 
									IDistributorServiceLookup distributorServiceLookup,
									ILogger<FulfillmentUpdateService> logger)
	{
		this.orderService = orderService;
		this.productSkuProxy = productSkuProxy;
		this.fulfillmentProxy = fulfillmentProxy;
		this.orderService = orderService;
		this.serviceProxy = serviceProxy;
		this.distributorServiceLookup = distributorServiceLookup;
		this.logger = logger;
		this.notificationProxy = notificationProxy;

		//immediateProcessingRequestedToken = immediateProcessingRequestedTokenSource.Token;
	}

	public void ResumeProcessingNow()
	{
		try
		{
			logger.LogInformation("Attempting to resume immediate processing of Orders and Fulfillment...");
			if (!immediateProcessingRequestedTokenSource.IsCancellationRequested)
			{
				immediateProcessingRequestedTokenSource.Cancel();
			}
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception trying to resume processing: {ex}");
		}
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(FulfillmentUpdateService));
		
		bool firstTime = true;
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateFulfillment())
			{
				CheckAndUpdateDelay(firstTime);
				firstTime = false;
				
				if (!stoppingToken.IsCancellationRequested && immediateProcessingRequestedTokenSource.IsCancellationRequested)
				{
					immediateProcessingRequestedTokenSource = new CancellationTokenSource();
				}

// TODO: CML - REMOVE THIS COMMENT WHEN FINISHED TESTING CANCELLATION				
				// UpdateFulfillment();
				
				using (CancellationTokenSource multiplexedTokens = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, immediateProcessingRequestedTokenSource.Token))
				{
					try
					{
Console.WriteLine("SLEEP");						
						await Task.Delay(delayTimeoutHours * 60 * 60 * 1000, immediateProcessingRequestedTokenSource.Token);
//await Task.Delay(delayTimeoutHours * 60 * 60 * 1000, multiplexedTokens.Token);
					}
					catch (OperationCanceledException)
					{
						// do nothing at the moment
					}
				}
Console.WriteLine("HELLO");
			}
		}

		if (!immediateProcessingRequestedTokenSource.IsCancellationRequested)
		{
			immediateProcessingRequestedTokenSource.Cancel();
		}
	}

	private void CheckAndUpdateDelay(bool firstTime = false)
	{
		int? hours = serviceProxy.GetFulfillmentCheckHours();
		if (hours == null)
		{
			if (firstTime)
			{
				logger.LogInformation($"Setting delay time for {nameof(FulfillmentUpdateService)} to default {DEFAULT_DELAY_TIMEOUT_HOURS} hours between processing tasks");
			}
			return;
		}
		
		if (hours != delayTimeoutHours)
		{
			logger.LogInformation($"Setting delay time for {nameof(FulfillmentUpdateService)} to {hours} hours between processing tasks");
			delayTimeoutHours = (int)hours;
		}
	}

	private bool UpdateFulfillment()
	{
		logger.LogInformation("Checking for fulfillment orders that need processing");
		ServiceLog? serviceLog = null;
		try
		{
			serviceLog = serviceProxy.CreateServiceLogFor(nameof(FulfillmentUpdateService));
			ProcessOpenOrders();
			serviceProxy.CloseServiceLogFor(serviceLog);
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception: {ex}");
			if (serviceLog != null)
			{
				serviceProxy.CloseServiceLogFor(serviceLog, true);
			}
		}
		
		return false;
	}

	private void ProcessOpenOrders()
	{
		foreach (var shopifyOrder in orderService.GetOpenOrders())
		{
			if (!fulfillmentProxy.DoesOrderExist(shopifyOrder.Id))
			{
				continue;
			}

			ProcessFulfillmentFor(shopifyOrder);
		}
	}

	private void ProcessFulfillmentFor(ShopifyOrder shopifyOrder)
	{
		logger.LogInformation($"New order in Shopify with Id {shopifyOrder.Id} with {shopifyOrder.LineItems} line items needing processing.");
		PrepareOrder(shopifyOrder);
	}

	private DbOrder PrepareOrder(ShopifyOrder shopifyOrder)
	{
		try
		{
			var order = new DbOrder();
			order.OrderId = shopifyOrder.Id;
			if (shopifyOrder.OrderNumber != null)
			{
				order.OrderNumber = shopifyOrder.OrderNumber.ToString();
			}

			order.CheckoutId = shopifyOrder.CheckoutId == null ? 0 : (long)shopifyOrder.CheckoutId;
			order.CheckoutToken = shopifyOrder.CheckoutToken;
			order.CreatedDateTime = shopifyOrder.CreatedAt;
			order.ProcessStartDateTime = DateTime.Now;

			foreach (var shopifyFulfillment in shopifyOrder.Fulfillments)
			{
				PrepareFulfillment(shopifyFulfillment, order);
			}

			fulfillmentProxy.SaveOrder(order);
			return order;
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception processing Shopify Order {shopifyOrder.Id}: {ex}");
		}

		return null;
	}

	private void PrepareFulfillment(ShopifyFulfillment shopifyFulfillment, DbOrder order)
	{
		var fulfillment = new FulfillmentOrder();
		fulfillment.FulfillmentOrderId = shopifyFulfillment.Id;
		fulfillment.OrderId = order.OrderId;
		fulfillment.Status = shopifyFulfillment.Status;
		order.FulfillmentOrders.Add(fulfillment);
		
		foreach (var lineItem in shopifyFulfillment.LineItems)
		{
			PrepareAndOrderLineItem(lineItem, fulfillment);
		}
	}

	private void PrepareAndOrderLineItem(LineItem shopifyLineItem, FulfillmentOrder fulfillmentOrder)
	{
		var lineItem = new OrderLineItem();
		lineItem.LineItemId = shopifyLineItem.Id;
		lineItem.FulfillmentOrderId = fulfillmentOrder.FulfillmentOrderId;
		lineItem.ProductId = shopifyLineItem.ProductId;
		lineItem.VariantId = shopifyLineItem.VariantId;
		lineItem.Sku = shopifyLineItem.Sku;
		lineItem.Name = shopifyLineItem.Name;
		lineItem.Quantity = shopifyLineItem.Quantity;
		
		fulfillmentOrder.OrderLineItems.Add(lineItem);

		var match = productSkuProxy.GetSkuMapFor(shopifyLineItem.Sku);
		if (match == null)
		{
			logger.LogError($"Unable to match SKU {shopifyLineItem.Sku} in Shopify Order {fulfillmentOrder.OrderId} for automated ordering");
			TransmitSkuNotFoundErrorMessage(shopifyLineItem, fulfillmentOrder);
		}
		else
		{
			var poNumber = orderGenerator.GenerateOrderFor(match, fulfillmentOrder.OrderId, shopifyLineItem.Quantity);
			if (poNumber != null)
			{
				logger.LogInformation($"Generated PO {poNumber} for ordering {shopifyLineItem.Quantity} of SKU {shopifyLineItem.Sku} in Shopify Order {fulfillmentOrder.OrderId}");
			}
		}
	}
	
	private void TransmitSkuNotFoundErrorMessage(LineItem shopifyLineItem, FulfillmentOrder fulfillmentOrder)
	{
		try
		{
			var subject = $"ERROR: Cannot find SKU - manual order is needed";
			
			var body = new StringBuilder();
			body.Append($"ERROR: CANNOT FIND SKU {shopifyLineItem.Sku} IN SHOPIFY ORDER {fulfillmentOrder.OrderId}!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"This SKU references an item called {shopifyLineItem.Name}.\r\n");
			body.Append($"Product Id in Shopify is {shopifyLineItem.ProductId}.\r\n");
			body.Append($"Variant Id in Shopify is {shopifyLineItem.VariantId}.\r\n");
			body.Append("\r\n");
			body.Append("The service cannot order this product.  It requires human intervention to send the order.\r\n\r\n");

			logger.LogInformation($"Transmitting ERROR message that cannot find SKU in Shopify Order {fulfillmentOrder.OrderId}.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that SKU cannot be found in Shopify Order {fulfillmentOrder.OrderId}: {ex}");
		}
	}
}