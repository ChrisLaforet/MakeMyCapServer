using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public interface IServiceProxy
{
	int? GetInventoryCheckHours();
	int? GetFulfillmentCheckHours();

	List<string> GetStatusEmailRecipients();

	List<string> GetCriticalEmailRecipients();

	ServiceLog CreateServiceLogFor(string serviceName);
	void CloseServiceLogFor(ServiceLog serviceLog, bool failed = false);

	int GetNextPoNumberSequence();
	void UpdateNextPoNumberSequence(int nextPoNumberSequence);
}