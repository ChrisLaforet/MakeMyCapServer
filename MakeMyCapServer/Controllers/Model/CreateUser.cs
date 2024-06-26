using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MakeMyCapServer.Controllers.Model;

public class CreateUser
{
	public string UserName { get; set; }
	public string Email { get; set; }
}