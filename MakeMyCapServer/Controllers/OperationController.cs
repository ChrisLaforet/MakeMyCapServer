using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class OperationController : ControllerBase
{
	private readonly ServiceStatusQueryHandler serviceStatusQueryHandler;
	private readonly SkuQueryHandler skuQueryHandler;
	private readonly CreateSkuCommandHandler createSkuCommandHandler;

	private readonly ILogger<OperationController> logger;

	public OperationController(IServiceProvider serviceProvider, ILogger<OperationController> logger)
	{
		this.logger = logger;

		serviceStatusQueryHandler = ActivatorUtilities.CreateInstance<ServiceStatusQueryHandler>(serviceProvider);
		skuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
		createSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
	}

	[Authorize]
	[HttpGet("get_service_status")]
	public IActionResult GetServiceStatus(string service)
	{
		if (string.Equals(service, "Email", StringComparison.OrdinalIgnoreCase) || 
		    string.Equals(service, "EmailQueue", StringComparison.OrdinalIgnoreCase))
		{
			return Ok(new { values = serviceStatusQueryHandler.Handle(new ServiceStatusQuery("EmailQueueProcessingService"))});
		}
		if (string.Equals(service, "Fulfillment", StringComparison.OrdinalIgnoreCase) || 
		    string.Equals(service, "FulfillmentUpdate", StringComparison.OrdinalIgnoreCase))
		{
			return Ok(new { values = serviceStatusQueryHandler.Handle(new ServiceStatusQuery("FulfillmentUpdateService"))});
		}
		if (string.Equals(service, "Inventory", StringComparison.OrdinalIgnoreCase) || 
		    string.Equals(service, "InventoryUpdate", StringComparison.OrdinalIgnoreCase))
		{
			return Ok(new { values = serviceStatusQueryHandler.Handle(new ServiceStatusQuery("InventoryUpdateService"))});
		}
		if (string.Equals(service, "Order", StringComparison.OrdinalIgnoreCase) || 
		    string.Equals(service, "Orders", StringComparison.OrdinalIgnoreCase) || 
		    string.Equals(service, "OrderPlacement", StringComparison.OrdinalIgnoreCase))
		{
			return Ok(new { values = serviceStatusQueryHandler.Handle(new ServiceStatusQuery("OrderPlacementQueueService"))});
		}

		return BadRequest(new {message = "Invalid service type provided"});
	}

	// [Authorize]
	// public IActionResult Skus()
	// {
	// 	return View("skus", DistributorsQueryHandler.Handle(new DistributorsQuery()));
	// }
	//
	// [Authorize]
	// public IActionResult DistributorSkus(string id)
	// {
	// 	return PartialView("DistributorSkus", DistributorSkusQueryHandler.Handle(new DistributorSkusQuery(id)));
	// }
	//
	// [Authorize]
	// public IActionResult AddSku()
	// {
	// 	var createSku = new CreateSku();
	// 	createSku.Distributors = DistributorsQueryHandler.Handle(new DistributorsQuery());
	// 	return View("AddSku", createSku);
	// }
	//
	// [Authorize]
	// [HttpPost]
	// public IActionResult AddSku(CreateSku model)
	// {
	// 	model.Distributors = DistributorsQueryHandler.Handle(new DistributorsQuery());		 // prepopulate in case we send model back out
	//
	// 	if (!ModelState.IsValid)
	// 	{
	// 		return View("AddSku", model);
	// 	}
	//
	// 	var skuResponse = SkuQueryHandler.Handle(new SkuQuery(model.Sku.Trim()));
	// 	if (skuResponse != null)
	// 	{
	// 		ModelState.AddModelError("Failed", $"There is already an assigned record with SKU {model.Sku} for {skuResponse.DistributorCode}");
	// 		return View("AddSku", model);
	// 	}
	//
	// 	try
	// 	{
	// 		CreateSkuCommandHandler.Handle(new CreateSkuCommand(model));
	// 		return RedirectToAction("AddSku", "Home");
	// 	}
	// 	catch (Exception)
	// 	{
	// 		ModelState.AddModelError("Failed", $"There was an error while creating record with SKU {model.Sku}");
	// 		return View("AddSku", model);
	// 	}
	// }
}