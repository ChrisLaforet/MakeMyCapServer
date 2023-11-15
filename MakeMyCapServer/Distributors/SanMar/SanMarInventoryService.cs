using MakeMyCap.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Proxies;
using SanMarWebService;

namespace MakeMyCapServer.Distributors.SanMar;

public class SanMarInventoryService : IInventoryService
{
	public const string CUSTOMER_NUMBER = "SanMarCustomerNumber";
	public const string USER_NAME = "SanMarUserName";
	public const string PASSWORD = "SanMarPassword";

	private readonly ILogger<SanMarInventoryService> logger;
	private readonly IProductSkuProxy productSkuProxy;
	
	private readonly SanMarServices services;
	
	public SanMarInventoryService(IConfigurationLoader configurationLoader, IProductSkuProxy productSkuProxy, ILogger<SanMarInventoryService> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
		
		var customerNumber = configurationLoader.GetKeyValueFor(CUSTOMER_NUMBER);
		var userName = configurationLoader.GetKeyValueFor(USER_NAME);
		var password = configurationLoader.GetKeyValueFor(PASSWORD);
		services = new SanMarServices(customerNumber, userName, password);
	}
	
	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
		var lookup = productSkuProxy.GetSanMarSkuMaps();

		var styles = new List<string>();
		var inventoryLevels = new List<SanMarInventoryLevel>();
		var responses = new List<InStockInventory>();
		foreach (var sku in skus)
		{
			var skuMap = lookup.Find(map => string.Compare(map.Sku, sku, true) == 0);
			if (skuMap != null)
			{
				if (!styles.Contains(skuMap.Style.ToUpper()))
				{
					styles.Add(skuMap.Style.ToUpper());
					
					var task = services.GetInventoryLevelsFor(skuMap.Style);		// get them all at one time for the style
					task.Wait();
					inventoryLevels.AddRange(task.Result);
				}

				var match = inventoryLevels.Find(level => string.Compare(level.Style, skuMap.Style, true) == 0 &&
									                                        string.Compare(level.Color, skuMap.Color, true) == 0 &&
									                                        string.Compare(level.Size, skuMap.Size, true) == 0);
				if (match == null)
				{
					logger.LogInformation($"There is no matching inventory level for sku {sku} in SanMar!");
				}
				else
				{
					responses.Add(new InStockInventory() { Sku = sku, Quantity = match.Quantity });
				}
			}
			else
			{
				logger.LogError($"Unable to map sku {sku} for SanMar!");
			}
		}

		return responses;
	}
}