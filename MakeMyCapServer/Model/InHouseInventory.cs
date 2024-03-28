namespace MakeMyCapServer.Model;

public partial class InHouseInventory
{
	public string Sku { get; set; }

	public int OnHand { get; set; }

	public int LastUsage { get; set; }
}