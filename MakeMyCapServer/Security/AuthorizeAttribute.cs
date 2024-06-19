using System.Security.Authentication;
using System.Security.Principal;
using MakeMyCapServer.Proxies;
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

		var token = bearer[0];
		if (token.StartsWith(BEARER_HEADER))
		{
			token = token.Substring(BEARER_HEADER.Length);
		}
		
		var userProxy = context.HttpContext.RequestServices.GetService<IUserProxy>();
		if (userProxy == null)
		{
			throw new AuthenticationException("Invalid or expired bearer token");
		}

		var user = userProxy.ValidateUserToken(token);
		if (user == null)
		{
			throw new AuthenticationException("Invalid or expired bearer token");
		}

		// var roles = jwt.GetRoles();
		// ValidateHasAuthorizedRole(jwt);

		var roles = new List<string>();		 // for expansion later?
		
		var identity = new GenericIdentity(user.UserId);
		var principal = new GenericPrincipal(identity, roles.ToArray());
		Thread.CurrentPrincipal = principal;
		context.HttpContext.User = principal;
		//HttpContext.Current.User = principal;
	}
}
