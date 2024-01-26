using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Notification;

namespace MakeMyCapServer.Services.Inventory;

public class InventoryScopedBackgroundService : BackgroundService
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<InventoryScopedBackgroundService> logger;
	
	public InventoryScopedBackgroundService(IServiceProvider serviceProvider, ILogger<InventoryScopedBackgroundService> logger)
	{
		this.serviceProvider = serviceProvider;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(InventoryScopedBackgroundService)} is running.");
		await DoWorkAsync(stoppingToken);
	}

	private async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		logger.LogInformation($"{nameof(InventoryScopedBackgroundService)} is working.");

		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IStatusNotificationService statusNotificationService = scope.ServiceProvider.GetRequiredService<IStatusNotificationService>();
			statusNotificationService.SendServiceStartupStatus(nameof(InventoryScopedBackgroundService));

			IInventoryProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IInventoryProcessingService>();
			await scopedProcessingService.DoWorkAsync(stoppingToken);

			statusNotificationService.SendServiceShutdownStatus(nameof(InventoryScopedBackgroundService));
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(InventoryScopedBackgroundService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}	
}