using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class ChangePasswordCommandHandler : ICommandHandler<ChangePasswordCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<ChangePasswordCommandHandler> logger;
	
	public ChangePasswordCommandHandler(IUserProxy userProxy, ILogger<ChangePasswordCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(ChangePasswordCommand command)
	{
		logger.LogInformation($"Request to change password for {command.UserName}");
		userProxy.ExpireUserTokens();

		try
		{
			userProxy.SetPasswordFor(command.UserName, command.ResetKey, command.NewPassword);
			logger.LogInformation($"Successfully change password for user {command.UserName}");
			return new NothingnessResponse();
		}
		catch (Exception ex)
		{
			logger.LogError($"Failure to log in {command.UserName} with exception: {ex}");
			throw;
		}
	}
}
