using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class NotificationsQueryHandler : IQueryHandler<NotificationsQuery, NotificationsResponse>
{
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<NotificationsQueryHandler> logger;
	
	public NotificationsQueryHandler(IServiceProxy serviceProxy, ILogger<NotificationsQueryHandler> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
	}
	
	public NotificationsResponse Handle(NotificationsQuery query)
	{
		var response = new NotificationsResponse();
		var emails = serviceProxy.GetStatusEmailRecipients();
		if (emails.Count > 0)
		{
			response.WarningEmail1 = emails[0];
			if (emails.Count > 2)
			{
				response.WarningEmail3 = emails[2];
			}

			if (emails.Count > 1)
			{
				response.WarningEmail2 = emails[1];
			}
		}
		
		emails = serviceProxy.GetCriticalEmailRecipients();
		if (emails.Count > 0)
		{
			response.CriticalEmail1 = emails[0];
			if (emails.Count > 2)
			{
				response.CriticalEmail3 = emails[2];
			}

			if (emails.Count > 1)
			{
				response.CriticalEmail2 = emails[1];
			}
		}

		return response;
	}
}