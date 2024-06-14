using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<CreateUserCommandHandler> logger;
	
	public CreateUserCommandHandler(IUserProxy userProxy, ILogger<CreateUserCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(CreateUserCommand command)
	{
		try
		{
			logger.LogInformation($"Request create user for userName of {command.UserName} and Email of {command.UserEmail}");

			var user = userProxy.CreateUser(command.UserName, command.UserEmail);
			userProxy.ChangePasswordFor(user.Email);
		}
		catch (Exception ex)
		{
			logger.LogError($"Exception prevents creating new user: {ex}");
		}
		return new NothingnessResponse();
	}
}
