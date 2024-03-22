using System.Text;
using Microsoft.Extensions.Primitives;

namespace MakeMyCapServer.Orders;

public static class OrderWriter
{
	public static string FormatOrder(DistributorOrders orders)
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
		output.Append("Line items:\r\n");
		foreach (var order in orders.PurchaseOrders)
		{
			output.Append($"{order.Quantity}");
			if (!string.IsNullOrEmpty(order.Sku))
			{
				output.Append($"  Sku: {order.Sku}");
			}
			else if (!string.IsNullOrEmpty(order.Name))
			{
				output.Append($"  Name: {order.Name}");
			}

			if (order.Style != null)
			{
				output.Append($"  Style: {order.Style}");
			}

			if (order.Color != null)
			{
				output.Append($"  Color: {order.Color}");
			}

			if (order.Size != null)
			{
				output.Append($"  Size: {order.Size}");
			}
			output.Append("\r\n");

			if (!string.IsNullOrEmpty(order.ImageOrText))
			{
				if (!string.IsNullOrEmpty(order.Position))
				{
					output.Append($"   Position: {order.Position}");
				}
				
				if (order.ImageOrText.StartsWith("http"))
				{
					output.Append($"  Image URL: {order.ImageOrText}");
				}
				else
				{
					output.Append($"  Text: {order.ImageOrText}");
				}
				output.Append("\r\n");
			}
			
			if (!string.IsNullOrEmpty(order.Correlation))
			{
				output.Append($"   Correlation: {order.Correlation}");
				output.Append("\r\n");
			}
			
			if (!string.IsNullOrEmpty(order.SpecialInstructions))
			{
				output.Append($"   Special Instructions: {order.SpecialInstructions}");
				output.Append("\r\n");
			}
			
			output.Append("\r\n");
		}

		return output.ToString();
	}

	public static string FormatOrder(IDistributorOrder order)
	{
		var output = new StringBuilder();

		output.Append("Order Details\r\n");
		output.Append("\r\n");
		output.Append($"Order date: {order.OrderDate.ToString("MM/dd/yyyy")}\r\n");
		output.Append($"PO Number: {order.PoNumber}\r\n");
		output.Append("Shopify order Id: ");
		if (order.ShopifyOrderId == null)
		{
			output.Append("Not provided\r\n");
		}
		else
		{
			output.Append($"{order.ShopifyOrderId.ToString()}\r\n");
		}

		output.Append("Line item:\r\n");

		output.Append($"{order.Quantity}");
		if (order.Sku != null)
		{
			output.Append($"  Sku: {order.Sku}");
		}

		if (order.Style != null)
		{
			output.Append($"  Style: {order.Style}");
		}

		if (order.Color != null)
		{
			output.Append($"  Color: {order.Color}");
		}

		if (order.Size != null)
		{
			output.Append($"  Size: {order.Size}");
		}

		output.Append("\r\n");

		return output.ToString();
	}
}