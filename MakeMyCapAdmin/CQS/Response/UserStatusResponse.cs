using MakeMyCapAdmin.Model;

namespace MakeMyCapAdmin.CQS.Response;

public class UserStatusResponse
{
	public bool EmailExists { get; }
	public bool UserNameExists { get; }

	public UserStatusResponse(bool emailExists, bool userNameExists)
	{
		EmailExists = emailExists;
		UserNameExists = userNameExists;
	}
}