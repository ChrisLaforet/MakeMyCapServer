using MakeMyCapServer.Controllers.Model;

namespace MakeMyCapServer.CQS.Command;

public class AuthenticateCommand : ICommand
{
	public string UserName { get; }
	public string Password { get; }

	public AuthenticateCommand(string userName, string password)
	{
		UserName = userName;
		Password = password;
	}
}