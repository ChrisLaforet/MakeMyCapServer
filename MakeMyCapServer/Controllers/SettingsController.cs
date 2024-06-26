﻿using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class SettingsController : ControllerBase
{
	private readonly SettingsQueryHandler settingsQueryHandler;
	private readonly NotificationsQueryHandler notificationsQueryHandler;

	private readonly ChangeSettingsCommandHandler changeSettingsCommandHandler;
	private readonly ChangeNotificationsCommandHandler changeNotificationsCommandHandler;

	private readonly ILogger<SettingsController> logger;

	public SettingsController(IServiceProvider serviceProvider, ILogger<SettingsController> logger)
	{
		this.logger = logger;
		
		settingsQueryHandler = ActivatorUtilities.CreateInstance<SettingsQueryHandler>(serviceProvider);
		notificationsQueryHandler = ActivatorUtilities.CreateInstance<NotificationsQueryHandler>(serviceProvider);

		changeSettingsCommandHandler = ActivatorUtilities.CreateInstance<ChangeSettingsCommandHandler>(serviceProvider);
		changeNotificationsCommandHandler = ActivatorUtilities.CreateInstance<ChangeNotificationsCommandHandler>(serviceProvider);
	}
	
	[Authorize]
	[HttpPost("save_notifications")]
	public IActionResult Notifications(NotificationEmails notificationEmails)
	{
		changeNotificationsCommandHandler.Handle(new ChangeNotificationsCommand(notificationEmails));
		return Ok(new { message = "Saved"});
	}

	[Authorize]
	[HttpGet("notifications")]
	public IActionResult Notifications()
	{
		var notificationResponse = notificationsQueryHandler.Handle(new NotificationsQuery());
		var notificationEmailValues = new NotificationEmails()
		{
			WarningEmail1 = notificationResponse.WarningEmail1,
			WarningEmail2 = notificationResponse.WarningEmail2,
			WarningEmail3 = notificationResponse.WarningEmail3,
			CriticalEmail1 = notificationResponse.CriticalEmail1,
			CriticalEmail2 = notificationResponse.CriticalEmail2,
			CriticalEmail3 = notificationResponse.CriticalEmail3
		};
		return Ok(new {notificationEmails = notificationEmailValues});
	}
	
	[Authorize]
	[HttpGet("settings")]
	public IActionResult GetSettings()
	{
		var settingResponse = settingsQueryHandler.Handle(new SettingsQuery());
		var settingValues = new Settings()
		{
			InventoryCheckHours = settingResponse.InventoryCheckHours,
			FulfillmentCheckHours = settingResponse.FulfillmentCheckHours,
			NextPoSequence = settingResponse.NextPoSequence
		};
		
		return Ok(new {settings = settingValues});
	}
	
	[Authorize]
	[HttpPost("save_settings")]
	public IActionResult Settings(Settings settings)
	{
		changeSettingsCommandHandler.Handle(new ChangeSettingsCommand(settings.InventoryCheckHours, settings.FulfillmentCheckHours, settings.NextPoSequence));
		return Ok(new { message = "Saved"});
	}
}