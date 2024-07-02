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
	private readonly CreateInventoryCommandHandler createInventoryCommandHandler;
	// private readonly UpdateInventoryCommandHandler updateInventoryCommandHandler;
	//
	private readonly InventoryQueryHandler inventoryQueryHandler;
	private readonly AvailableSkusQueryHandler availableSkusQueryHandler;
	private readonly SkuQueryHandler skuQueryHandler;
	
	private readonly ILogger<InventoryController> logger;
	
	public InventoryController(IServiceProvider serviceProvider, ILogger<InventoryController> logger)
	{
		this.logger = logger;
	
		createInventoryCommandHandler = ActivatorUtilities.CreateInstance<CreateInventoryCommandHandler>(serviceProvider);
	// 	updateInventoryCommandHandler = ActivatorUtilities.CreateInstance<UpdateInventoryCommandHandler>(serviceProvider);
	//
		inventoryQueryHandler = ActivatorUtilities.CreateInstance<InventoryQueryHandler>(serviceProvider);
		availableSkusQueryHandler = ActivatorUtilities.CreateInstance<AvailableSkusQueryHandler>(serviceProvider);
		skuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
	}
	
	[Authorize]
	[HttpGet("in-house-inventory")]
	public IActionResult GetInHouseInventory()
	{
		var inventory = inventoryQueryHandler.Handle(new InventoryQuery());
		return Ok(new { items = inventory });
	}
	
	[Authorize]
	[HttpGet("available-skus")]
	public IActionResult GetAvailableSkus()
	{
		var skus = availableSkusQueryHandler.Handle(new AvailableSkusQuery());
		return Ok(new { skus = skus });
	}
	
	[Authorize]
	[HttpPost("create-in-house-inventory")]
	public IActionResult CreateInHouseInventory(CreateInventory model)
	{
		try
		{
			createInventoryCommandHandler.Handle(new CreateInventoryCommand(model));
			return Ok();
		}
		catch (Exception)
		{
			return BadRequest( $"There was an error while creating inventory for SKU {model.Sku}");
		}
	}
}