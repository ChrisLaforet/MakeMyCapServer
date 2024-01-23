using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public class EmailProxy : IEmailProxy
{
	private readonly MakeMyCapServerContext context;
	private readonly ILogger<EmailProxy> logger;

	public EmailProxy(MakeMyCapServerContext context, ILogger<EmailProxy> logger)
	{
		this.context = context;
		this.logger = logger;
	}

	public void QueueMessage(string sender, List<string> recipients, string subject, string body)
	{
		var emailQueue = new EmailQueue();
		emailQueue.PostedDateTime = DateTime.Now;
		emailQueue.Sender = sender;
		emailQueue.Body = body;
		emailQueue.Subject = subject;

		emailQueue.Recipient = recipients[0];
		if (recipients.Count >= 2)
		{
			emailQueue.Recipient2 = recipients[1];
		}

		if (recipients.Count >= 3)
		{
			emailQueue.Recipient3 = recipients[2];
		}

		if (recipients.Count >= 4)
		{
			emailQueue.Recipient4 = recipients[3];
		}

		if (recipients.Count > 4)
		{
			logger.LogError($"Request to queue message with {recipients.Count} senders...only using 4 and dropping the rest.");
		}

		context.EmailQueues.Add(emailQueue);
		context.SaveChanges();
	}

	public List<EmailQueue> GetPendingQueuedMessages()
	{
		return context.EmailQueues
			.Where(emailQueue => emailQueue.SentDateTime == null && emailQueue.AbandonedDateTime == null)
			.ToList();
	}

	public EmailQueue? GetMessageById(int id)
	{
		return context.EmailQueues.Find(id);
	}

	public void SaveQueuedMessage(EmailQueue emailQueue)
	{
		context.SaveChanges();
	}
}