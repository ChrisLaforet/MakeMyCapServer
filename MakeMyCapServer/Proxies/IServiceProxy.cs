using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public interface IServiceProxy
{
	int? GetInventoryCheckHours();
	void UpdateInventoryCheckHours(int hours);
	int? GetFulfillmentCheckHours();
	void UpdateFulfillmentCheckHours(int hours);

	List<string> GetStatusEmailRecipients();

	List<string> GetCriticalEmailRecipients();

	ServiceLog CreateServiceLogFor(string serviceName);
	void CloseServiceLogFor(ServiceLog serviceLog, bool failed = false);
	List<ServiceLog> GetLastServiceLogsFor(string serviceName, int number = 3);
	
	int GetNextPoNumberSequence();
	void UpdateNextPoNumberSequence(int nextPoNumberSequence);
}