namespace MakeMyCapServer.CQS.Command;

public class ChangeSettingsCommand : ICommand
{
	public int InventoryCheckHours { get; }

	public int FulfillmentCheckHours { get; }

	public int NextPoSequence { get; }

	public ChangeSettingsCommand(int inventoryCheckHours, int fulfillmentCheckHours, int nextPoSequence)
	{
		InventoryCheckHours = inventoryCheckHours;
		FulfillmentCheckHours = fulfillmentCheckHours;
		NextPoSequence = nextPoSequence;
	}
}