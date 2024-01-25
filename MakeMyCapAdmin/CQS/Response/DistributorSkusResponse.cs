namespace MakeMyCapAdmin.CQS.Response;

public class DistributorSkusResponse
{
	public string DistributorName { get; }
	public string DistributorCode { get; }
	public List<AssignedSkuResponse> Skus { get; }

	public DistributorSkusResponse(string distributorName, string distributorCode, List<AssignedSkuResponse> skus)
	{
		DistributorName = distributorName;
		DistributorCode = distributorCode;
		Skus = skus;
	}
}