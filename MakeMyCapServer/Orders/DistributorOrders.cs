namespace MakeMyCapServer.Orders;

public class DistributorOrders
{
	public DateTime OrderDate { get; set; }
	public string PoNumber { get; set; }
	public long? ShopifyOrderId { get; set; }
	public string DistributorName { get; set; }
	public string DistributorLookupCode { get; set; }

	public List<IDistributorOrder> PurchaseOrders { get; } = new List<IDistributorOrder>();
}