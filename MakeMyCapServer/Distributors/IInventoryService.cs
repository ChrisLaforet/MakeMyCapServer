namespace MakeMyCapServer.Distributors;

public interface IInventoryService
{
	List<InStockInventory> GetInStockInventoryFor(List<string> skus);
}