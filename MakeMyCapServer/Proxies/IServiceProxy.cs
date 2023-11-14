using MakeMyCap.Model;

namespace MakeMyCapServer.Proxies;

public interface IServiceProxy
{
	int? GetInventoryCheckHours();
	int? GetFulfillmentCheckHours();

	ServiceLog CreateServiceLogFor(string serviceName);
	void CloseServiceLogFor(ServiceLog serviceLog, bool failed = false);
}