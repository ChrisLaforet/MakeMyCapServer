using MakeMyCapServer.Distributors;

namespace MakeMyCapServer.Lookup;

public interface IDistributorServiceLookup
{
	IInventoryService GetInventoryServiceFor(string lookupCode);
	IOrderService GetOrderServiceFor(string lookupCode);
}