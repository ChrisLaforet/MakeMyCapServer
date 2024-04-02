using System.Text;
using Microsoft.Extensions.Primitives;

namespace MakeMyCapServer.Orders;

public static class OrderWriter
{
	public const string CAP_MONIKER = "Cap";
	
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
		output.Append("Order #: ");
		if (string.IsNullOrEmpty(orders.ShopifyOrderNumber))
		{
			output.Append("Not provided\r\n");
		}
		else
		{
			output.Append($"{orders.ShopifyOrderNumber}\r\n");
		}
		
		output.Append("\r\nDelivery to:\r\n\r\n");
		if (!string.IsNullOrEmpty(orders.DeliverToName))
		{
			output.Append($"{orders.DeliverToName}\r\n");
		}
		if (!string.IsNullOrEmpty(orders.DeliverToAddress1))
		{
			output.Append($"{orders.DeliverToAddress1}\r\n");
		}
		if (!string.IsNullOrEmpty(orders.DeliverToAddress2))
		{
			output.Append($"{orders.DeliverToAddress2}\r\n");
		}
		output.Append($"{orders.DeliverToCity}, {orders.DeliverToStateProv} {orders.DeliverToZipPC}  {orders.DeliverToCountry}\r\n");

		output.Append("\r\nLine items:\r\n\r\n");
		foreach (var group in GroupAndSortOrders(orders.PurchaseOrders))
		{
			foreach (var order in group)
			{
				output.Append($"{order.Quantity}x");
				if (!string.IsNullOrEmpty(order.Sku))
				{
					output.Append($"  Sku: {order.Sku}");
				}
				else if (!string.IsNullOrEmpty(order.Name))
				{
					output.Append($"  Name: {order.Name}");
				}

				if (!string.IsNullOrEmpty(order.Style))
				{
					output.Append($"  Style: {order.Style}");
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
				
				if (!string.IsNullOrEmpty(order.ShopifyName))
				{
					output.Append($"   Shopify Name: {order.ShopifyName}");
					output.Append("\r\n");
				}

				if (!string.IsNullOrEmpty(order.SupplierPoNumber))
				{
					output.Append($"   Supplier/Supplier PO: {order.Supplier} {order.SupplierPoNumber}");
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
		}

		return output.ToString();
	}

	private static List<List<IDistributorOrder>> GroupAndSortOrders(List<IDistributorOrder> orders)
	{
		var groups = new List<List<IDistributorOrder>>();

		var correlations = orders.Select(o => o.Correlation).ToHashSet();
		foreach (var correlation in correlations)
		{
			var matches = orders.FindAll(o => o.Correlation == correlation).ToList();
			var group = new List<IDistributorOrder>();
			
			// output order is Cap, Front, Left, Right, Back, Anything else
			group.AddRange(matches.FindAll(o => o.Name == CAP_MONIKER).ToList());
			group.AddRange(matches.FindAll(o => o.Position.ToUpper().StartsWith("FRONT")).ToList());
			group.AddRange(matches.FindAll(o => o.Position.ToUpper().StartsWith("LEFT")).ToList());
			group.AddRange(matches.FindAll(o => o.Position.ToUpper().StartsWith("RIGHT")).ToList());
			group.AddRange(matches.FindAll(o => o.Position.ToUpper().StartsWith("BACK")).ToList());
			group.AddRange(matches.Where(o => !group.Contains(o)).ToList());

			groups.Add(group);
		}
		
		return groups;
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