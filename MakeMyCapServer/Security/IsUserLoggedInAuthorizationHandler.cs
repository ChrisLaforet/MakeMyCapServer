using MakeMyCapServer.Proxies;
using Microsoft.AspNetCore.Authorization;

namespace MakeMyCapServer.Security;

// See: https://learn.microsoft.com/en-us/aspnet/core/security/authorization/iard?view=aspnetcore-8.0

public class IsUserLoggedInAuthorizationHandler : AuthorizationHandler<LoggedInRequirement>
{
	// public const string AUTHORIZATION_KEY = "authorization";
	// public const string BEARER_HEADER = "Bearer ";
	//
	// private readonly IUserProxy userProxy;
	private readonly ILogger<IsUserLoggedInAuthorizationHandler> logger;

	public IsUserLoggedInAuthorizationHandler(ILogger<IsUserLoggedInAuthorizationHandler> logger)
	{
		this.logger = logger;
	}

	// Check whether a given MinimumAgeRequirement is satisfied or not for a particular
	// context.
	protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, LoggedInRequirement requirement)
	{
		// // Checks the bearer JWT token
		// var bearer = context.HttpContext.Request.Headers[AUTHORIZATION_KEY];
		// if (!bearer.IsNullOrEmpty())
		// {
		// 	var token = bearer[0];
		// 	if (token.StartsWith(BEARER_HEADER))
		// 	{
		// 		token = token.Substring(BEARER_HEADER.Length);
		// 	}
		//
		// 	if (ValidateBearerToken(token))
		// 	{
		// 		context.Succeed(requirement);
		// 	}
		// 	logger.LogInformation("User's token is expired or not logged in!");
		// }
		// else
		// {
		// 	logger.LogInformation("User does not present a valid Bearer token!");
		// }
		
		if (context.User.Identity?.IsAuthenticated == true)
		{
			logger.LogDebug("User is logged in!");
			context.Succeed(requirement);
		}
		else
		{
			logger.LogInformation("User is not logged in!");
			context.Fail();
		}

		return Task.CompletedTask;
	}

	// private bool ValidateBearerToken(string token)
	// {
	// 	logger.LogWarning("UserProxy is null!");
	//
	// 	var user = userProxy.ValidateUserToken(token);
	// 	return user != null;
	// }
}