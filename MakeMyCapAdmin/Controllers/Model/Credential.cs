using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapAdmin.Controllers.Model;

public class Credential
{
	[Required] 
	[DisplayName("User name")]
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}