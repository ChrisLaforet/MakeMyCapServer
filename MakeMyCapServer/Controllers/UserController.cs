using MakeMyCapServer.Controllers.Model;
using Microsoft.AspNetCore.Authorization;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using Microsoft.AspNetCore.Mvc;
using WellBalanced.Controllers.Model;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
//[EnableCors(origins: "http://localhost", headers: "*", methods: "*")]
public class UserController : ControllerBase
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<UserController> logger;

	public AuthenticateCommandHandler AuthenticateCommandHandler { get; set; }
	public LogoutCommandHandler LogoutCommandHandler { get; set; }

	public UserController(IServiceProvider? serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
		if (serviceProvider == null)
		{
			throw new ArgumentException("ServiceProvider is null");
		}

		this.AuthenticateCommandHandler = ActivatorUtilities.CreateInstance<AuthenticateCommandHandler>(serviceProvider);
		this.LogoutCommandHandler = ActivatorUtilities.CreateInstance<LogoutCommandHandler>(serviceProvider);
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public IActionResult Login(AuthenticateRequest model)
	{
		try
		{
			var response = AuthenticateCommandHandler.Handle(new AuthenticateCommand(model.Username, model.Password));
			var user = userProxy.GetUserById(response.UserId);
			return Ok(new JwtResponse(response.Token, response.UserName, response.Email));
		}
		catch (Exception ex)
		{
			logger.Log(LogLevel.Debug, "Error logging in user " + model.Username + ": " + ex);
		}

		return BadRequest(new { message = "Unable to authenticate" });
	}
    
	[HttpPut("logout")]
	[AllowAnonymous]
	public IActionResult Logout()
	{
		try
		{
			var possibleUser = this.Request.HttpContext.Items["User"];
			if (possibleUser != null)
			{
				User user = (User)possibleUser;
				LogoutCommandHandler.Handle(new LogoutCommand(user.Username));
			}
		}
		catch (Exception ex)
		{
			logger.LogDebug($"Exception caught while attempting to log out: {ex}");
		}

		return Ok(new {message = "Logged out"});
	}
}