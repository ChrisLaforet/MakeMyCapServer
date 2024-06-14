using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class LogoutCommand : ICommand
{
	public string Username
	{
		get;
	}

	public LogoutCommand(string username)
	{
		this.Username = username;
	}
}