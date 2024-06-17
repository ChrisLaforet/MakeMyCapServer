using MakeMyCapServer.CQS.Interfaces;


public class ChangePasswordCommand : ICommand
{
	public string UserName { get; }
	public string ConfirmationCode { get; }
	public string NewPassword { get; }

	public ChangePasswordCommand(string userName, string confirmationCode, string newPassword)
	{
		UserName = userName;
		ConfirmationCode = confirmationCode;
		NewPassword = newPassword;
	}
}