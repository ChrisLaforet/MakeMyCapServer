using MakeMyCapServer.Distributors;
using MakeMyCapServer.Distributors.CapAmerica;
using MakeMyCapServer.Distributors.MakeMyCap;
using MakeMyCapServer.Distributors.SandS;
using MakeMyCapServer.Distributors.SanMar;
using MakeMyCapServer.Lookup.Exceptions;

namespace MakeMyCapServer.Lookup;

public class DistributorServiceLookup : IDistributorServiceLookup
{
	private CapAmericaInventoryService capAmericaInventoryService;
	private SanMarInventoryService sanMarInventoryService;
	private SandSInventoryService sandSInventoryService;
	private MakeMyCapInventoryService makeMyCapInventoryService = new MakeMyCapInventoryService();

	private CapAmericaOrderService capAmericaOrderService;
	private SanMarOrderService sanMarOrderService;
	private SandSOrderService sandSOrderService;
	private MakeMyCapOrderService makeMyCapOrderService;
	
	public DistributorServiceLookup(IServiceProvider serviceProvider)
	{
		capAmericaInventoryService = ActivatorUtilities.CreateInstance<CapAmericaInventoryService>(serviceProvider);
		sanMarInventoryService = ActivatorUtilities.CreateInstance<SanMarInventoryService>(serviceProvider);
		sandSInventoryService = ActivatorUtilities.CreateInstance<SandSInventoryService>(serviceProvider);

		capAmericaOrderService = ActivatorUtilities.CreateInstance<CapAmericaOrderService>(serviceProvider);
		sanMarOrderService = ActivatorUtilities.CreateInstance<SanMarOrderService>(serviceProvider);
		sandSOrderService = ActivatorUtilities.CreateInstance<SandSOrderService>(serviceProvider);
		makeMyCapOrderService = ActivatorUtilities.CreateInstance<MakeMyCapOrderService>(serviceProvider);
	}
	
	// For now, let's keep this as a static lookup that maps DistributorLookupCode to the delivery interface code
	public IInventoryService GetInventoryServiceFor(string lookupCode)
	{
		switch (lookupCode)
		{
			case "CA":
				return capAmericaInventoryService;
			
			case "SS":
				return sandSOrderService;
			
			case "SM":
				return sanMarInventoryService;
			
			case "MMC":
				return makeMyCapInventoryService;
		}

		throw new ServiceNotFoundException($"Cannot find an inventory service for {lookupCode}");
	}
	
	// For now, let's keep this as a static lookup that maps DistributorLookupCode to the delivery interface code
	public IOrderService GetOrderServiceFor(string lookupCode)
	{
		switch (lookupCode)
		{
			case "CA":
				return capAmericaOrderService;
			
			case "SS":
				return sanMarOrderService;
			
			case "SM":
				return sandSOrderService;
			
			case "MMC":
				return makeMyCapOrderService;
		}

		throw new ServiceNotFoundException($"Cannot find an order service for {lookupCode}");
	}
}