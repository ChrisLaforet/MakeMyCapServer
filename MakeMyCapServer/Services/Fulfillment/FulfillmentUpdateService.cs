using System.Text;
using MakeMyCapServer.Distributors;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Background;
using MakeMyCapServer.Shopify.Dtos.Fulfillment;
using ShopifyOrder = MakeMyCapServer.Shopify.Dtos.Fulfillment.Order;
using DbOrder = MakeMyCapServer.Model.Order;
using IOrderService = MakeMyCapServer.Shopify.Services.IOrderService;

namespace MakeMyCapServer.Services.Fulfillment;

public sealed class FulfillmentUpdateService : IFulfillmentProcessingService
{
	private const int DEFAULT_DELAY_TIMEOUT_HOURS = 2;
	private const int ONE_MINUTE_IN_MSEC = 60 * 1000;
	
	private const string MMC_DISTRIBUTOR_CODE = "MMC";
	private const string MMC_INTERNAL_SKU = "INTERNAL";
	
	private readonly IOrderService orderService;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly IOrderingProxy orderingProxy;
	private readonly IServiceProxy serviceProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<FulfillmentUpdateService> logger;
	private readonly IDistributorServiceLookup distributorServiceLookup;
	private readonly IOrderGenerator orderGenerator;
	
	private static NotificationFlag notifiedOrderExists = new NotificationFlag(false);

	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;

	public FulfillmentUpdateService(IOrderService orderService, 
									IProductSkuProxy productSkuProxy,
									IOrderingProxy orderingProxy,
									IOrderGenerator orderGenerator,
									IServiceProxy serviceProxy, 
									INotificationProxy notificationProxy, 
									IDistributorServiceLookup distributorServiceLookup,
									ILogger<FulfillmentUpdateService> logger)
	{
		this.orderService = orderService;
		this.productSkuProxy = productSkuProxy;
		this.orderingProxy = orderingProxy;
		this.orderService = orderService;
		this.serviceProxy = serviceProxy;
		this.orderGenerator = orderGenerator;
		this.distributorServiceLookup = distributorServiceLookup;
		this.logger = logger;
		this.notificationProxy = notificationProxy;

		//immediateProcessingRequestedToken = immediateProcessingRequestedTokenSource.Token;
	}

	public void ResumeProcessingNow()
	{
		notifiedOrderExists.Set();
		logger.LogInformation("Attempting to resume immediate processing of Orders and Fulfillment...");
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

				UpdateFulfillment();
				
				try
				{
					notifiedOrderExists.UnSet();
					int totalDelayTime = delayTimeoutHours * 60 * 60 * 1000;
					int oneMinuteLoops = totalDelayTime / ONE_MINUTE_IN_MSEC;
					for (int minute = 0; minute < oneMinuteLoops && 
					                     !stoppingToken.IsCancellationRequested && 
					                     !FulfillmentUpdateService.notifiedOrderExists.IsSet(); minute++)
					{

						await Task.Delay(ONE_MINUTE_IN_MSEC, stoppingToken);
					}
				}
				catch (OperationCanceledException)
				{
					// do nothing at the moment
				}
			}
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
			if (orderingProxy.DoesOrderExist(shopifyOrder.Id))
			{
				continue;
			}

