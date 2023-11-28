using System.Text;
using CapAmericaInventory;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors.SanMar;
using MakeMyCapServer.Model;
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
		var lookup = productSkuProxy.GetSkuMapsFor(CAPAMERICA_DISTRIBUTOR_CODE);

		var responses = new List<InStockInventory>();
		foreach (var sku in skus)
		{
			var skuMap = lookup.Find(map => string.Compare(map.Sku, sku, true) == 0);
			if (skuMap != null)
			{
				var task = services.GetInventoryLevelsFor(skuMap.StyleCode);
				task.Wait();
				var inventoryLevels = task.Result;
				if (inventoryLevels.Count > 0)
				{
					var quantity = FindMatchingPartIdIn(inventoryLevels, skuMap);

					var response = new InStockInventory();
					response.Sku = sku;
					response.Quantity = quantity;
					responses.Add(response);
				}
			}
			else
			{
				logger.LogError($"Unable to map sku {sku} for CapAmerica!");
			}
		}

		return responses;
	}

	private int FindMatchingPartIdIn(List<CapAmericaInventoryLevel> inventoryLevels, DistributorSkuMap skuMap)
	{
		var mappedPartId = SanitizePartId(skuMap.PartId);
		int quantity = 0;
		foreach (var inventoryLevel in inventoryLevels)
		{
			if (string.Compare(mappedPartId, SanitizePartId(inventoryLevel.PartId), true) == 0)
			{
				quantity += inventoryLevel.Quantity;
			}
		}

		return quantity;
	}

	private string SanitizePartId(string value)
	{
		var sanitized = new StringBuilder();
		foreach (char ch in value)
		{
			if (char.IsLetterOrDigit(ch) || ch == '-' || ch == '/')
			{
				sanitized.Append(ch);
			}
		}
		return sanitized.ToString();
	}
	
}