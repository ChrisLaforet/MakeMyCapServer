using MakeMyCapServer.CQS.Interfaces;

namespace MakeMyCapServer.CQS.Command;

public class TokenValidationCommand : ICommand
{
	public string Token { get; }

	public TokenValidationCommand(string token)
	{
		Token = token;
	}
}