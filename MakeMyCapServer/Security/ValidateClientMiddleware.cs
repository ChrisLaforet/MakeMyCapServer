using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.Security;

public class ValidateClientMiddleware
{
	private readonly RequestDelegate nextDelegate;

	public ValidateClientMiddleware(RequestDelegate nextDelegate)
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

public static class ValidateClientMiddlewareExtensions
{
	public static IApplicationBuilder UseValidateClient(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ValidateClientMiddleware>();
	}
}