using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MakeMyCapServer.CQS.Response;

namespace MakeMyCapServer.Controllers.Model;

public class CreateSku
{
	public List<DistributorResponse>? Distributors { get; set; }
	
	[Required] 
	[DisplayName("MMC Sku")]
	[MaxLength(30)]
	public string Sku { get; set; }

	[Required]
	[DisplayName("Distributor")]
	[MaxLength(5)]
	public string DistributorCode { get; set; }

	[DisplayName("Distributor's Sku")]
	[MaxLength(30)]
	public string? DistributorSku { get; set; }

	[MaxLength(100)]
	public string? Brand { get; set; }

	[Required]
	[MaxLength(50)]
	public string StyleCode { get; set; }

	[MaxLength(50)]
	public string? PartId { get; set; }

	[MaxLength(100)]
	public string? Color { get; set; }

	[MaxLength(10)]
	public string? ColorCode { get; set; }

	[MaxLength(20)]
	public string? SizeCode { get; set; }
}