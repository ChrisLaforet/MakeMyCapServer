namespace MakeMyCapServer.Proxies.Exceptions;

public class UserNotFoundException : Exception
{
	public UserNotFoundException(string message) : base(message) {}
}