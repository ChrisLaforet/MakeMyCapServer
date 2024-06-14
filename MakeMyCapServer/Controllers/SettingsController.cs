using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.QueryHandler;
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

}