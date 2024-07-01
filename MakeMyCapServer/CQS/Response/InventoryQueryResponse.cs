namespace MakeMyCapServer.CQS.Response;

public class InventoryQueryResponse
{
	public string Sku { get; }
	public string Description { get; }
	public int OnHand { get; }
	public int LastUsage { get; }
	public string DistributorCode { get; }
	public string DistributorName { get; }

	public InventoryQueryResponse(string sku, string description, int onHand, int lastUsage, string distributorCode, string distributorName)
	{
		Sku = sku;
		Description = description;
		OnHand = onHand;
		LastUsage = lastUsage;
		DistributorCode = distributorCode;
		DistributorName = distributorName;
	}
}