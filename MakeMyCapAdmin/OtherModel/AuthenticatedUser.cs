namespace MakeMyCapAdmin.Models;

public class AuthenticatedUser
{
	public string UserName { get; }
	public string Email { get; }
	public string Token { get; }

	public AuthenticatedUser(string userName, string email, string token)
	{
		UserName = userName;
		Email = email;
		Token = token;
	}
}