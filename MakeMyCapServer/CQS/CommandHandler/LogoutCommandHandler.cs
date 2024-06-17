using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class LogoutCommandHandler : ICommandHandler<LogoutCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<LogoutCommandHandler> logger;

	public LogoutCommandHandler(IUserProxy userProxy, ILogger<LogoutCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(LogoutCommand command)
	{
		try
		{
			logger.LogInformation($"Request to log out {command.Username}");
			userProxy.LogoutUser(command.Username);
		}
		catch (System.Exception ex)
		{
			logger.LogDebug($"Error while logging out {command.Username} due to {ex}");
		}

		return new NothingnessResponse();
	}
}