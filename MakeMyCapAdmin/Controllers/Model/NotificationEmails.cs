using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapAdmin.Controllers.Model;

public class NotificationEmails
{
	[Required]
	[DisplayName("Warning Email 1 (Req)")]
	[MaxLength(120)]
	public string WarningEmail1 { get; set; }
	
	[DisplayName("Warning Email 2")]
	[MinLength(0)]
	[MaxLength(120)]
	public string? WarningEmail2 { get; set; }
		
	[DisplayName("Warning Email 3")]
	[MinLength(0)]
	[MaxLength(120)]
	public string? WarningEmail3 { get; set; }

	[Required]
	[DisplayName("Critical Email 1 (Req)")]
	[MaxLength(120)]
	public string CriticalEmail1 { get; set; }
	
	[DisplayName("Critical Email 2")]
	[MinLength(0)]
	[MaxLength(120)]
	public string? CriticalEmail2 { get; set; }
		
	[DisplayName("Critical Email 3")]
	[MinLength(0)]
	[MaxLength(120)]
	public string? CriticalEmail3 { get; set; }
}