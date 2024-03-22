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

		output.Append("\r\nLine items:\r\n\r\n");
		foreach (var order in orders.PurchaseOrders)
		{
			output.Append($"{order.Quantity}x   Style: {order.Style}");

			if (!string.IsNullOrEmpty(order.Color))
			{
				output.Append($"  Color: {order.Color}");
			}
			
			if (!string.IsNullOrEmpty(order.Size))
			{
				output.Append($"  Size: {order.Size}");
			}

			output.Append("\r\n");
		}

		return output.ToString();
	}
}