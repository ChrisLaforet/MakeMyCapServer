using MakeMyCapServer.Distributors;
using MakeMyCapServer.Distributors.CapAmerica;
using MakeMyCapServer.Distributors.MakeMyCap;
using MakeMyCapServer.Distributors.SandS;
using MakeMyCapServer.Distributors.SanMar;
using MakeMyCapServer.Lookup.Exceptions;

namespace MakeMyCapServer.Lookup;

public class DistributorServiceLookup : IDistributorServiceLookup
{
	private readonly CapAmericaInventoryService capAmericaInventoryService;
	private readonly SanMarInventoryService sanMarInventoryService;
	private readonly SandSInventoryService sandSInventoryService;
	private readonly MakeMyCapInventoryService makeMyCapInventoryService = new MakeMyCapInventoryService();

	private readonly CapAmericaOrderService capAmericaOrderService;
	private readonly SanMarOrderService sanMarOrderService;
	private readonly SandSOrderService sandSOrderService;
	private readonly MakeMyCapOrderService makeMyCapOrderService;
	
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
				return sandSInventoryService;
			
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