using System.Text;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies.Exceptions;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.OrderPlacement;

namespace MakeMyCapServer.Proxies;

public class NotificationProxy : INotificationProxy
{
	private readonly IServiceProxy serviceProxy;
	private readonly IEmailQueueService emailQueueService;
	private readonly ILogger<NotificationProxy> logger;
	
	public NotificationProxy(IServiceProxy serviceProxy, IEmailQueueService emailQueueService, ILogger<NotificationProxy> logger)
	{
		this.serviceProxy = serviceProxy;
		this.emailQueueService = emailQueueService;
		this.logger = logger;
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
			
			emailQueueService.Add(recipients, subject, body.ToString());
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
			
			emailQueueService.Add(recipients, subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting Critical Notification {subject} because of {ex}");
			throw new NotificationNotSentException(ex);
		}
	}
}