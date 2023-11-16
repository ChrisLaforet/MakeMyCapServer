using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Services.OrderPlacement;

public class OrderPlacementQueueService : IOrderPlacementProcessingService
{
	private const int FIVE_MINUTES = 5;
	private const int PROCESSING_TIMEOUT_MSEC = FIVE_MINUTES * 60 * 1000;

	private readonly IServiceProxy serviceProxy;
	private readonly IOrderingProxy orderingProxy;
	private readonly ILogger<OrderPlacementQueueService> logger;
	private readonly IEmailService emailService;

	public OrderPlacementQueueService(IServiceProxy serviceProxy, IOrderingProxy orderingProxy, IEmailService emailService, ILogger<OrderPlacementQueueService> logger)
	{
		this.serviceProxy = serviceProxy;
		this.orderingProxy = orderingProxy;
		this.logger = logger;
		this.emailService = emailService;
	}
	
	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(OrderPlacementQueueService));
		bool firstTime = true;
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!ProcessOrderQueue())
			{
				await Task.Delay(PROCESSING_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool ProcessOrderQueue()
	{
		logger.LogInformation("Checking for queued orders that need sending");
	}
}