using MakeMyCapAdmin.Configuration;
using MakeMyCapAdmin.Proxies.Exceptions;

namespace MakeMyCapAdmin.Proxies;

public class NotificationProxy : INotificationProxy
{
	public const string SENDGRID_SENDER = "SendGridSender";
	
	private readonly string defaultSender;
	
	private readonly IServiceProxy serviceProxy;
	private readonly IEmailProxy emailProxy;

	private readonly ILogger<NotificationProxy> logger;
	
	public NotificationProxy(IServiceProxy serviceProxy, IConfigurationLoader configurationLoader, IEmailProxy emailProxy, ILogger<NotificationProxy> logger)
	{
		this.serviceProxy = serviceProxy;
		this.emailProxy = emailProxy;
		this.logger = logger;
		defaultSender = configurationLoader.GetKeyValueFor(SENDGRID_SENDER);
	}

	public void SendWarningErrorNotification(string subject, string body, string sender = null)
	{
		try
		{
			var recipients = serviceProxy.GetStatusEmailRecipients();
			if (recipients.Count == 0)
			{
				logger.LogCritical($"Warning Notification Email Recipients are not configured to receive notification of {subject}!!");
				throw new RecipientsNotConfiguredException("Warning Notification Email Recipients are not configured!");
			}
			
			emailProxy.QueueMessage(string.IsNullOrEmpty(sender) ? defaultSender : sender, recipients, subject, body);
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting Warning Notification {subject} because of {ex}");
			throw new NotificationNotSentException(ex);
		}
	}

	public void SendCriticalErrorNotification(string subject, string body, string sender = null)
	{
		try
		{
			var recipients = serviceProxy.GetCriticalEmailRecipients();
			if (recipients.Count == 0)
			{
				logger.LogCritical($"Critical Notification Email Recipients are not configured to receive notification of {subject}!!");
				throw new RecipientsNotConfiguredException("Critical Notification Email Recipients are not configured!");
			}
			emailProxy.QueueMessage(string.IsNullOrEmpty(sender) ? defaultSender : sender, recipients, subject, body);
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting Critical Notification {subject} because of {ex}");
			throw new NotificationNotSentException(ex);
		}
	}
	
	public void SendNotificationToSingleRecipient(string recipient, string subject, string body, string sender = null)
	{
		try
		{
			var recipients = new List<string>();
			recipients.Add(recipient);
			
			emailProxy.QueueMessage(string.IsNullOrEmpty(sender) ? defaultSender : sender, recipients, subject, body);
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting Warning Notification {subject} because of {ex}");
			throw new NotificationNotSentException(ex);
		}
	}
}