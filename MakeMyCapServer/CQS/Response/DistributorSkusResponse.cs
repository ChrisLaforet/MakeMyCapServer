namespace MakeMyCapServer.CQS.Response;

public class DistributorSkusResponse
{
	public string DistributorName { get; }
	public string DistributorCode { get; }
	public List<AssignedSku> Skus { get; }

	public DistributorSkusResponse(string distributorName, string distributorCode, List<AssignedSku> skus)
	{
		DistributorName = distributorName;
		DistributorCode = distributorCode;
		Skus = skus;
	}
}