using MakeMyCapServer.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Fulfillment;
using MakeMyCapServer.Services.Inventory;
using MakeMyCapServer.Services.OrderPlacement;
using Microsoft.AspNetCore.Authorization;

namespace MakeMyCapServer.Controllers;

public class HomeController : Controller
{
	private readonly ServiceStatusQueryHandler ServiceStatusQueryHandler;
	private readonly SettingsQueryHandler SettingsQueryHandler;
	private readonly DistributorsQueryHandler DistributorsQueryHandler;
	private readonly DistributorSkusQueryHandler DistributorSkusQueryHandler;
	private readonly SkuQueryHandler SkuQueryHandler;
	
	private readonly ChangeSettingsCommandHandler ChangeSettingsCommandHandler;
	private readonly CreateSkuCommandHandler CreateSkuCommandHandler;

	private readonly ILogger<HomeController> _logger;

	public HomeController(IServiceProvider? serviceProvider, ILogger<HomeController> logger)
	{
		_logger = logger;
		
		ServiceStatusQueryHandler = ActivatorUtilities.CreateInstance<ServiceStatusQueryHandler>(serviceProvider);
		SettingsQueryHandler = ActivatorUtilities.CreateInstance<SettingsQueryHandler>(serviceProvider);
		DistributorSkusQueryHandler = ActivatorUtilities.CreateInstance<DistributorSkusQueryHandler>(serviceProvider);
		DistributorsQueryHandler = ActivatorUtilities.CreateInstance<DistributorsQueryHandler>(serviceProvider);
		SkuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);

		ChangeSettingsCommandHandler = ActivatorUtilities.CreateInstance<ChangeSettingsCommandHandler>(serviceProvider);
		CreateSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
	}

	[Authorize]
	public IActionResult Index()
	{
		var serviceStatus = new ServiceStatus();
		serviceStatus.EmailServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery(nameof(EmailQueueProcessingService)));
		serviceStatus.FulfillmentServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery(nameof(FulfillmentUpdateService)));
		serviceStatus.InventoryServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery(nameof(InventoryUpdateService)));
		serviceStatus.OrderPlacementServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery(nameof(OrderPlacementQueueService)));

		return View("Index", serviceStatus);
	}
	
	[Authorize]
	public IActionResult Notifications()
	{
		return View();
	}
	
	[Authorize]
	public IActionResult Settings()
	{
		var settingResponse = SettingsQueryHandler.Handle(new SettingsQuery());
		var settings = new Settings()
		{
			InventoryCheckHours = settingResponse.InventoryCheckHours,
			FulfillmentCheckHours = settingResponse.FulfillmentCheckHours,
			NextPoSequence = settingResponse.NextPoSequence
		};
		
		return View("Settings", settings);
	}
	
	[Authorize]
	[HttpPost]
	public IActionResult Settings(Settings settings)
	{
		if (!ModelState.IsValid)
		{
			return View("Settings", settings);
		}

		ChangeSettingsCommandHandler.Handle(new ChangeSettingsCommand(settings.InventoryCheckHours, settings.FulfillmentCheckHours, settings.NextPoSequence));
		return RedirectToAction("Settings", "Home");
	}
	
	[Authorize]
	public IActionResult Users()
	{
		return View();
	}

	[Authorize]
	public IActionResult Skus()
	{
		return View("skus", DistributorsQueryHandler.Handle(new DistributorsQuery()));
	}

	[Authorize]
	public IActionResult DistributorSkus(string id)
	{
		return PartialView("DistributorSkus", DistributorSkusQueryHandler.Handle(new DistributorSkusQuery(id)));
	}
	
	[Authorize]
	public IActionResult AddSku()
	{
		var createSku = new CreateSku();
		createSku.Distributors = DistributorsQueryHandler.Handle(new DistributorsQuery());
		return View("AddSku", createSku);
	}
	
	[Authorize]
	[HttpPost]
	public IActionResult AddSku(CreateSku model)
	{
		model.Distributors = DistributorsQueryHandler.Handle(new DistributorsQuery());		 // prepopulate in case we send model back out

		if (!ModelState.IsValid)
		{
			return View("AddSku", model);
		}

		var skuResponse = SkuQueryHandler.Handle(new SkuQuery(model.Sku.Trim()));
		if (skuResponse != null)
		{
			ModelState.AddModelError("Failed", $"There is already an assigned record with SKU {model.Sku} for {skuResponse.DistributorCode}");
			return View("AddSku", model);
		}

		try
		{
			CreateSkuCommandHandler.Handle(new CreateSkuCommand(model));
			return RedirectToAction("AddSku", "Home");
		}
		catch (Exception)
		{
			ModelState.AddModelError("Failed", $"There was an error while creating record with SKU {model.Sku}");
			return View("AddSku", model);
		}
	}
	
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public IActionResult Error()
	{
		return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
	}
}