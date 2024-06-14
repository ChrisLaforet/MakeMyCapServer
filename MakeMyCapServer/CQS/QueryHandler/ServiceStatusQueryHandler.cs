using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class ServiceStatusQueryHandler : IQueryHandler<ServiceStatusQuery, List<ServiceLogResponse>>
{
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<ServiceStatusQueryHandler> logger;
	
	public ServiceStatusQueryHandler(IServiceProxy serviceProxy, ILogger<ServiceStatusQueryHandler> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
	}
	
	public List<ServiceLogResponse> Handle(ServiceStatusQuery query)
	{
		var logs = serviceProxy.GetLastServiceLogsFor(query.ServiceName, 10);
		var response = new List<ServiceLogResponse>();
		foreach (var log in logs)
		{
			response.Add(new ServiceLogResponse(log));
		}

		return response;
	}
}
