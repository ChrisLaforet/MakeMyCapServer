namespace MakeMyCapServer.CQS.Query;

public class ServiceStatusQuery : IQuery
{
	public string ServiceName { get; }

	public ServiceStatusQuery(string serviceName)
	{
		ServiceName = serviceName;
	}
}