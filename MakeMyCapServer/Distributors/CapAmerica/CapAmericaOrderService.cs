using System.Security.Cryptography.Pkcs;
using System.Text;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Distributors.CapAmerica;

public class CapAmericaOrderService : IOrderService
{
	public const string ORDER_EMAIL_ADDRESS = "mmc.orders@capamerica.com";

	private readonly IProductSkuProxy productSkuProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly IEmailQueueService emailQueueService;
	private readonly ILogger<CapAmericaInventoryService> logger;
	
	public CapAmericaOrderService(IProductSkuProxy productSkuProxy, 
								INotificationProxy notificationProxy, 
								IEmailQueueService emailQueueService, 
								ILogger<CapAmericaInventoryService> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.notificationProxy = notificationProxy;
		this.emailQueueService = emailQueueService;
		this.logger = logger;
	} 
	
	public bool PlaceOrder(DistributorOrders orders)
	{
// TODO: CML - determine what additional formatting is needed here		

		var orderBody = FormatOrder(orders);
		if (orderBody == null)
		{
			logger.LogError($"No line items remain in PO {orders.PoNumber} so there is nothing to transmit to CapAmerica.");
			return true;
		}
		var subject = $"MMC Purchase Order - PO {orders.PoNumber}";
		var body = new StringBuilder();
		body.Append("Customer: Make My Cap\r\n");
		body.Append(orderBody);
		body.Append("\r\n");
		body.Append("Deliver to Decorated - Annex building, Cap America.\r\n\r\n");
			
		emailQueueService.Add(ORDER_EMAIL_ADDRESS, subject, body.ToString());
		foreach (var order in orders.PurchaseOrders)
		{
			order.SuccessDateTime = DateTime.Now;
		}
		return true;
	}

	public string? FormatOrder(DistributorOrders orders)
	{
		var lookup = productSkuProxy.GetSkuMapsFor(CapAmericaInventoryService.CAPAMERICA_DISTRIBUTOR_CODE);

		var output = new StringBuilder();

		output.Append("Order Details\r\n");
		output.Append("\r\n");
		output.Append($"Order date: {orders.OrderDate.ToString("MM/dd/yyyy")}\r\n");
		output.Append($"PO Number: {orders.PoNumber}\r\n");
		output.Append("Shopify order Id: ");
		if (orders.ShopifyOrderId == null)
		{
			output.Append("Not provided\r\n");
		}
		else
		{
			output.Append($"{orders.ShopifyOrderId.ToString()}\r\n");
		}

		var notFoundSkus = new List<IDistributorOrder>();

		output.Append("Line items:\r\n");
		foreach (var order in orders.PurchaseOrders)
		{
			var map = lookup.SingleOrDefault(map => string.Compare(map.Sku, order.Sku, true) == 0);
			if (map == null)
			{
				logger.LogError($"Unable to map sku {order.Sku} to place order for CapAmerica!");
				notFoundSkus.Add(order);
				continue;
			}
			
			output.Append($"{order.Quantity}   Style: {map.StyleCode}");
			if (!string.IsNullOrEmpty(map.PartId))
			{
				output.Append($"  Part Id: {map.PartId}");
			}

			if (!string.IsNullOrEmpty(map.Color))
			{
				output.Append($"  Color: {map.Color}");
			}
			
			if (!string.IsNullOrEmpty(map.SizeCode))
			{
				output.Append($"  Size: {map.SizeCode}");
			}

			output.Append("\r\n");
		}

		if (notFoundSkus.Count > 0)
		{
			NotifyOfMissingSkuMatches(notFoundSkus);
		}

		if (notFoundSkus.Count == orders.PurchaseOrders.Count)
		{
			return null;
		}
		
		return output.ToString();
	}
	
	private void NotifyOfMissingSkuMatches(List<IDistributorOrder> notFoundSkus)
	{
		var subject = $"Urgent! Order SKUs for CapAmerica cannot be found for sending PO {notFoundSkus[0].PoNumber}";

		var body = new StringBuilder();
		body.Append($"The following Line Items for PO {notFoundSkus[0].PoNumber} cannot be found in our mappings.\r\n");
		body.Append($"PLEASE ORDER THESE ITEMS MANUALLY NOW.  Once done, the SKU(s) need to be mapped in our mapping table.\r\n\r\n");
		foreach (var notFoundSku in notFoundSkus)
		{
			body.Append($"   Our SKU: {notFoundSku.Sku}\r\n  Style: {notFoundSku.Style}\r\n  Color: {notFoundSku.Color}\r\n  Size: {notFoundSku.Size}\r\n  Quantity: {notFoundSku.Quantity}\r\n");
		}

		body.Append("\r\n");
		body.Append("Any other items on this PO that were located will be ordered electronically.");
		body.Append("\r\n\r\n");

		logger.LogInformation($"Transmitting critical message concerning inability to map all SKUs in PO {notFoundSkus[0].PoNumber}.");
		notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
	}
}