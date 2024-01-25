using MakeMyCapAdmin.Model;

namespace MakeMyCapAdmin.CQS.Response;

public class UserResponse
{
	public string UserName { get; }
	public string Email { get; }
	public string Token { get; }

	public UserResponse(string userName, string email, string token)
	{
		UserName = userName;
		Email = email;
		Token = token;
	}
}