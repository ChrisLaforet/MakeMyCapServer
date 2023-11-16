using SendGrid;
using SendGrid.Helpers.Mail;
using MakeMyCapServer.Configuration;

namespace MakeMyCapServer.Services.Email;

public class SendgridEmailService : IEmailService
{
	public const string SENDGRID_API_KEY = "SendGridWebApiKey";
	public const string SENDGRID_SENDER = "SendGridSender";
	
	private readonly IConfigurationLoader configurationLoader;
	private readonly ILogger<SendgridEmailService> logger;
	
	public SendgridEmailService(IConfigurationLoader configurationLoader, ILogger<SendgridEmailService> logger)
	{        
		this.configurationLoader = configurationLoader;
		this.logger = logger;

	}
	
	public async Task SendMail(string to, string subject, string content, bool isHtml = false)
	{
		try
		{
			var sendgridApiKey = configurationLoader.GetKeyValueFor(SENDGRID_API_KEY);
			var sender = configurationLoader.GetKeyValueFor(SENDGRID_SENDER);
			
			var client = new SendGridClient(sendgridApiKey);
			var from = new EmailAddress(sender, "Make My Cap");
			var recipient = new EmailAddress(to);
			var plainTextContent = isHtml ? "Please see HTML attachment" : content;
			var htmlContent = isHtml ? content : null;
			var messageToSend = MailHelper.CreateSingleEmail(from, recipient, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(messageToSend);
			logger.LogInformation($"Sending mail had a response of {response.StatusCode}");
		}
		catch (Exception ex)
		{
			logger.LogError($"Sending mail had an error: {ex}");
		}
	}
	
	public async Task SendMailToMultipleRecipients(List<string> to, string subject, string content, bool isHtml = false)
	{
		try
		{
			var sendgridApiKey = configurationLoader.GetKeyValueFor(SENDGRID_API_KEY);
			var sender = configurationLoader.GetKeyValueFor(SENDGRID_SENDER);
			
			var client = new SendGridClient(sendgridApiKey);
			var from = new EmailAddress(sender, "Make My Cap");
			var recipients = new List<EmailAddress>();
			foreach (var address in to)
			{
				recipients.Add(new EmailAddress(address));
			}

			var plainTextContent = isHtml ? "Please see HTML attachment" : content;
			var htmlContent = isHtml ? content : null;
			var messageToSend = MailHelper.CreateSingleEmailToMultipleRecipients(from, recipients, subject, plainTextContent, htmlContent);
			var response = await client.SendEmailAsync(messageToSend);
			logger.LogInformation($"Sending mail had a response of {response.StatusCode}");
		}
		catch (Exception ex)
		{
			logger.LogError($"Sending mail had an error: {ex}");
		}
	}
}