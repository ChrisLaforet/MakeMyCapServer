using MakeMyCapServer.Services.Background;
using MakeMyCapServer.Services.Fulfillment;

namespace MakeMyCapServer.Webhooks;

public class ShopifyWebhookService
{
	private readonly ILogger<ShopifyWebhookService> logger;
	private IInterruptableService fulfillmentUpdateService;
	
	public ShopifyWebhookService(IServiceProvider serviceProvider, ILogger<ShopifyWebhookService> logger)
	{
		fulfillmentUpdateService = serviceProvider
			.GetServices<IHostedService>()
			.OfType<FulfillmentScopedBackgroundService>()
			.Single();

		this.logger = logger;
	}
	
	public async Task AcceptOrderCreatedNotification(HttpContext context)
	{
		var requestBody = await context.Request.ReadFromJsonAsync<MakeMyCapServer.Shopify.Dtos.Fulfillment.Order>();
		logger.LogInformation($"Received webhook notification of Order Creation for Order Id {requestBody.Id} - requesting immediate processing of Orders.");
		context.Response.StatusCode = 200;
		await context.Response.WriteAsync("ACK");
		
		fulfillmentUpdateService.ResumeProcessingNow();
	}
}