using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class UpdateInventoryCommandHandler : ICommandHandler<UpdateInventoryCommand, NothingnessResponse>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<UpdateInventoryCommandHandler> logger;
	
	public UpdateInventoryCommandHandler(IProductSkuProxy productSkuProxy, ILogger<UpdateInventoryCommandHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}

	public NothingnessResponse Handle(UpdateInventoryCommand command)
	{
		try
		{
			var inventory = productSkuProxy.GetInHouseInventoryFor(command.Sku);
			if (inventory == null)
			{
				throw new InvalidSkuException(command.Sku);
			}

			logger.LogInformation($"Updating in-house inventory for SKU {command.Sku} with {inventory.OnHand} items");
			inventory.OnHand = command.OnHand >= 0 ? command.OnHand : 0;
			productSkuProxy.SaveInHouseInventory(inventory);
			return new NothingnessResponse();
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Error while updating in-house inventory for SKU {command.Sku}: {ex}");
			throw ex;
		}
	}
}