using MakeMyCapAdmin.Model;
using MakeMyCapAdmin.Proxies.Exceptions;

namespace MakeMyCapAdmin.Proxies;

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
	
	public void UpdateInventoryCheckHours(int hours)
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			setting = CreateDefaultSetting();
		}

		setting.InventoryCheckHours = hours;
		context.SaveChanges();
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
	
	public void UpdateFulfillmentCheckHours(int hours)
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			setting = CreateDefaultSetting();
		}

		setting.FulfillmentCheckHours = hours;
		context.SaveChanges();
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

	public void SetStatusEmailRecipients(string email1, string? email2 = null, string? email3 = null)
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			setting = CreateDefaultSetting();
		}

		setting.StatusEmailRecipient1 = email1;
		setting.StatusEmailRecipient2 = email2;
		setting.StatusEmailRecipient3 = email3;

		context.SaveChanges();
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
	
	public void SetCriticalEmailRecipients(string email1, string? email2 = null, string? email3 = null)
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			setting = CreateDefaultSetting();
		}

		setting.CriticalEmailRecipient1 = email1;
		setting.CriticalEmailRecipient2 = email2;
		setting.CriticalEmailRecipient3 = email3;

		context.SaveChanges();
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

	public List<ServiceLog> GetLastServiceLogsFor(string serviceName, int number = 3)
	{
		return context.ServiceLogs.Where(log => log.ServiceName == serviceName).OrderByDescending(log => log.StartTime).Take(number).ToList();
	}

	public int GetNextPoNumberSequence()
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			throw new SettingsNotConfiguredException();
		}

		if (setting.NextPosequence == null || setting.NextPosequence <= 0)
		{
			setting.NextPosequence = 1;
			context.SaveChanges();
		}
		return (int)setting.NextPosequence;
	}

	public void UpdateNextPoNumberSequence(int nextPoNumberSequence)
	{
		var setting = context.Settings.FirstOrDefault();
		if (setting == null)
		{
			setting = CreateDefaultSetting();
		}

		setting.NextPosequence = nextPoNumberSequence;
		context.SaveChanges();
	}

	private Setting CreateDefaultSetting()
	{
		var setting = new Setting();
		setting.Id = 1;
		setting.InventoryCheckHours = 8;
		setting.FulfillmentCheckHours = 1;
		setting.NextPosequence = 1;
		context.Settings.Add(setting);
		return setting;
	}
}