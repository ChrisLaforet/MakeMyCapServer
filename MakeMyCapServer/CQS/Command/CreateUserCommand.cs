namespace MakeMyCapServer.CQS.Command;

public class CreateUserCommand : ICommand
{
	public string UserName { get; }
	public string UserEmail { get;  }

	public CreateUserCommand(string userName, string userEmail)
	{
		UserName = userName;
		UserEmail = userEmail;
	}
}