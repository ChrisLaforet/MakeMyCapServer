namespace MakeMyCapServer.Controllers.Model;

public class JwtResponse
{
	public string Token { get; private set; }
		
	public string Username { get; private set; }

		
	public string Email { get; private set; }

	public JwtResponse(string token, string username, string email)
	{
		this.Token = token;
		this.Username = username;
		this.Email = email;
	}
}