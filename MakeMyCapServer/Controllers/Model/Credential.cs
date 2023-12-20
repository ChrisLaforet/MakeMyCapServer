using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class Credential
{
	[Required] 
	public string UserName { get; set; }

	[Required]
	[DataType(DataType.Password)]
	public string Password { get; set; }
}