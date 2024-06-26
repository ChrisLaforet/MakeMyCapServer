using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.Exception;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.CommandHandler;

public class DeleteSkuCommandHandler : ICommandHandler<DeleteSkuCommand, NothingnessResponse>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<DeleteSkuCommandHandler> logger;
	
	public DeleteSkuCommandHandler(IProductSkuProxy productSkuProxy, ILogger<DeleteSkuCommandHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(DeleteSkuCommand command)
	{
		try
		{
			var record = productSkuProxy.GetSkuMapFor(command.Sku);
			if (record == null)
			{
				throw new InvalidSkuException(command.Sku);
			}

			productSkuProxy.DeleteSkuMap(record);
			return new NothingnessResponse();
		}
		catch (System.Exception ex)
		{
			logger.LogError($"Caught exception deleting a SKU mapping for {command.Sku}: {ex}");
			throw ex;
		}
	}
}