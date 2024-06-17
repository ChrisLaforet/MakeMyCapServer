using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class PasswordChange
{
	public string UserName { get; set; }

	public string ConfirmationCode { get; set; }
	
	public string Password { get; set; }
}