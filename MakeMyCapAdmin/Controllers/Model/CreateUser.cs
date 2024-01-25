using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapAdmin.Controllers.Model;

public class CreateUser
{
	[Required] 
	[DisplayName("User name")]
	public string UserName { get; set; }

	[Required]
	public string Email { get; set; }
}