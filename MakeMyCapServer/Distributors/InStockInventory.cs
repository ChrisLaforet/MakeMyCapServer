namespace MakeMyCapServer.Distributors;

public class InStockInventory
{
	public string Sku { get; set; }
	public string? OurSku { get; set; }
	public long? Quantity { get; set; }

	public bool IsNotFound()
	{
		return Quantity == null;
	}
}