using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Background;
using MakeMyCapServer.Services.Notification;

namespace MakeMyCapServer.Services.Fulfillment;

public class FulfillmentScopedBackgroundService : BackgroundService, IInterruptableService
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<FulfillmentScopedBackgroundService> logger;
	
	public FulfillmentScopedBackgroundService(IServiceProvider serviceProvider, ILogger<FulfillmentScopedBackgroundService> logger)
	{
		this.serviceProvider = serviceProvider;
		this.logger = logger;
	}

	public void ResumeProcessingNow()
	{
		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IFulfillmentProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IFulfillmentProcessingService>();
			scopedProcessingService.ResumeProcessingNow();
		}
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(FulfillmentScopedBackgroundService)} is running.");
		await DoWorkAsync(stoppingToken);
	}

	private async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		await Task.Yield();
		logger.LogInformation($"{nameof(FulfillmentScopedBackgroundService)} is working.");

		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IStatusNotificationService statusNotificationService = scope.ServiceProvider.GetRequiredService<IStatusNotificationService>();
			statusNotificationService.SendServiceStartupStatus(nameof(FulfillmentScopedBackgroundService));

			IFulfillmentProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IFulfillmentProcessingService>();
			await scopedProcessingService.DoWorkAsync(stoppingToken);
						
			statusNotificationService.SendServiceStartupStatus(nameof(FulfillmentScopedBackgroundService));
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(FulfillmentScopedBackgroundService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}	
}