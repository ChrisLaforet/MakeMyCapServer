using MakeMyCapAdmin.CQS.Command;
using MakeMyCapAdmin.CQS.Response;
using MakeMyCapAdmin.Proxies;

namespace MakeMyCapAdmin.CQS.CommandHandler;

public class ChangeSettingsCommandHandler : ICommandHandler<ChangeSettingsCommand, NothingnessResponse>
{
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<ChangeSettingsCommandHandler> logger;
	
	public ChangeSettingsCommandHandler(IServiceProxy serviceProxy, ILogger<ChangeSettingsCommandHandler> logger)
	{
		this.serviceProxy = serviceProxy;
		this.logger = logger;
	}
	
	public NothingnessResponse Handle(ChangeSettingsCommand command)
	{
		try
		{
			logger.LogInformation($"Request to change settings to InventoryCheckHours={command.InventoryCheckHours} FulfillmentCheckHours={command.FulfillmentCheckHours} NextPOSequenceNumber={command.NextPoSequence}");
			serviceProxy.UpdateInventoryCheckHours(command.InventoryCheckHours);
			serviceProxy.UpdateFulfillmentCheckHours(command.FulfillmentCheckHours);
			serviceProxy.UpdateNextPoNumberSequence(command.NextPoSequence);
		}
		catch (Exception ex)
		{
			logger.LogError($"Exception prevents changing settings: {ex}");
		}
		return new NothingnessResponse();
	}
}
