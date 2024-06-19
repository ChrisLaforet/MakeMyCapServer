using MakeMyCapServer.CQS.Response;

namespace MakeMyCapServer.Controllers.Model;

public class ServiceStatus
{
	public List<ServiceLogResponse> Status { get; set; }
}