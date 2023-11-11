namespace ShopifyInventoryFulfillment.Services;


public interface IScopedProcessingService
{
	Task DoWorkAsync(CancellationToken stoppingToken);
}