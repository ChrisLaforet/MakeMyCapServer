using MakeMyCapAdmin.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MakeMyCapAdmin.Controllers.Model;
using MakeMyCapAdmin.CQS.Command;
using MakeMyCapAdmin.CQS.CommandHandler;
using MakeMyCapAdmin.CQS.Query;
using MakeMyCapAdmin.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;

namespace MakeMyCapAdmin.Controllers;

public class HomeController : Controller
{
	private readonly ServiceStatusQueryHandler ServiceStatusQueryHandler;
	private readonly SettingsQueryHandler SettingsQueryHandler;
	private readonly DistributorsQueryHandler DistributorsQueryHandler;
	private readonly DistributorSkusQueryHandler DistributorSkusQueryHandler;
	private readonly SkuQueryHandler SkuQueryHandler;
	private readonly NotificationsQueryHandler NotificationsQueryHandler;
	private readonly UserQueryHandler UserQueryHandler;

	private readonly ChangeSettingsCommandHandler ChangeSettingsCommandHandler;
	private readonly CreateSkuCommandHandler CreateSkuCommandHandler;
	private readonly ChangeNotificationsCommandHandler ChangeNotificationsCommandHandler;
	private readonly CreateUserCommandHandler CreateUserCommandHandler;

	private readonly ILogger<HomeController> _logger;

	public HomeController(IServiceProvider serviceProvider, ILogger<HomeController> logger)
	{
		_logger = logger;
		
		ServiceStatusQueryHandler = ActivatorUtilities.CreateInstance<ServiceStatusQueryHandler>(serviceProvider);
		SettingsQueryHandler = ActivatorUtilities.CreateInstance<SettingsQueryHandler>(serviceProvider);
		DistributorSkusQueryHandler = ActivatorUtilities.CreateInstance<DistributorSkusQueryHandler>(serviceProvider);
		DistributorsQueryHandler = ActivatorUtilities.CreateInstance<DistributorsQueryHandler>(serviceProvider);
		SkuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
		NotificationsQueryHandler = ActivatorUtilities.CreateInstance<NotificationsQueryHandler>(serviceProvider);
		UserQueryHandler = ActivatorUtilities.CreateInstance<UserQueryHandler>(serviceProvider);

		ChangeSettingsCommandHandler = ActivatorUtilities.CreateInstance<ChangeSettingsCommandHandler>(serviceProvider);
		CreateSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
		ChangeNotificationsCommandHandler = ActivatorUtilities.CreateInstance<ChangeNotificationsCommandHandler>(serviceProvider);
		CreateUserCommandHandler = ActivatorUtilities.CreateInstance<CreateUserCommandHandler>(serviceProvider);
	}

	[Authorize]
	public IActionResult Index()
	{
		var serviceStatus = new ServiceStatus();
		serviceStatus.EmailServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery("EmailQueueProcessingService"));
		serviceStatus.FulfillmentServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery("FulfillmentUpdateService"));
		serviceStatus.InventoryServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery("InventoryUpdateService"));
		serviceStatus.OrderPlacementServiceStatus = ServiceStatusQueryHandler.Handle(new ServiceStatusQuery("OrderPlacementQueueService"));

		return View("Index", serviceStatus);
	}
	
	[Authorize]
	public IActionResult Notifications()
	{
		var notificationResponse = NotificationsQueryHandler.Handle(new NotificationsQuery());
		var notificationEmails = new NotificationEmails()
		{
			WarningEmail1 = notificationResponse.WarningEmail1,
			WarningEmail2 = notificationResponse.WarningEmail2,
			WarningEmail3 = notificationResponse.WarningEmail3,
			CriticalEmail1 = notificationResponse.CriticalEmail1,
			CriticalEmail2 = notificationResponse.CriticalEmail2,
			CriticalEmail3 = notificationResponse.CriticalEmail3
		};
		return View(notificationEmails);
	}

	[Authorize]
	[HttpPost]
	public IActionResult Notifications(NotificationEmails notificationEmails)
	{
		if (!ModelState.IsValid)
		{
			return View("Notifications", notificationEmails);
		}
		
		ChangeNotificationsCommandHandler.Handle(new ChangeNotificationsCommand(notificationEmails));
		return RedirectToAction("Notifications", "Home");
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
	public IActionResult AddUser()
	{
		return View(new CreateUser());
	}

	[Authorize]
	[HttpPost]
	public IActionResult AddUser(CreateUser model)
	{
		if (!ModelState.IsValid)
		{
			return View("AddUser", model);
		}

		var userResponse = UserQueryHandler.Handle(new UserQuery(model.UserName, model.Email));
		if (userResponse.EmailExists || userResponse.UserNameExists)
		{
			if (userResponse.EmailExists)
			{
				ModelState.AddModelError("Failed", $"There is already a user with an Email of {model.Email}");
			}

			if (userResponse.UserNameExists)
			{
				ModelState.AddModelError("Failed", $"There is already a user with a username of {model.UserName}");
			}

			return View("AddUser", model);
		}
		
		try
		{
			CreateUserCommandHandler.Handle(new CreateUserCommand(model.UserName, model.Email));
			return RedirectToAction("Index", "Home");
		}
		catch (Exception)
		{
			ModelState.AddModelError("Failed", $"There was an error while creating user record");
			return View("AddUser", model);
		}
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