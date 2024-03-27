namespace MakeMyCapServer.Orders;

public class DistributorOrders
{
	public DateTime OrderDate { get; set; }
	public string PoNumber { get; set; }
	public string ShopifyOrderNumber { get; set; }
	public long? ShopifyOrderId { get; set; }
	public string DistributorName { get; set; }
	public string DistributorLookupCode { get; set; }
	
	public string? DeliverToName { get; set; }
	public string? DeliverToAddress1 { get; set; }
	public string? DeliverToAddress2 { get; set; }
	public string? DeliverToCity { get; set; }
	public string? DeliverToStateProv { get; set; }
	public string? DeliverToZipPC { get; set; }
	public string? DeliverToCountry { get; set; }
	
	public List<IDistributorOrder> PurchaseOrders { get; } = new List<IDistributorOrder>();
}