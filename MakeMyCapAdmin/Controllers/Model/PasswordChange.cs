using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class PasswordChange
{
	[Required] 
	[DisplayName("User name")]
	public string UserName { get; set; }

	[Required] 
	[DisplayName("Reset key")]
	public string ResetKey { get; set; }
	
	[Required]
	[MinLength(8)]
	[DisplayName("New password")]
	[DataType(DataType.Password)]
	public string NewPassword { get; set; }
	
	[Required]
	[Compare("NewPassword")]
	[DisplayName("Confirm password")]
	[DataType(DataType.Password)]
	public string ConfirmPassword { get; set; }
}