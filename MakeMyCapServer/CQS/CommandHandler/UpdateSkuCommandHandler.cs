using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class UpdateSkuCommandHandler : ICommandHandler<UpdateSkuCommand, NothingnessResponse>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<UpdateSkuCommandHandler> logger;
	
	public UpdateSkuCommandHandler(IProductSkuProxy productSkuProxy, ILogger<UpdateSkuCommandHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(UpdateSkuCommand command)
	{
		try
		{
			var record = productSkuProxy.GetSkuMapFor(command.OriginalSku);
			if (record == null)
			{
				throw new InvalidSkuException(command.OriginalSku);
			}

			record.Sku = command.NewSku;
			record.DistributorSku = command.DistributorSku;
			record.Brand = command.Brand;
			record.PartId = command.PartId;
			record.Color = command.Color;
			record.ColorCode = command.ColorCode;
			record.SizeCode = command.SizeCode;
			record.StyleCode = command.StyleCode;
			logger.LogInformation($"Updating SKU mapping for {command.OriginalSku} for distributor {record.DistributorCode}");
			productSkuProxy.UpdateSkuMap(record);
			return new NothingnessResponse();
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Caught exception updating a SKU mapping for {command.OriginalSku}: {ex}");
			throw ex;
		}
	}
}