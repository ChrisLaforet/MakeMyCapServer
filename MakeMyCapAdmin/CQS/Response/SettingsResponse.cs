namespace MakeMyCapAdmin.CQS.Response;

public class SettingsResponse
{
	public int InventoryCheckHours { get; set; }

	public int FulfillmentCheckHours { get; set; }

	public int NextPoSequence { get; set; }
}