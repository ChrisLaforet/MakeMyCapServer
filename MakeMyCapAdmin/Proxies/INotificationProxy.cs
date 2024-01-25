namespace MakeMyCapAdmin.Proxies;

public interface INotificationProxy
{
	void SendWarningErrorNotification(string subject, string body, string sender = null);
	void SendCriticalErrorNotification(string subject, string body, string sender = null);
	void SendNotificationToSingleRecipient(string recipient, string subject, string body, string sender = null);
}