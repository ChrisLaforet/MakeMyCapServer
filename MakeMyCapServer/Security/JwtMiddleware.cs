using MakeMyCapServer.Security.JWT;

namespace MakeMyCapServer.Security;

public class JwtMiddleware
{
    private readonly RequestDelegate nextDelegate;

    public JwtMiddleware(RequestDelegate nextDelegate, IConfigurationLoader configurationLoader)
    {
        this.nextDelegate = nextDelegate;
    }

    public async Task Invoke(HttpContext context, IUserProxy userProxy)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token != null)
        {
            attachUserToContext(context, userProxy, token);
        }

        await nextDelegate(context);
    }

    private void attachUserToContext(HttpContext context, IUserProxy userProxy, string token)
    {
        try
        {
            JsonWebToken jwt = JsonWebToken.From(token);
            jwt.AssertTokenIsExpired();

            if (userProxy.ValidateUserToken(jwt))
            {
                context.Items["User"] = userProxy.GetUserByGuid(jwt.UserId);
            }
        }
        catch
        {
            // do nothing if jwt validation fails
            // user is not attached to context so request won't have access to secure routes
        }
    }
}
