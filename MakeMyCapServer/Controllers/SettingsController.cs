using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
	private readonly SettingsQueryHandler SettingsQueryHandler;
	private readonly DistributorsQueryHandler DistributorsQueryHandler;
	private readonly DistributorSkusQueryHandler DistributorSkusQueryHandler;
	private readonly SkuQueryHandler SkuQueryHandler;
	private readonly NotificationsQueryHandler NotificationsQueryHandler;

	private readonly ChangeSettingsCommandHandler ChangeSettingsCommandHandler;
	private readonly CreateSkuCommandHandler CreateSkuCommandHandler;
	private readonly ChangeNotificationsCommandHandler ChangeNotificationsCommandHandler;
	private readonly ILogger<SettingsController> logger;

	public SettingsController(IServiceProvider serviceProvider, ILogger<SettingsController> logger)
	{
		this.logger = logger;
		
		SettingsQueryHandler = ActivatorUtilities.CreateInstance<SettingsQueryHandler>(serviceProvider);
		DistributorSkusQueryHandler = ActivatorUtilities.CreateInstance<DistributorSkusQueryHandler>(serviceProvider);
		DistributorsQueryHandler = ActivatorUtilities.CreateInstance<DistributorsQueryHandler>(serviceProvider);
		SkuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
		NotificationsQueryHandler = ActivatorUtilities.CreateInstance<NotificationsQueryHandler>(serviceProvider);

		ChangeSettingsCommandHandler = ActivatorUtilities.CreateInstance<ChangeSettingsCommandHandler>(serviceProvider);
		CreateSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
		ChangeNotificationsCommandHandler = ActivatorUtilities.CreateInstance<ChangeNotificationsCommandHandler>(serviceProvider);
	}
	
	// [Authorize]
	// [HttpPost]
	// public IActionResult Notifications(NotificationEmails notificationEmails)
	// {
	// 	if (!ModelState.IsValid)
	// 	{
	// 		return View("Notifications", notificationEmails);
	// 	}
	// 	
	// 	ChangeNotificationsCommandHandler.Handle(new ChangeNotificationsCommand(notificationEmails));
	// 	return RedirectToAction("Notifications", "Home");
	// }

	// [Authorize]
	// public IActionResult Settings()
	// {
	// 	var settingResponse = SettingsQueryHandler.Handle(new SettingsQuery());
	// 	var settings = new Settings()
	// 	{
	// 		InventoryCheckHours = settingResponse.InventoryCheckHours,
	// 		FulfillmentCheckHours = settingResponse.FulfillmentCheckHours,
	// 		NextPoSequence = settingResponse.NextPoSequence
	// 	};
	// 	
	// 	return View("Settings", settings);
	// }
	//
	// [Authorize]
	// [HttpPost]
	// public IActionResult Settings(Settings settings)
	// {
	// 	if (!ModelState.IsValid)
	// 	{
	// 		return View("Settings", settings);
	// 	}
	//
	// 	ChangeSettingsCommandHandler.Handle(new ChangeSettingsCommand(settings.InventoryCheckHours, settings.FulfillmentCheckHours, settings.NextPoSequence));
	// 	return RedirectToAction("Settings", "Home");
	// }
}