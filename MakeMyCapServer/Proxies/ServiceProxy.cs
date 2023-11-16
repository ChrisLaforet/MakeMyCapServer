using MakeMyCapServer.Model;

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