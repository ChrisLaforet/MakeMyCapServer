using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using SendGrid.Helpers.Mail;

namespace MakeMyCapServer.Services.Email;

public class EmailQueueProcessingService : IEmailQueueProcessingService
{
	private const int FAILURE_HOURS = 48;

	private readonly IEmailProxy emailProxy;
	private readonly IEmailSender emailSender;
	private readonly ILogger<EmailQueueProcessingService> logger;
	
	public EmailQueueProcessingService(IEmailProxy emailProxy, IEmailSender emailSender, ILogger<EmailQueueProcessingService> logger)
	{
		this.emailProxy = emailProxy;
		this.emailSender = emailSender;
		this.logger = logger;
	}
	
	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		try
		{
			var queuedMessages = emailProxy.GetPendingQueuedMessages();
			if (queuedMessages.Count > 0)
			{
				logger.LogInformation($"Email queue contains {queuedMessages.Count} message(s)");
				foreach (var queuedMessage in queuedMessages)
				{
					SendQueuedMessage(queuedMessage);
					emailProxy.SaveQueuedMessage(queuedMessage);
				}
			}
		}
		catch (Exception ex)
		{
			logger.LogError($"Error processing Email queue: {ex}");
		}
	}

	private void SendQueuedMessage(EmailQueue queuedMessage)
	{
		try
		{
			queuedMessage.TotalAttempts++;
			queuedMessage.LastAttemptDateTime = DateTime.Now;

			var recipients = ExtractRecipientsFrom(queuedMessage);
			if (recipients.Count == 1)
			{
				emailSender.SendMail(queuedMessage.Sender, recipients[0], queuedMessage.Subject, queuedMessage.Body, queuedMessage.BodyIsHtml);
			}
			else
			{
				emailSender.SendMailToMultipleRecipients(queuedMessage.Sender, recipients, queuedMessage.Subject, queuedMessage.Body, queuedMessage.BodyIsHtml);
			}

			queuedMessage.SentDateTime = DateTime.Now;
		}
		catch (Exception ex)
		{
			logger.LogError($"Error sending Email with Id {queuedMessage.Id}: {ex}");
			
			var now = DateTime.Now;
			var difference = now.Subtract(queuedMessage.PostedDateTime);
			if (difference.TotalHours > FAILURE_HOURS)
			{
				logger.LogCritical($"Failure to send Email with Id {queuedMessage.Id} after {Convert.ToInt32(difference.TotalHours)} hours and {queuedMessage.TotalAttempts}: Marking as ABANDONED");
				queuedMessage.AbandonedDateTime = DateTime.Now;
			}
		}		
	}

	private List<string> ExtractRecipientsFrom(EmailQueue queuedMessage)
	{
		var recipients = new List<string>();
		if (!string.IsNullOrEmpty(queuedMessage.Recipient))
		{
			recipients.Add(queuedMessage.Recipient);
		}
		if (!string.IsNullOrEmpty(queuedMessage.Recipient2))
		{
			recipients.Add(queuedMessage.Recipient2);
		}
		if (!string.IsNullOrEmpty(queuedMessage.Recipient3))
		{
			recipients.Add(queuedMessage.Recipient3);
		}
		if (!string.IsNullOrEmpty(queuedMessage.Recipient4))
		{
			recipients.Add(queuedMessage.Recipient4);
		}

		return recipients;
	}
}