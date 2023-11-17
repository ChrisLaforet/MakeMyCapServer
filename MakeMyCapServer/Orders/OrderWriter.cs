using System.Text;
using Microsoft.Extensions.Primitives;

namespace MakeMyCapServer.Orders;

public static class OrderWriter
{
	public static string FormatOrder(IOrder order)
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
		output.Append("Line items:\r\n");
		foreach (var lineItem in order.LineItems)
		{
			output.Append($"{lineItem.Quantity}");
			if (lineItem.Sku != null)
			{
				output.Append($"  Sku: {lineItem.Sku}");
			}

			if (lineItem.Style != null)
			{
				output.Append($"  Style: {lineItem.Style}");

			}

			if (lineItem.Color != null)
			{
				output.Append($"  Color: {lineItem.Color}");

			}

			if (lineItem.Size != null)
			{
				output.Append($"  Size: {lineItem.Size}");

			}
			output.Append("\r\n");
		}

		return output.ToString();
	}
}