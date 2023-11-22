using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.Services.OrderPlacement;

public class OrderPlacementScopedBackgroundService : BackgroundService
{
	private readonly IServiceProvider serviceProvider;
	private readonly ILogger<OrderPlacementScopedBackgroundService> logger;

	private ServiceLog serviceLog;

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
		logger.LogInformation($"{nameof(OrderPlacementScopedBackgroundService)} is working.");
		
		using (IServiceScope scope = serviceProvider.CreateScope())
		{
			IOrderPlacementProcessingService scopedProcessingService = scope.ServiceProvider.GetRequiredService<IOrderPlacementProcessingService>();
			await scopedProcessingService.DoWorkAsync(stoppingToken);
		}
	}

	public override async Task StopAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation($"{nameof(OrderPlacementScopedBackgroundService)} is stopping.");
		await base.StopAsync(stoppingToken);
	}	
}