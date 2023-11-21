using CapAmericaInventory;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors.SanMar;
using MakeMyCapServer.Proxies;
using SanMarWebService;

namespace MakeMyCapServer.Distributors.CapAmerica;

public class CapAmericaInventoryService : IInventoryService
{
	public const string USERID = "CapAmericaUserId";
	public const string PASSWORD = "CapAmericaPassword";

	public const string CAPAMERICA_DISTRIBUTOR_CODE = "CA";

	private readonly ILogger<CapAmericaInventoryService> logger;
	private readonly IProductSkuProxy productSkuProxy;
	
	private readonly CapAmericaServices services;
	
	public CapAmericaInventoryService(IConfigurationLoader configurationLoader, IProductSkuProxy productSkuProxy, ILogger<CapAmericaInventoryService> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
		
		var userId = configurationLoader.GetKeyValueFor(USERID);
		var password = configurationLoader.GetKeyValueFor(PASSWORD);
		services = new CapAmericaServices(userId, password);
	}

	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
// TODO: CML - is CapAmerica's ProductId = SKU??  If not we need a mapper like SanMar
	
		var responses = new List<InStockInventory>();
		foreach (var sku in skus)
		{
			var task = services.GetInventoryLevelsFor(sku);
			task.Wait();
			var inventoryLevels = task.Result;
			int quantity = 0;
			foreach (var inventoryLevel in inventoryLevels)
			{
				quantity += inventoryLevel.Quantity;
			}
			
			var response = new InStockInventory();
			response.Sku = sku;
			response.Quantity = quantity;
			responses.Add(response);
		}

		return responses;
	}
}