using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class Settings
{
	[Required]
	[DisplayName("Inventory check (Hours)")]
	[Range(1, 24)]
	public int InventoryCheckHours { get; set; }

	[Required]
	[DisplayName("Fulfillment check (Hours)")]
	[Range(1, 24)]
	public int FulfillmentCheckHours { get; set; }

	[Required]
	[DisplayName("Next PO sequence number")]
	[Range(1, 1000000000)]
	public int NextPoSequence { get; set; }
}