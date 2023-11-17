namespace MakeMyCapServer.Distributors.MakeMyCap;

public class MakeMyCapInventoryService : IInventoryService
{
	// A mock object since MMC doesn't maintain inventory

	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
		return new List<InStockInventory>();
	}
}