namespace MakeMyCapServer.CQS.Response;

public class AvailableSkuResponse
{
	public string Sku { get; }
	public string Description { get; }
	public string DistributorCode { get; }
	public string DistributorName { get; }

	public AvailableSkuResponse(string sku, string description, string distributorCode, string distributorName)
	{
		Sku = sku;
		Description = description;
		DistributorCode = distributorCode;
		DistributorName = distributorName;
	}
}