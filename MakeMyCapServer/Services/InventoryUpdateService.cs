using ShopifyInventoryFulfillment.Configuration;
using ShopifyInventoryFulfillment.Shopify;

namespace ShopifyInventoryFulfillment.Services;

public sealed class InventoryUpdateService : IScopedProcessingService 
{
	// TODO: pick this up from a database and check for updates to implement changed timeouts
	public const int DELAY_TIMEOUT_MSEC = 15000;

	private IInventoryService inventoryService;
	private readonly ILogger<InventoryUpdateService> logger;

	public InventoryUpdateService(IInventoryService inventoryService, ILogger<InventoryUpdateService> logger)
	{
		this.inventoryService = inventoryService;
		this.logger = logger;
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(InventoryUpdateService));
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateInventory())
			{
				await Task.Delay(DELAY_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool UpdateInventory()
	{
		logger.LogInformation("Checking for inventory changes");
		try
		{
			var inventoryLevels = inventoryService.GetInventoryLevels();
			logger.LogInformation("Got " + inventoryLevels.Count + " items!");

			foreach (var inventoryLevel in inventoryLevels)
			{
				var inventory = inventoryService.GetInventoryItem(inventoryLevel.InventoryItemId);
				if (inventory != null)
				{
					logger.LogInformation("Found item for " + inventory.Id + " with SKU of " + inventory.Sku);
				}
			}
			
		}
		catch (Exception ex)
		{
			logger.LogError("Caught exception: " + ex);
		}
		// // read an item that has not been attempted and send it.
		// // if nothing, get something attempted over 5 mins ago
		// var toSend = emailProxy.GetUnattemptedMessages();
		// if (toSend.Count == 0)
		// {
		// 	toSend = emailProxy.GetRetryMessages();
		// }
		//
		// if (toSend.Count > 0)
		// {
		// 	var entry = toSend[0];
		// 	try
		// 	{
		// 		emailSenderService.SendSingleMessage(entry.EmailRecipient, entry.EmailSender, entry.EmailSubject, entry.EmailBody);
		// 		emailProxy.DeleteMessage(entry);
		// 	}
		// 	catch (Exception ex)
		// 	{
		// 		logger.Log(LogLevel.Information, "Error sending message " + entry.Id + ": ", ex);
		// 		emailProxy.MarkMessageFailed(entry);
		// 	}
		//
		// 	return true;
		// }

		return false;
	}
}