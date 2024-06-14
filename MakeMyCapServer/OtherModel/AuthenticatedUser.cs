namespace MakeMyCapServer.Models;

public class AuthenticatedUser
{
	public string UserName { get; }
	public string Email { get; }
	public string UserId { get; }
	public string Token { get; }

	public AuthenticatedUser(string userName, string email, string userId, string token)
	{
		UserName = userName;
		Email = email;
		UserId = userId;
		Token = token;
	}
}