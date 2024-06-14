using MakeMyCapServer.CQS.Interfaces;


public class ChangePasswordCommand : ICommand
{
	public string UserName { get; }
	public string ResetKey { get; }
	public string NewPassword { get; }

	public ChangePasswordCommand(string userName, string resetKey, string newPassword)
	{
		UserName = userName;
		ResetKey = resetKey;
		NewPassword = newPassword;
	}
}