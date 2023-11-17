using MakeMyCapServer.Orders;

namespace MakeMyCapServer.Model;

public partial class PurchaseOrder : IOrder
{
	public DateTime OrderDate => CreateDate;
	
	public string PoNumber => Ponumber;

	public List<IOrderItem> LineItems
	{
		get
		{
			var lineItems = new List<IOrderItem>();
			lineItems.Add(new OrderItem()
			{
				Sku = this.Sku,
				Style = this.Style,
				Color = this.Color,
				Size = this.Size,
				Quantity = this.Quantity
			});
			return lineItems;
		}
	}

	public string DistributorName
	{
		get
		{
			if (!string.IsNullOrEmpty(Distributor.Name))
			{
				return Distributor.Name;
			}

			return string.Empty;
		}
	}
}


internal class OrderItem : IOrderItem
{
	public string Sku { get; set; }

	public int Quantity { get; set; }

	public string? Style { get; set; }

	public string? Color { get; set; }

	public string? Size { get; set; }	
}