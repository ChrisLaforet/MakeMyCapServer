using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class CreateInventoryCommandHandler : ICommandHandler<CreateInventoryCommand, NothingnessResponse>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<CreateInventoryCommandHandler> logger;
	
	public CreateInventoryCommandHandler(IProductSkuProxy productSkuProxy, ILogger<CreateInventoryCommandHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}

	public NothingnessResponse Handle(CreateInventoryCommand command)
	{
		try
		{
			var skuResponse = productSkuProxy.GetSkuMapFor(command.Sku);
			if (skuResponse == null)
			{
				throw new InvalidSkuException(command.Sku);
			}
			
			var record = new InHouseInventory();
			record.Sku = command.Sku;
			record.OnHand = command.OnHand >= 0 ? command.OnHand : 0;
			record.LastUsage = 0;
			logger.LogInformation($"Creating in-house inventory for SKU {command.Sku} with {record.OnHand} items");
			productSkuProxy.SaveInHouseInventory(record);
			return new NothingnessResponse();
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Error while creating in-house inventory for SKU {command.Sku}: {ex}");
			throw ex;
		}
	}
}