using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class TokenValidationCommandHandler : ICommandHandler<TokenValidationCommand, NothingnessResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<TokenValidationCommandHandler> logger;

	public TokenValidationCommandHandler(IUserProxy userProxy, ILogger<TokenValidationCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(TokenValidationCommand command)
	{

		var userToken = userProxy.ValidateUserToken(command.Token);
		if (userToken == null)
		{
			throw new InvalidTokenException();
		}

		return new NothingnessResponse();
	}
}