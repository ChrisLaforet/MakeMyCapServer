﻿namespace MakeMyCapServer.Services.Email;

public interface IEmailService
{
	Task SendMail(string to, string subject, string content, bool isHtml = false);
}