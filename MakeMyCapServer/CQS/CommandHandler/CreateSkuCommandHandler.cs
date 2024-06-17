using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class CreateSkuCommandHandler : ICommandHandler<CreateSkuCommand, NothingnessResponse>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<CreateSkuCommandHandler> logger;
	
	public CreateSkuCommandHandler(IProductSkuProxy productSkuProxy, ILogger<CreateSkuCommandHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(CreateSkuCommand command)
	{
		try
		{
			logger.LogInformation($"Creating SKU mapping for {command.Sku} for distributor {command.DistributorCode}");
			productSkuProxy.CreateSkuMap(command.Sku, command.DistributorCode, command.DistributorSku, command.Brand,
											command.StyleCode, command.PartId, command.Color, command.ColorCode, command.SizeCode);
			return new NothingnessResponse();
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Caught exception creating a SKU mapping for {command.Sku}: {ex}");
			throw ex;
		}
	}
}