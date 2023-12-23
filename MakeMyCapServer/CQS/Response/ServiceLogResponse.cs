using MakeMyCapServer.Model;

namespace MakeMyCapServer.CQS.Response;

public class ServiceLogResponse
{
	public string ServiceName { get; }

	public string StartTime { get; }

	public string? EndTime { get; }
    
	public string? Failed { get; }

	public ServiceLogResponse(ServiceLog serviceLog)
	{
		ServiceName = serviceLog.ServiceName;
		StartTime = serviceLog.StartTime.ToString("yyyy-MM-dd HH:mm:ss");
		EndTime = serviceLog.EndTime == null ? "" : ((DateTime)serviceLog.EndTime).ToString("yyyy-MM-dd HH:mm:ss");
		Failed = serviceLog.Failed == null ? "" : ((bool) serviceLog.Failed).ToString();
	}
}