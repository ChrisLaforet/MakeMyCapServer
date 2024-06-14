using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class PasswordRecoveryCommandHandler : ICommandHandler<PasswordRecoveryCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<PasswordRecoveryCommandHandler> logger;
	
	public PasswordRecoveryCommandHandler(IUserProxy userProxy, ILogger<PasswordRecoveryCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(PasswordRecoveryCommand command)
	{
		logger.LogInformation($"Request to change password for user with Email of {command.Email}");

		try
		{
			userProxy.ChangePasswordFor(command.Email);
		}
		catch (Exception ex)
		{
			logger.LogError($"Failure to change password for a user with Email of {command.Email} with exception: {ex}");
		}

		return new NothingnessResponse();
	}
}