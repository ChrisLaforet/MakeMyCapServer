using MakeMyCapAdmin.CQS.Response;

namespace MakeMyCapAdmin.Controllers.Model;

public class ServiceStatus
{
	public List<ServiceLogResponse> EmailServiceStatus { get; set; }
	public List<ServiceLogResponse> FulfillmentServiceStatus { get; set; }
	public List<ServiceLogResponse> InventoryServiceStatus { get; set; }
	public List<ServiceLogResponse> OrderPlacementServiceStatus { get; set; }
}