			ProcessFulfillmentFor(shopifyOrder);
		}
	}

	private void ProcessFulfillmentFor(ShopifyOrder shopifyOrder)
	{
		var totalLineItems = shopifyOrder.LineItems == null ? 0 : shopifyOrder.LineItems.Count;
		logger.LogInformation($"New order in Shopify with Id {shopifyOrder.Id} with {totalLineItems} line items needing processing.");
		PrepareOrder(shopifyOrder);
	}

	private DbOrder? PrepareOrder(ShopifyOrder shopifyOrder)
	{
		try
		{
			var order = new DbOrder();
			order.OrderId = shopifyOrder.Id;
			if (shopifyOrder.OrderNumber != null)
			{
				order.OrderNumber = ((int)shopifyOrder.OrderNumber).ToString();
			}

			order.CheckoutId = shopifyOrder.CheckoutId == null ? 0 : (long)shopifyOrder.CheckoutId;
			order.CheckoutToken = shopifyOrder.CheckoutToken;
			order.CreatedDateTime = shopifyOrder.CreatedAt;
			order.ProcessStartDateTime = DateTime.Now;

			if (shopifyOrder.ShippingAddress != null)
			{
				order.DeliverToName = shopifyOrder.ShippingAddress?.Name;
				order.DeliverToAddress1 = shopifyOrder.ShippingAddress?.Address1;
				order.DeliverToAddress2 = shopifyOrder.ShippingAddress?.Address2;
				order.DeliverToCity = shopifyOrder.ShippingAddress?.City;
				order.DeliverToStateProv = shopifyOrder.ShippingAddress?.ProvinceCode;
				order.DeliverToZipPC = shopifyOrder.ShippingAddress?.Zip;
				order.DeliverToCountry = shopifyOrder.ShippingAddress?.CountryCode;
			}

			if (!string.IsNullOrEmpty(shopifyOrder.Note))
			{
				order.OrderNotes = shopifyOrder.Note.Trim();
			}

			if (shopifyOrder.LineItems.Count > 0) {
				PrepareFulfillment(shopifyOrder.LineItems, order);
			}

			orderingProxy.SaveOrder(order);
			return order;
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception processing Shopify Order {shopifyOrder.Id}: {ex}");
		}

		return null;
	}

	private void PrepareFulfillment(List<LineItem> lineItems, DbOrder order)
	{
		var poLookup = new Dictionary<string, int>();		// ensures that each distributor reuses the same PO for multiple line items in the same order
		foreach (var lineItem in lineItems)
		{
			var sku = lineItem.Sku;
			var poSequence = 0;
			int? distributorPO = null;

			var lineItemProperties = ExtractLineItemProperties(lineItem);

			var skuMap = string.IsNullOrEmpty(sku) ? null : productSkuProxy.GetSkuMapFor(sku);
			if (skuMap != null)
			{
				if (poLookup.ContainsKey(skuMap.DistributorCode))
				{
					poSequence = poLookup[skuMap.DistributorCode];
				}
				else
				{
					poSequence = orderGenerator.GetNextPOSequence();
					poLookup[skuMap.DistributorCode] = poSequence;
				}

				distributorPO = poSequence;
			
				logger.LogInformation($"Using PO {poSequence} for ordering {lineItem.Quantity} of SKU {lineItem.Sku} in Shopify Order {order.OrderId}");
				
// Question pending with Cory: Do we need to send an order to CapAmerica indicating we are fulfilling this item from in-house quantities
				var remaining = RemainingAfterFulfillingFromInHouseInventory(skuMap, lineItem);
				if (remaining > 0)
				{
					PrepareAndOrderLineItem(lineItem, remaining, lineItemProperties, order, poSequence, skuMap.DistributorCode, skuMap, null);
				}
			}
			
			// always duplicate caps to MMC and all non-cap items go to MMC
			if (poLookup.ContainsKey(MMC_DISTRIBUTOR_CODE))
			{
				poSequence = poLookup[MMC_DISTRIBUTOR_CODE];
			}
			else
			{
				poSequence = orderGenerator.GetNextPOSequence();
				poLookup[MMC_DISTRIBUTOR_CODE] = poSequence;
				logger.LogInformation($"Using PO {poSequence} for ordering for MMC in Shopify Order {order.OrderId}");
			}
			
			PrepareAndOrderLineItem(lineItem, lineItem.Quantity, lineItemProperties, order, poSequence, MMC_DISTRIBUTOR_CODE, skuMap, distributorPO);
		}
	}

	private ItemProperties ExtractLineItemProperties(LineItem lineItem)
	{
		var result = new ItemProperties();
		if (lineItem.Properties == null || lineItem.Properties.Count == 0)
		{
			return result;
		}

		foreach (var property in lineItem.Properties)
		{
			var value = property.Value == null ? "" : property.Value.Trim();
			switch (property.Name.ToUpper())
			{
				case "_SPECIAL INSTRUCTIONS":
					result.SpecialInstructions = value;
					break;
				case "TEXT":
					result.Text = value;
					break;
				case "POSITION":
					result.Position = value;
					break;
				case "_ADDONID":
					result.Correlation = value;
					break;
				case "UPLOAD IMAGE":
					result.ImageUrl = value;
					break;
			}
		}

		return result;
	}

	private int RemainingAfterFulfillingFromInHouseInventory(DistributorSkuMap skuMap, LineItem shopifyLineItem)
	{
		var remaining = shopifyLineItem.Quantity;
		var inHouse = productSkuProxy.GetInHouseInventoryFor(skuMap.Sku);
		if (inHouse != null && inHouse.OnHand > 0)
		{
			var toPick = inHouse.OnHand >= remaining ? remaining : inHouse.OnHand;
			remaining -= toPick;
			inHouse.OnHand -= toPick;
			inHouse.LastUsage = toPick;
			productSkuProxy.SaveInHouseInventory(inHouse);
		}
		return remaining;
	}

	private void PrepareAndOrderLineItem(LineItem shopifyLineItem, int quantity, ItemProperties lineItemProperties, 
										DbOrder order, int poSequence, string distributorCode, 
										DistributorSkuMap? skuMap, int? distributorPO)
	{
		var mmcCapCopy = false;
		var lineItem = new OrderLineItem();
		lineItem.LineItemId = shopifyLineItem.Id;
		lineItem.OrderId = order.OrderId;
		lineItem.ProductId = shopifyLineItem.ProductId == null ? 0 : (long)shopifyLineItem.ProductId;
		lineItem.VariantId = shopifyLineItem.VariantId == null ? 0 : (long)shopifyLineItem.VariantId;
		lineItem.Sku = shopifyLineItem.Sku == null ? "" : shopifyLineItem.Sku;
		lineItem.Name = skuMap == null ? shopifyLineItem.Name : "";
		if (distributorCode == MMC_DISTRIBUTOR_CODE)
		{
			mmcCapCopy = skuMap != null;
			lineItem.Name = skuMap == null ? shopifyLineItem.Name : "Cap";		// "Cap" or actual name is used later to sort items for MMC order
		}
		else
		{
			lineItem.Name = skuMap == null ? shopifyLineItem.Name : "";
		}
		if (lineItem.Name.Length > 50)
		{
			lineItem.Name = lineItem.Name.Substring(0, 50);
		}
		lineItem.Quantity = quantity;
		lineItem.Correlation = lineItemProperties.Correlation;
		lineItem.ImageOrText = string.IsNullOrEmpty(lineItemProperties.ImageUrl) ? lineItemProperties.Text : lineItemProperties.ImageUrl;
		lineItem.Position = lineItemProperties.Position;
		lineItem.SpecialInstructions = lineItemProperties.SpecialInstructions;
		lineItem.ShopifyName = shopifyLineItem.Name;
		if (lineItem.SpecialInstructions.Length > 4000)
		{
			lineItem.SpecialInstructions = lineItem.SpecialInstructions.Substring(0, 4000);
		}

		if (mmcCapCopy)
		{
			orderGenerator.GenerateOrderFor(distributorCode, skuMap, order.OrderId, shopifyLineItem.Quantity, poSequence, 
				lineItem.Name, lineItem.Correlation, lineItem.ImageOrText, lineItem.Position, 
				lineItem.SpecialInstructions, lineItem.ShopifyName, distributorPO);
		}
		else
		{
			order.OrderLineItems.Add(lineItem);

			if (skuMap == null && !string.IsNullOrEmpty(lineItem.Sku))
			{
				logger.LogError($"Unable to match SKU {shopifyLineItem.Sku} in Shopify Order {order.OrderId} for automated ordering");
				TransmitSkuNotFoundErrorMessage(shopifyLineItem, order);
			}
			else
			{
				orderGenerator.GenerateOrderFor(distributorCode, skuMap, order.OrderId, shopifyLineItem.Quantity, poSequence,
					lineItem.Name, lineItem.Correlation, lineItem.ImageOrText, lineItem.Position, lineItem.SpecialInstructions, lineItem.ShopifyName, null);
			}
		}
	}
	
	private void TransmitSkuNotFoundErrorMessage(LineItem shopifyLineItem, DbOrder order)
	{
		try
		{
			var subject = $"ERROR: Cannot find SKU - manual order is needed";
			
			var body = new StringBuilder();
			body.Append($"ERROR: CANNOT FIND SKU {shopifyLineItem.Sku} IN SHOPIFY ORDER {order.OrderNumber}!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"This SKU references an item called {shopifyLineItem.Name}.\r\n");
			body.Append($"Product Id in Shopify is {shopifyLineItem.ProductId}.\r\n");
			body.Append($"Variant Id in Shopify is {shopifyLineItem.VariantId}.\r\n");
			body.Append("\r\n");
			body.Append("The service cannot order this product.  It requires human intervention to send the order.\r\n\r\n");

			logger.LogInformation($"Transmitting ERROR message that cannot find SKU in Shopify Order {order.OrderId}.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that SKU cannot be found in Shopify Order {order.OrderId}: {ex}");
		}
	}

	internal class ItemProperties
	{
		public string Correlation { get; set; } = "";
		public string ImageUrl { get; set; } = "";
		public string Text { get; set; } = "";
		public string Position { get; set; } = "";
		public string SpecialInstructions { get; set; } = "";
	}
}