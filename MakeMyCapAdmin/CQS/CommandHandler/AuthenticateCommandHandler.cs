﻿using MakeMyCapAdmin.CQS.Command;
using MakeMyCapAdmin.CQS.Response;
using MakeMyCapAdmin.Proxies;

namespace MakeMyCapAdmin.CQS.CommandHandler;

public class AuthenticateCommandHandler : ICommandHandler<AuthenticateCommand, UserResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<AuthenticateCommandHandler> logger;
	
	public AuthenticateCommandHandler(IUserProxy userProxy, ILogger<AuthenticateCommandHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public UserResponse Handle(AuthenticateCommand command)
	{
		logger.LogInformation($"Request to log in {command.UserName}");
		userProxy.ExpireUserTokens();

		try
		{
			var authenticatedUser = userProxy.AuthenticateUser(command.UserName, command.Password);
			logger.LogInformation($"Successful login of user {command.UserName}");
			return new UserResponse(authenticatedUser.UserName, authenticatedUser.Email, authenticatedUser.Token);
		}
		catch (Exception ex)
		{
			logger.LogError($"Failure to log in {command.UserName} with exception: {ex}");
			throw;
		}
	}
}