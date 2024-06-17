using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class PasswordRecoveryCommand : ICommand
{
	public string UserIdentity { get; }

	public PasswordRecoveryCommand(string userIdentity)
	{
		UserIdentity = userIdentity;
	}
}