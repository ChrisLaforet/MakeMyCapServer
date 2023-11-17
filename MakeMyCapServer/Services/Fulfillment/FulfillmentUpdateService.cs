using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;

namespace MakeMyCapServer.Services.Fulfillment;

public sealed class FulfillmentUpdateService : IFulfillmentProcessingService 
{
	private const int DEFAULT_DELAY_TIMEOUT_HOURS = 2;

	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<FulfillmentUpdateService> logger;
	private readonly IEmailQueueService emailQueueService;

	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;

	public FulfillmentUpdateService(IServiceProxy serviceProxy, IEmailQueueService emailQueueService, ILogger<FulfillmentUpdateService> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
		this.emailQueueService = emailQueueService;
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(FulfillmentUpdateService));
		bool firstTime = true;
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateFulfillment())
			{
				CheckAndUpdateDelay(firstTime);
				firstTime = false;

				await Task.Delay(delayTimeoutHours * 60 * 60 * 1000, stoppingToken);
			}
		}
	}

	private void CheckAndUpdateDelay(bool firstTime = false)
	{
		int? hours = serviceProxy.GetFulfillmentCheckHours();
		if (hours == null)
		{
			if (firstTime)
			{
				logger.LogInformation($"Setting delay time for {nameof(FulfillmentUpdateService)} to default {DEFAULT_DELAY_TIMEOUT_HOURS} hours between processing tasks");
			}
			return;
		}
		
		if (hours != delayTimeoutHours)
		{
			logger.LogInformation($"Setting delay time for {nameof(FulfillmentUpdateService)} to {hours} hours between processing tasks");
			delayTimeoutHours = (int)hours;
		}
	}

	private bool UpdateFulfillment()
	{
		logger.LogInformation("Checking for fulfillment orders that need processing");
		ServiceLog? serviceLog = null;
		try
		{
			serviceLog = serviceProxy.CreateServiceLogFor(nameof(FulfillmentUpdateService));
// TODO: CML - Add logic here to get fullfillment orders from Shopify!

			serviceProxy.CloseServiceLogFor(serviceLog);
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception: {ex}");
			if (serviceLog != null)
			{
				serviceProxy.CloseServiceLogFor(serviceLog, true);
			}
		}
		
		return false;
	}
}