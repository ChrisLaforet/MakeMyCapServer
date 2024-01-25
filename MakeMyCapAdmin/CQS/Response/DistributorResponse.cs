namespace MakeMyCapAdmin.CQS.Response;

public class DistributorResponse
{
	public string DistributorName { get; }
	public string DistributorCode { get; }

	public DistributorResponse(string distributorName, string distributorCode)
	{
		DistributorName = distributorName;
		DistributorCode = distributorCode;
	}
}