using MakeMyCapServer.Model;
using MakeMyCapServer.Services.Notification;

namespace MakeMyCapServer.Services.OrderPlacement;

public class OrderPlacementScopedBackgroundService : BackgroundService
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<OrderPlacementScopedBackgroundService> logger;

	public OrderPlacementScopedBackgroundService(IServiceProvider serviceProvider, ILogger<OrderPlacementScopedBackgroundService> logger)
	{
		this.serviceProvider = serviceProvider;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(OrderPlacementScopedBackgroundService)} is running.");
		await DoWorkAsync(stoppingToken);
	}

	private async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		logger.LogInformation($"{nameof(OrderPlacementScopedBackgroundService)} is working.");
		
		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IStatusNotificationService statusNotificationService = scope.ServiceProvider.GetRequiredService<IStatusNotificationService>();
			statusNotificationService.SendServiceStartupStatus(nameof(OrderPlacementScopedBackgroundService));
			
			IOrderPlacementProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IOrderPlacementProcessingService>();
			await scopedProcessingService.DoWorkAsync(stoppingToken);

			statusNotificationService.SendServiceShutdownStatus(nameof(OrderPlacementScopedBackgroundService));
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(OrderPlacementScopedBackgroundService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}	
}