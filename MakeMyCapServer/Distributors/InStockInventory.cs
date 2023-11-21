namespace MakeMyCapServer.Distributors;

public class InStockInventory
{
	public string Sku { get; set; }
	public string? DistributorSku { get; set; }
	public long? Quantity { get; set; }

	public bool IsNotFound()
	{
		return Quantity == null;
	}
}