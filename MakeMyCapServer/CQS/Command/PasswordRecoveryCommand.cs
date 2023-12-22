namespace MakeMyCapServer.CQS.Command;

public class PasswordRecoveryCommand : ICommand
{
	public string Email { get; }

	public PasswordRecoveryCommand(string email)
	{
		Email = email;
	}
}