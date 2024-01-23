using MakeMyCapServer.Configuration;
using MakeMyCapServer.Controllers;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.Security;

public class TokenValidationMiddleware
{
	private readonly RequestDelegate nextDelegate;

	public TokenValidationMiddleware(RequestDelegate nextDelegate)
	{
		this.nextDelegate = nextDelegate;
	}

	public async Task Invoke(HttpContext context, IUserProxy userProxy)
	{
		var claims = context.User?.Claims;
		if (claims != null)
		{
			foreach (var claim in claims)
			{
				if (claim.Type == LoginController.TOKEN_NAME &&
					!ValidateUserToken(context, userProxy, claim.Value))
				{
						context.Response.Redirect("/Login/LogOff");
						return;
				}
			}
		}

		await nextDelegate(context);
	}

	private bool ValidateUserToken(HttpContext context, IUserProxy userProxy, string token)
	{
		try
		{
			if (context.Request.Path.ToString().StartsWith("/Login"))
			{
				return true;
			}
			
			var userToken = userProxy.ValidateUserToken(token);
			if (userToken != null)
			{
				return true;
			}
		}
		catch
		{
			// do nothing if validation fails
			// user is not attached to context so request won't have access to secure routes
		}

		return false;
	}
}