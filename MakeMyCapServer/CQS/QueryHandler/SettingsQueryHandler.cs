using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class SettingsQueryHandler : IQueryHandler<SettingsQuery, SettingsResponse>
{
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<SettingsQueryHandler> logger;
	
	public SettingsQueryHandler(IServiceProxy serviceProxy, ILogger<SettingsQueryHandler> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
	}
	
	public SettingsResponse Handle(SettingsQuery query)
	{
		var response = new SettingsResponse();
		var hours = serviceProxy.GetInventoryCheckHours();
		response.InventoryCheckHours = hours == null ? 24 : (int)hours;
		
		hours = serviceProxy.GetFulfillmentCheckHours();
		response.FulfillmentCheckHours = hours == null ? 24 : (int) hours;

		response.NextPoSequence = serviceProxy.GetNextPoNumberSequence();

		return response;
	}
}