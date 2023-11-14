using MakeMyCap.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Fulfillment;
using MakeMyCapServer.Services.Inventory;
using MakeMyCapServer.Shopify;
using MakeMyCapServer.Shopify.Dtos;

namespace MakeMyCapServer.Services.Background;

public sealed class FulfillmentUpdateService : IFulfillmentProcessingService 
{
	// TODO: pick this up from a database and check for updates to implement changed timeouts
	public const int DELAY_TIMEOUT_MSEC = 60 * 60 * 1000;

	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<InventoryUpdateService> logger;
	private readonly IEmailService emailService; 
	
	private List<SaleProduct> saleProducts = new List<SaleProduct>();

	public FulfillmentUpdateService(IServiceProxy serviceProxy, IEmailService emailService, ILogger<InventoryUpdateService> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
		this.emailService = emailService;
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(InventoryUpdateService));
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateFulfillment())
			{
				await Task.Delay(DELAY_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool UpdateFulfillment()
	{
		logger.LogInformation("Checking for fulfillment orders that need processing");
		ServiceLog? serviceLog = null;
		try
		{
			serviceLog = serviceProxy.CreateServiceLogFor(nameof(InventoryUpdateService));


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