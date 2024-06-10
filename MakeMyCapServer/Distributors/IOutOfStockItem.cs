using MakeMyCapServer.Model;

namespace MakeMyCapServer.Services.OrderPlacement;

public interface IOutOfStockItem
{
	public DistributorSkuMap? DistributorSkuMap { get; }
	
	public string Description { get; }
	
	public int OrderedQuantity { get; }
	public int AvailableQuantity { get; }
}