using System.Security.Claims;
using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Protocols.WsFed;

namespace MakeMyCapServer.Controllers;

public class LoginController : Controller
{
	public const string COOKIE_NAME = "MMCUser";
	public const string TOKEN_NAME = "MMCUserToken";

	private readonly AuthenticateCommandHandler AuthenticateCommandHandler;
	private readonly PasswordRecoveryCommandHandler PasswordRecoveryCommandHandler;
	private readonly ChangePasswordCommandHandler ChangePasswordCommandHandler;

	private readonly ILogger<LoginController> _logger;

	public LoginController(IServiceProvider? serviceProvider, ILogger<LoginController> logger)
	{
		_logger = logger;
		AuthenticateCommandHandler = ActivatorUtilities.CreateInstance<AuthenticateCommandHandler>(serviceProvider);
		PasswordRecoveryCommandHandler = ActivatorUtilities.CreateInstance<PasswordRecoveryCommandHandler>(serviceProvider);
		ChangePasswordCommandHandler = ActivatorUtilities.CreateInstance<ChangePasswordCommandHandler>(serviceProvider);
	}
	
	[AllowAnonymous]
	public IActionResult Index()
	{
		return View("Index", new Credential());
	}
	
	[AllowAnonymous]
	public async Task<IActionResult> LogOff()
	{
		await HttpContext.SignOutAsync(COOKIE_NAME);
		return View("Index", new Credential());
	}

	[HttpPost]
	[AllowAnonymous]
	public async Task<IActionResult> Authenticate(Credential credential)
	{
		if (!ModelState.IsValid)
		{
			return View("Index", credential);
		}
		
		try
		{
			await HttpContext.SignOutAsync(COOKIE_NAME);

			var user = AuthenticateCommandHandler.Handle(new AuthenticateCommand(credential.UserName, credential.Password));
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.UserName),
				new Claim(ClaimTypes.Email, user.Email),
				new Claim(TOKEN_NAME, user.Token)
			};
			var identity = new ClaimsIdentity(claims, COOKIE_NAME);
			ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

			await HttpContext.SignInAsync(COOKIE_NAME, claimsPrincipal);
			return RedirectToAction("Index", "Home");
		}
		catch (Exception ex)
		{
			return View("Index", credential);
		}
	}

	[AllowAnonymous]
	public IActionResult ForgotPassword()
	{
		return View("ForgotPassword", new UserIdentity());
	}

	[AllowAnonymous]
	[HttpPost]
	public IActionResult RecoverPassword(UserIdentity user)
	{
		if (!ModelState.IsValid)
		{
			return View("ForgotPassword", user);
		}
		
		try
		{
			PasswordRecoveryCommandHandler.Handle(new PasswordRecoveryCommand(user.Email));
			return RedirectToAction("ChangePassword", "Login");
		}
		catch (Exception ex)
		{
			return RedirectToAction("Index", "Login");
		}
	}
	
	[AllowAnonymous]
	public IActionResult ChangePassword()
	{
		return View("ChangePassword", new PasswordChange());
	}

	[AllowAnonymous]
	[HttpPost]
	public IActionResult CompletePasswordChange(PasswordChange passwordChange)
	{
		if (!ModelState.IsValid)
		{
			return View("ChangePassword", passwordChange);
		}

		try
		{
			ChangePasswordCommandHandler.Handle(new ChangePasswordCommand(passwordChange.UserName, passwordChange.ResetKey, passwordChange.NewPassword));
			return RedirectToAction("Index", "Login");
		}
		catch (Exception e)
		{
			ModelState.AddModelError("Failed", "Password change has failed - are all fields correct?");
			return View("ChangePassword", passwordChange);
		}
	}
}