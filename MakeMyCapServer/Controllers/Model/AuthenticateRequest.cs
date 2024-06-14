using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class AuthenticateRequest
{
	[Required]
	public string Username { get; set; }

	[Required]
	public string Password { get; set; }
}