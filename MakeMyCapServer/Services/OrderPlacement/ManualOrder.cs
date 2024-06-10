using MakeMyCapServer.Model;

namespace MakeMyCapServer.Services.OrderPlacement;

public class ManualOrder
{
	public DistributorSkuMap DistributorSkuMap { get; private set; }
	public int OrderedQuantity { get; }
	public int AvailableQuantity { get; }
	
	public string PoNumber { get; }
	
	public long? ShopifyOrderId { get; }
	
	public string? DistributorName { get; }
	
	public string Description { get; }

	public ManualOrder(IOutOfStockItem item, string poNumber, long? shopifyOrderId, string? distributorName) 
	{
		DistributorSkuMap = item.DistributorSkuMap!;
		Description = item.Description;
		OrderedQuantity = item.OrderedQuantity;
		AvailableQuantity = item.AvailableQuantity;
		PoNumber = poNumber;
		ShopifyOrderId = shopifyOrderId;
		DistributorName = distributorName;
	}
}