using MakeMyCapServer.Services.Email;
using System.Text;
using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Distributors.MakeMyCapInStock;


public class MakeMyCapInStockOrderService : IOrderService
{
	public const string ORDER_EMAIL_ADDRESS = "orders@makemycap.com";

	private readonly IEmailQueueService emailQueueService;
	private ILogger<MakeMyCapInStockOrderService> logger;
	
	public MakeMyCapInStockOrderService(IEmailQueueService emailQueueService, ILogger<MakeMyCapInStockOrderService> logger)
	{
		this.emailQueueService = emailQueueService;
		this.logger = logger;
	} 
	
	public OrderStatus PlaceOrder(DistributorOrders orders)
	{
		var subject = $"MMC Internal Inventory Pull Request (PO {orders.PoNumber})";
		var body = new StringBuilder();
		body.Append($"The following items need to be pulled from MMC internal inventory\r\n\r\n");
		body.Append(FormatOrder(orders));
		body.Append("\r\n");
			
		emailQueueService.Add(ORDER_EMAIL_ADDRESS, subject, body.ToString());
		return new OrderStatus(true);
	}
	
	private string? FormatOrder(DistributorOrders orders)
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

			if (!string.IsNullOrEmpty(order.Brand))
			{
				output.Append($"  Brand: {order.Brand}");
			}
			
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