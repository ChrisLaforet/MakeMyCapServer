using MakeMyCapAdmin.Controllers.Model;

namespace MakeMyCapAdmin.CQS.Command;

public class ChangeNotificationsCommand : ICommand
{
	public string WarningEmail1 { get; }
	
	public string? WarningEmail2 { get; }
		
	public string? WarningEmail3 { get; }

	public string CriticalEmail1 { get; }
	
	public string? CriticalEmail2 { get; }
		
	public string? CriticalEmail3 { get; }

	public ChangeNotificationsCommand(NotificationEmails notificationEmails)
	{
		WarningEmail1 = notificationEmails.WarningEmail1;
		WarningEmail2 = notificationEmails.WarningEmail2;
		WarningEmail3 = notificationEmails.WarningEmail3;
		CriticalEmail1 = notificationEmails.CriticalEmail1;
		CriticalEmail2 = notificationEmails.CriticalEmail2;
		CriticalEmail3 = notificationEmails.CriticalEmail3;
	}
}