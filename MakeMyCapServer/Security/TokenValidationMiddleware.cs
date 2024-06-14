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
		var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (token != null)
		{
			var userToken = userProxy.ValidateUserToken(token);
			if (userToken != null)
			{
				context.Items["User"] = userToken.User;
			}
		}

		await nextDelegate(context);
	}
}