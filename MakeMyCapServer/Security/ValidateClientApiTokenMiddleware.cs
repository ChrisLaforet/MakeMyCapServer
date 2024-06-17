using MakeMyCapServer.Configuration;

namespace MakeMyCapServer.Security;

public class ValidateClientApiTokenMiddleware
{
	private const string APIKEY_STRING_CONFIGURATION_KEY = "Apikey";
	private const string APIKEY_HEADER_KEY = "X-Api-Key";
	
	private readonly RequestDelegate next;
	private readonly string apikey;
	
	public ValidateClientApiTokenMiddleware(RequestDelegate next, IConfigurationLoader configurationLoader)
	{
		this.next = next;
		apikey = configurationLoader.GetKeyValueFor(APIKEY_STRING_CONFIGURATION_KEY);
	}

	public async Task InvokeAsync(HttpContext context)
	{
		if (!context.Request.Headers.ContainsKey(APIKEY_HEADER_KEY))
		{
			context.Response.StatusCode = 403; //forbidden 
			await context.Response.WriteAsync("Invalid apikey");
			return;
		}

		var apikeyFromHeader = context.Request.Headers[APIKEY_HEADER_KEY];
		if (apikeyFromHeader != apikey)
		{
			context.Response.StatusCode = 401; // Unauthorized
			await context.Response.WriteAsync("Invalid apikey");
			return;
		}

		// Call the next delegate/middleware in the pipeline.
		await this.next(context);
	}
}

public static class ValidateClientMiddlewareExtensions
{
	public static IApplicationBuilder UseValidateClient(this IApplicationBuilder builder)
	{
		return builder.UseMiddleware<ValidateClientApiTokenMiddleware>();
	}
}

