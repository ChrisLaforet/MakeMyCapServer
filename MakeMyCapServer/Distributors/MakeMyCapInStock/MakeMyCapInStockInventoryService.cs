using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.Distributors.MakeMyCapInStock;

public class MakeMyCapInStockInventoryService : IInventoryService
{
	public const string MMC_INSTOCK_DISTRIBUTOR_CODE = "INSTK";
	
	private readonly ILogger<MakeMyCapInStockInventoryService> logger;
	private readonly IProductSkuProxy productSkuProxy;

	public MakeMyCapInStockInventoryService(IProductSkuProxy productSkuProxy, ILogger<MakeMyCapInStockInventoryService> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
		var lookup = productSkuProxy.GetSkuMapsFor(MMC_INSTOCK_DISTRIBUTOR_CODE);

		var responses = new List<InStockInventory>();
		foreach (var sku in skus)
		{
			var skuMap = lookup.Find(map => string.Compare(map.Sku, sku, true) == 0);
			if (skuMap != null)
			{
				var response = new InStockInventory();
				response.Sku = sku;
				response.Quantity = 0;		// creates an empty entry to be updated with in-house inventory later
				responses.Add(response);
			}
			else
			{
				logger.LogError($"Unable to map sku {sku} for MakeMyCapInStock!");
			}
		}

		return responses;
	}
}