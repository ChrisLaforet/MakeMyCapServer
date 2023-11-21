namespace MakeMyCapServer.Proxies.Exceptions;

public class NotificationNotSentException : Exception
{
	public NotificationNotSentException(Exception ex) : base("Notification failed with exception", ex) {}
}