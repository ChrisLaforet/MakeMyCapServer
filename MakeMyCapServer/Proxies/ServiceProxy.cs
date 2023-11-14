using MakeMyCap.Model;

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
	
	public int GetInventoryCheckHours()
	{
		throw new NotImplementedException();
	}

	public int GetFulfillmentCheckHours()
	{
		throw new NotImplementedException();
	}

	public ServiceLog CreateServiceLogFor(string serviceName)
	{
		var serviceLog = new ServiceLog();
		serviceLog.ServiceName = serviceName;
		serviceLog.StartTime = DateTime.Now;
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