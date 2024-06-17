using MakeMyCapServer.Controllers.Model;
using Microsoft.AspNetCore.Authorization;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Exception;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
//[EnableCors("AllowSpecificOrigins")]
public class UserController : ControllerBase
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<UserController> logger;

	private readonly AuthenticateCommandHandler authenticateCommandHandler;
	private readonly LogoutCommandHandler logoutCommandHandler;
	private readonly TokenValidationCommandHandler tokenValidationCommandHandler;
	private readonly PasswordRecoveryCommandHandler passwordRecoveryCommandHandler;
	private readonly ChangePasswordCommandHandler changePasswordCommandHandler;

	public UserController(IServiceProvider? serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
		if (serviceProvider == null)
		{
			throw new ArgumentException("ServiceProvider is null");
		}

		this.authenticateCommandHandler = ActivatorUtilities.CreateInstance<AuthenticateCommandHandler>(serviceProvider);
		this.logoutCommandHandler = ActivatorUtilities.CreateInstance<LogoutCommandHandler>(serviceProvider);
		this.tokenValidationCommandHandler = ActivatorUtilities.CreateInstance<TokenValidationCommandHandler>(serviceProvider);
		this.passwordRecoveryCommandHandler = ActivatorUtilities.CreateInstance<PasswordRecoveryCommandHandler>(serviceProvider);
		this.changePasswordCommandHandler = ActivatorUtilities.CreateInstance<ChangePasswordCommandHandler>(serviceProvider);
	}

	[HttpPost("login")]
	[AllowAnonymous]
	public IActionResult Login(AuthenticateRequest model)
	{
		try
		{
			var response = authenticateCommandHandler.Handle(new AuthenticateCommand(model.Username, model.Password));
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
				logoutCommandHandler.Handle(new LogoutCommand(user.Username));
			}
		}
		catch (Exception ex)
		{
			logger.LogDebug($"Exception caught while attempting to log out: {ex}");
		}

		return Ok(new {message = "Logged out"});
	}

	[HttpGet("validate-token")]
	public IActionResult ValidateToken()
	{
		try
		{
			var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
			tokenValidationCommandHandler.Handle(new TokenValidationCommand(token));
			return Ok(new {message = "Token valid"});
		}
		catch (InvalidTokenException ite)
		{
			logger.LogDebug("Invalid/expired token provided for validation");
		}
		catch (Exception ex)
		{
			logger.LogDebug($"Exception caught while attempting to validate token: {ex}");
		}
		return BadRequest(new { message = "Invalid token" });
	}

	[AllowAnonymous]
	[HttpPost("request_password_reset")]
	public IActionResult RequestPasswordReset(UserIdentity user)
	{
	
		try
		{
			passwordRecoveryCommandHandler.Handle(new PasswordRecoveryCommand(user.Identity));
			return Ok(new {message = "Password change reset requested"});
		}
		catch (Exception ex)
		{
			logger.LogDebug($"Exception caught while attempting to execute password reset: {ex}");
		}
		return BadRequest(new { message = "Request failed" });
	}

	[AllowAnonymous]
	[HttpPost("execute_password_reset")]
	public IActionResult ExecutePasswordReset(PasswordChange passwordChange)
	{
		try
		{
			changePasswordCommandHandler.Handle(new ChangePasswordCommand(passwordChange.UserName, passwordChange.ConfirmationCode, passwordChange.Password));
			return Ok(new {message = "Successful change"});
		}
		catch (Exception e)
		{
			ModelState.AddModelError("Failed", "Password change has failed - are all fields correct?");
		}
		return BadRequest(new { message = "Request failed" });
	}
}