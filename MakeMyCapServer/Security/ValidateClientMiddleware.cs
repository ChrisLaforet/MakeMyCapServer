using System.Threading.Tasks;
using MakeMyCapServer.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;


namespace MakeMyCapServer.Security;

public class ValidateClientMiddleware
{
	private const string APIKEY_HEADER_KEY = "apikey";
	private const string APIKEY_STRING_KEY = "Apikey";

	private readonly RequestDelegate next;
	private readonly string apikey;

	public ValidateClientMiddleware(RequestDelegate next, IConfigurationLoader configurationLoader)
	{
		this.next = next;
		apikey = configurationLoader.GetKeyValueFor(APIKEY_STRING_KEY);
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
		return builder.UseMiddleware<ValidateClientMiddleware>();
	}
}
