using MakeMyCapServer.Distributors;
using MakeMyCapServer.Distributors.CapAmerica;
using MakeMyCapServer.Distributors.MakeMyCap;
using MakeMyCapServer.Distributors.SandS;
using MakeMyCapServer.Distributors.SanMar;
using MakeMyCapServer.Lookup.Exceptions;
using LocalSanMarOrderService = MakeMyCapServer.Distributors.SanMar.SanMarOrderService;

namespace MakeMyCapServer.Lookup;

public class DistributorServiceLookup : IDistributorServiceLookup
{
	private readonly CapAmericaInventoryService capAmericaInventoryService;
	private readonly SanMarInventoryService sanMarInventoryService;
	private readonly SandSInventoryService sandSInventoryService;
	private readonly MakeMyCapInventoryService makeMyCapInventoryService = new MakeMyCapInventoryService();
	private readonly MakeMyCapInStockInventoryService makeMyCapInStockInventoryService;

	private readonly CapAmericaOrderService capAmericaOrderService;
	private readonly LocalSanMarOrderService sanMarOrderService;
	private readonly SandSOrderService sandSOrderService;
	private readonly MakeMyCapOrderService makeMyCapOrderService;
	private readonly MakeMyCapInStockOrderService makeMyCapInStockOrderService;

	public DistributorServiceLookup(IServiceProvider serviceProvider)
	{
		capAmericaInventoryService = ActivatorUtilities.CreateInstance<CapAmericaInventoryService>(serviceProvider);
		sanMarInventoryService = ActivatorUtilities.CreateInstance<SanMarInventoryService>(serviceProvider);
		sandSInventoryService = ActivatorUtilities.CreateInstance<SandSInventoryService>(serviceProvider);
		makeMyCapInStockInventoryService = ActivatorUtilities.CreateInstance<MakeMyCapInStockInventoryService>(serviceProvider);

		capAmericaOrderService = ActivatorUtilities.CreateInstance<CapAmericaOrderService>(serviceProvider);
		sanMarOrderService = ActivatorUtilities.CreateInstance<LocalSanMarOrderService>(serviceProvider);
		sandSOrderService = ActivatorUtilities.CreateInstance<SandSOrderService>(serviceProvider);
		makeMyCapOrderService = ActivatorUtilities.CreateInstance<MakeMyCapOrderService>(serviceProvider);
		makeMyCapInStockOrderService = ActivatorUtilities.CreateInstance<MakeMyCapInStockOrderService>(serviceProvider);
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
			
			case "INSTK":
				return makeMyCapInStockInventoryService;
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
			
			case "SM":
				return sanMarOrderService;
			
			case "SS":
				return sandSOrderService;
			
			case "MMC":
				return makeMyCapOrderService;
			
			case "INSTK":
				return makeMyCapInStockOrderService;
		}

		throw new ServiceNotFoundException($"Cannot find an order service for {lookupCode}");
	}
}