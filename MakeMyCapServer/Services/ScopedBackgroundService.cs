namespace ShopifyInventoryFulfillment.Services;

public class ScopedBackgroundService : BackgroundService
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<ScopedBackgroundService> logger;

	public ScopedBackgroundService(IServiceProvider serviceProvider, ILogger<ScopedBackgroundService> logger)
	{
		this.serviceProvider = serviceProvider;
		this.logger = logger;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(ScopedBackgroundService)} is running.");
		await DoWorkAsync(stoppingToken);
	}

	private async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(ScopedBackgroundService)} is working.");

		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IScopedProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IScopedProcessingService>();
			await scopedProcessingService.DoWorkAsync(stoppingToken);
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(ScopedBackgroundService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}	
}