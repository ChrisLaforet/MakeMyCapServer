﻿using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public class ServiceProxy : IServiceProxy
{
	private readonly MakeMyCapServerContext context;
	private readonly ILogger<ServiceProxy> logger;
	
	public ServiceProxy(MakeMyCapServerContext context, ILogger<ServiceProxy> logger)
	{
		this.context = context;
		this.logger = logger;
	}
	
	public int? GetInventoryCheckHours()
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			return null;
		}
		return setting.InventoryCheckHours;
	}

	public int? GetFulfillmentCheckHours()
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			return null;
		}
		return setting.FulfillmentCheckHours;
	}

	public List<string> GetStatusEmailRecipients()
	{
		var emails = new List<string>();
		var setting = context.Settings.FirstOrDefault();
		if (setting != null)
		{
			if (!string.IsNullOrEmpty(setting.StatusEmailRecipient1))
			{
				emails.Add(setting.StatusEmailRecipient1);
			}

			if (!string.IsNullOrEmpty(setting.StatusEmailRecipient2))
			{
				emails.Add(setting.StatusEmailRecipient2);
			}

			if (!string.IsNullOrEmpty(setting.StatusEmailRecipient3))
			{
				emails.Add(setting.StatusEmailRecipient3);
			}
		}

		return emails;
	}

	public List<string> GetCriticalEmailRecipients()
	{
		var emails = new List<string>();
		var setting = context.Settings.FirstOrDefault();
		if (setting != null)
		{
			if (!string.IsNullOrEmpty(setting.CriticalEmailRecipient1))
			{
				emails.Add(setting.CriticalEmailRecipient1);
			}

			if (!string.IsNullOrEmpty(setting.CriticalEmailRecipient2))
			{
				emails.Add(setting.CriticalEmailRecipient2);
			}

			if (!string.IsNullOrEmpty(setting.CriticalEmailRecipient3))
			{
				emails.Add(setting.CriticalEmailRecipient3);
			}
		}

		return emails;		
	}
	
	public ServiceLog CreateServiceLogFor(string serviceName)
	{
		var serviceLog = new ServiceLog();
		serviceLog.ServiceName = serviceName;
		serviceLog.StartTime = DateTime.Now;
		context.ServiceLogs.Add(serviceLog);
		context.SaveChanges();
		return serviceLog;
	}

	public void CloseServiceLogFor(ServiceLog serviceLog, bool failed = false)
	{
		try
		{
			serviceLog.EndTime = DateTime.Now;
			serviceLog.Failed = failed;
			context.SaveChanges();
		}
		catch (Exception ex)
		{
			// do not propagate this error because it may break the background task runner
			logger.LogError($"Error closing service logs for {serviceLog.Id} for service {serviceLog.ServiceName}: {ex}");
		}
	}
}