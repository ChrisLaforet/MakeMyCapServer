namespace MakeMyCapServer.Proxies.Exceptions;

public class RecipientsNotConfiguredException : Exception
{
	public RecipientsNotConfiguredException(string message) : base(message) {}
}