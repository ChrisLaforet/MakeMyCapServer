using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class InventoryController : ControllerBase
{
	// private readonly CreateInventoryCommandHandler createInventoryCommandHandler;
	// private readonly UpdateInventoryCommandHandler updateInventoryCommandHandler;
	//
	// private readonly InventoryQueryHandler inventoryQueryHandler;
	// private readonly SkusQueryHandler skusQueryHandler;
	// private readonly SkuQueryHandler skuQueryHandler;
	//
	// private readonly ILogger<InventoryController> logger;
	//
	// public InventoryController(IServiceProvider serviceProvider, ILogger<InventoryController> logger)
	// {
	// 	this.logger = logger;
	//
	// 	createInventoryCommandHandler = ActivatorUtilities.CreateInstance<CreateInventoryCommandHandler>(serviceProvider);
	// 	updateInventoryCommandHandler = ActivatorUtilities.CreateInstance<UpdateInventoryCommandHandler>(serviceProvider);
	//
	// 	inventoryQueryHandler = ActivatorUtilities.CreateInstance<InventoryQueryHandler>(serviceProvider);
	// 	skusQueryHandler = ActivatorUtilities.CreateInstance<SkusQueryHandler>(serviceProvider);
	// 	skuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
	// }
	//
	// [Authorize]
	// [HttpGet("inventory")]
	// public IActionResult Distributors()
	// {
	// 	var distributors = distributorsQueryHandler.Handle(new DistributorsQuery());
	// 	return Ok(new {distributors = distributors});
	// }
}