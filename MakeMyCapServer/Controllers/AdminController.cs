using MakeMyCapServer.Controllers.Model;
using Microsoft.AspNetCore.Authorization;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.Proxies;
using Microsoft.AspNetCore.Mvc;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using MakeMyCapServer.CQS.Response;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class AdminController : ControllerBase
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<UserController> logger;

	private readonly CreateUserCommandHandler createUserCommandHandler;
	private readonly UserQueryHandler userQueryHandler;
	private readonly UsersQueryHandler usersQueryHandler;

	public AdminController(IServiceProvider? serviceProvider, IUserProxy userProxy, ILogger<UserController> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
		if (serviceProvider == null)
		{
			throw new ArgumentException("ServiceProvider is null");
		}

		this.createUserCommandHandler = ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(serviceProvider);
		this.usersQueryHandler = ActivatorUtilities.CreateInstance<UsersQueryHandler>(serviceProvider);
		this.userQueryHandler = ActivatorUtilities.CreateInstance<UserQueryHandler>(serviceProvider);
	}
	
	[Authorize]
	[HttpGet("users")]
	public IActionResult Users(string id)
	{
		var users = usersQueryHandler.Handle(new UsersQuery());
		return Ok(new {users = users});
	}
	
	[Authorize]
	[HttpPost("create-user")]
	public IActionResult CreateUser(CreateUser model)
	{
		var userResponse = userQueryHandler.Handle(new UserQuery(model.UserName, model.Email));
		if (userResponse.EmailExists)
		{
			return BadRequest($"There is already a user with an Email of {model.Email}");
		}

		if (userResponse.UserNameExists)
		{
			return BadRequest($"There is already a user with a username of {model.UserName}");
		}
		
		try
		{
			createUserCommandHandler.Handle(new CreateUserCommand(model.UserName, model.Email));
			var userRecord = userProxy.GetUserByUsername(model.UserName);
			if (userRecord != null)
			{
				return Ok(new UserDataResponse(userRecord.Username, userRecord.Email, userRecord.CreateDate));
			}

			return Ok(new UserDataResponse(model.UserName, model.Email, DateTime.Now));
		}
		catch (Exception)
		{
			return BadRequest($"There was an error while creating user record");
		}
	}
}