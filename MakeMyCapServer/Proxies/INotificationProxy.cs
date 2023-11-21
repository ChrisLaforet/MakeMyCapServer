namespace MakeMyCapServer.Proxies;

public interface INotificationProxy
{
	void SendWarningErrorNotification(string subject, string body, string sender = null);
	void SendCriticalErrorNotification(string subject, string body, string sender = null);
}