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
		logger.LogInformation($"Request to change password for user with identity of {command.UserIdentity}");

		try
		{
			var user = userProxy.GetUserByUsername(command.UserIdentity);
			if (user == null)
			{
				user = userProxy.GetUserByEmail(command.UserIdentity);
			}

			if (user != null)
			{
				userProxy.ChangePasswordFor(user.Email);
			}
			else
			{
				logger.LogError($"Failure to change password for a user with identity of {command.UserIdentity} since no such identity exists as a username or an email.");
			}
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Failure to change password for a user with identity of {command.UserIdentity} with exception: {ex}");
		}

		return new NothingnessResponse();
	}
}