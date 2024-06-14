using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class UserIdentity
{
	[Required] 
	[DisplayName("User Email")]
	public string Email { get; set; }
}