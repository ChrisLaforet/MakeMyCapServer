using MakeMyCapAdmin.CQS.Command;
using MakeMyCapAdmin.CQS.Response;
using MakeMyCapAdmin.Proxies;

namespace MakeMyCapAdmin.CQS.CommandHandler;

public class ChangeNotificationsCommandHandler : ICommandHandler<ChangeNotificationsCommand, NothingnessResponse>
{
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<ChangeNotificationsCommandHandler> logger;
	
	public ChangeNotificationsCommandHandler(IServiceProxy serviceProxy, ILogger<ChangeNotificationsCommandHandler> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(ChangeNotificationsCommand command)
	{
		try
		{
			logger.LogInformation("Request to change notification emails");
			
			serviceProxy.SetStatusEmailRecipients(command.WarningEmail1, command.WarningEmail2, command.WarningEmail3);
			serviceProxy.SetCriticalEmailRecipients(command.CriticalEmail1, command.CriticalEmail2, command.CriticalEmail3);
		}
		catch (Exception ex)
		{
			logger.LogError($"Exception prevents changing notification emails: {ex}");
		}
		return new NothingnessResponse();
	}
}
