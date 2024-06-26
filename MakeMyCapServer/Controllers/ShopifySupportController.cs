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
public class ShopifySupportController : ControllerBase
{
	private readonly DistributorsQueryHandler distributorsQueryHandler;
	private readonly DistributorSkusQueryHandler distributorSkusQueryHandler;
	private readonly SkuQueryHandler skuQueryHandler;
	private readonly CreateSkuCommandHandler createSkuCommandHandler;

	private readonly ILogger<ShopifySupportController> logger;

	public ShopifySupportController(IServiceProvider serviceProvider, ILogger<ShopifySupportController> logger)
	{
		this.logger = logger;

		distributorsQueryHandler = ActivatorUtilities.CreateInstance<DistributorsQueryHandler>(serviceProvider);
		distributorSkusQueryHandler = ActivatorUtilities.CreateInstance<DistributorSkusQueryHandler>(serviceProvider);
		skuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
		createSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
	}
	
	[Authorize]
	[HttpGet("distributors")]
	public IActionResult Distributors()
	{
		var distributors = distributorsQueryHandler.Handle(new DistributorsQuery());
		return Ok(new {distributors = distributors});
	}

	[Authorize]
	[HttpGet("skus")]
	public IActionResult Skus(string id)
	{
		var response = distributorSkusQueryHandler.Handle(new DistributorSkusQuery(id));
		return Ok(response);
	}
	
	
	//
	// [Authorize]
	// public IActionResult DistributorSkus(string id)
	// {
	// 	return PartialView("DistributorSkus", distributorSkusQueryHandler.Handle(new DistributorSkusQuery(id)));
	// }
	//
	// [Authorize]
	// public IActionResult AddSku()
	// {
	// 	var createSku = new CreateSku();
	// 	createSku.Distributors = distributorsQueryHandler.Handle(new DistributorsQuery());
	// 	return View("AddSku", createSku);
	// }
	
	// [Authorize]
	// [HttpPost]
	// public IActionResult AddSku(CreateSku model)
	// {
	// 	model.Distributors = distributorsQueryHandler.Handle(new DistributorsQuery());		 // prepopulate in case we send model back out
	//
	// 	if (!ModelState.IsValid)
	// 	{
	// 		return View("AddSku", model);
	// 	}
	//
	// 	var skuResponse = skuQueryHandler.Handle(new SkuQuery(model.Sku.Trim()));
	// 	if (skuResponse != null)
	// 	{
	// 		ModelState.AddModelError("Failed", $"There is already an assigned record with SKU {model.Sku} for {skuResponse.DistributorCode}");
	// 		return View("AddSku", model);
	// 	}
	//
	// 	try
	// 	{
	// 		createSkuCommandHandler.Handle(new CreateSkuCommand(model));
	// 		return RedirectToAction("AddSku", "Home");
	// 	}
	// 	catch (Exception)
	// 	{
	// 		ModelState.AddModelError("Failed", $"There was an error while creating record with SKU {model.Sku}");
	// 		return View("AddSku", model);
	// 	}
	// }
}