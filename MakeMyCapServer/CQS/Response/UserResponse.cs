using MakeMyCapServer.Model;

namespace MakeMyCapServer.CQS.Response;

public class UserResponse
{
	public string UserName { get; }
	public string Email { get; }
	public string UserId { get; }
	public string Token { get; }

	public UserResponse(string userName, string email, string userId, string token)
	{
		UserName = userName;
		Email = email;
		UserId = userId;
		Token = token;
	}
}