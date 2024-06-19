using System.Security.Claims;
using System.Text.Encodings.Web;
using MakeMyCapServer.Proxies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace MakeMyCapServer.Security;

public class MakeMyCapAuthenticationHandler : AuthenticationHandler<MakeMyCapAuthenticationOptions>
{
	public const string AUTHORIZATION_KEY = "authorization";
	public const string BEARER_HEADER = "Bearer ";
	
	private readonly IUserProxy userProxy;
	private readonly ILogger<MakeMyCapAuthenticationHandler> logger;
	
	public MakeMyCapAuthenticationHandler(IOptionsMonitor<MakeMyCapAuthenticationOptions> options, IUserProxy userProxy, ILoggerFactory logger, UrlEncoder encoder, 
		ISystemClock clock) : base(options, logger, encoder, clock)
	{
		this.userProxy = userProxy;
		this.logger = logger.CreateLogger<MakeMyCapAuthenticationHandler>();
	}

	protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
	{
		var bearer = Request.Headers[AUTHORIZATION_KEY].FirstOrDefault()?.Split(" ").Last();
		if (string.IsNullOrEmpty(bearer))
		{
			logger.LogDebug("Authorization token is not provided.");
			return AuthenticateResult.Fail("Authorization token is not provided");
		}
		
		var token = bearer.StartsWith(BEARER_HEADER) ? bearer.Substring(BEARER_HEADER.Length) : bearer;

		var userToken = userProxy.ValidateUserToken(token);
		if (userToken == null)
		{
			logger.LogInformation("Authorization token is not valid or is expired");
			return AuthenticateResult.Fail("Authorization token is not valid or is expired");
		}

		var user = userProxy.GetUserById(userToken.UserId);
		if (user == null)
		{
			logger.LogInformation("User record cannot be located - has it been deleted?");
			return AuthenticateResult.Fail("Authorization token is not valid or is expired");
		}
		
		logger.LogDebug($"Authenticated logged in user {user.Username}");

		var claims = new[]
		{
			new Claim(ClaimTypes.Name, user.Username)
		};
		var identity = new ClaimsIdentity(claims, Scheme.Name);
		var principal = new ClaimsPrincipal(identity);

		return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
	}
}


public class MakeMyCapAuthenticationOptions : AuthenticationSchemeOptions
{
	// Custom properties for the scheme
}