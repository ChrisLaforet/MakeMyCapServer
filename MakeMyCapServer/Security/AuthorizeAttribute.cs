using System.Security.Authentication;
using System.Security.Principal;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Security.JWT;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

namespace MakeMyCapServer.Security;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeAttribute : Attribute, IAuthorizationFilter
{
	public const string AUTHORIZATION_KEY = "authorization";
	public const string BEARER_HEADER = "Bearer ";

	public string? Roles { get; set; }
	
	public void OnAuthorization(AuthorizationFilterContext context)
	{
		// TODO: CML- finish authorization support?? - now roles are in, is there anything else needed here?

		// Checks the bearer JWT token
		var bearer = context.HttpContext.Request.Headers[AUTHORIZATION_KEY];
		if (bearer.IsNullOrEmpty())
		{
			throw new AuthenticationException("No authorization token provided");
		}

		var jwtContent = bearer[0];
		if (jwtContent.StartsWith(BEARER_HEADER))
		{
			jwtContent = jwtContent.Substring(BEARER_HEADER.Length);
		}

		var jwt = JsonWebToken.From(jwtContent);
		var userProxy = context.HttpContext.RequestServices.GetService<IUserProxy>();
		if (userProxy == null || !userProxy.ValidateUserToken(jwt))
		{
			throw new AuthenticationException("Invalid or expired bearer token");
		}

		var roles = jwt.GetRoles();
		ValidateHasAuthorizedRole(jwt);
		
		var identity = new GenericIdentity(jwt.User);
		var principal = new GenericPrincipal(identity, roles);
		Thread.CurrentPrincipal = principal;
		context.HttpContext.User = principal;
		//HttpContext.Current.User = principal;
	}

	private void ValidateHasAuthorizedRole(JsonWebToken jwt)
	{
		if (Roles?.Length > 0)
		{
			var expectedRoles = Roles.Split(",", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
			bool isMissingRole = true;
			foreach (var expectedRole in expectedRoles)
			{
				if (jwt.GetRoles().Contains(expectedRole))
				{
					isMissingRole = false;
					break;
				}
			}

			if (isMissingRole)
			{
				throw new UnauthorizedAccessException($"User {jwt.User} does not have a role in {Roles}");
			}
		}
	}
}
