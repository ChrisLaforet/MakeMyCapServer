using MakeMyCapAdmin.Model;

namespace MakeMyCapAdmin.Proxies;

public interface IServiceProxy
{
	int? GetInventoryCheckHours();
	void UpdateInventoryCheckHours(int hours);
	int? GetFulfillmentCheckHours();
	void UpdateFulfillmentCheckHours(int hours);

	List<string> GetStatusEmailRecipients();
	void SetStatusEmailRecipients(string email1, string? email2 = null, string? email3 = null);

	List<string> GetCriticalEmailRecipients();
	void SetCriticalEmailRecipients(string email1, string? email2 = null, string? email3 = null);

	ServiceLog CreateServiceLogFor(string serviceName);
	void CloseServiceLogFor(ServiceLog serviceLog, bool failed = false);
	List<ServiceLog> GetLastServiceLogsFor(string serviceName, int number = 3);
	
	int GetNextPoNumberSequence();
	void UpdateNextPoNumberSequence(int nextPoNumberSequence);
}