﻿
namespace MakeMyCapServer.Controllers.Model;

public class NotificationEmails
{
	public string WarningEmail1 { get; set; } = "";
	
	public string? WarningEmail2 { get; set; }

	public string? WarningEmail3 { get; set; }

	public string CriticalEmail1 { get; set; } = "";

	public string? CriticalEmail2 { get; set; }

	public string? CriticalEmail3 { get; set; }
}