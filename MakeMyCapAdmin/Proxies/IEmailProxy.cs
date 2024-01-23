using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public interface IEmailProxy
{
	void QueueMessage(string sender, List<string> recipients, string subject, string body);

	List<EmailQueue> GetPendingQueuedMessages();

	EmailQueue? GetMessageById(int id);

	void SaveQueuedMessage(EmailQueue emailQueue);
}