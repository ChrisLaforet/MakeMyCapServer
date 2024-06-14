using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
	private readonly UserQueryHandler UserQueryHandler;
	private readonly CreateUserCommandHandler CreateUserCommandHandler;
	private readonly ILogger<UsersController> logger;

	public UsersController(IServiceProvider serviceProvider, ILogger<UsersController> logger)
	{
		this.logger = logger;
		
		UserQueryHandler = ActivatorUtilities.CreateInstance<UserQueryHandler>(serviceProvider);
		CreateUserCommandHandler = ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(serviceProvider);
	}

}