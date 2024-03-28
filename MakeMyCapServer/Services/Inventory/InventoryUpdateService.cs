using MakeMyCapServer.Distributors;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;
using IInventoryService = MakeMyCapServer.Shopify.Services.IInventoryService;
using Product = MakeMyCapServer.Shopify.Dtos.Inventory.Product;

namespace MakeMyCapServer.Services.Inventory;

public sealed class InventoryUpdateService : IInventoryProcessingService 
{
	private const int DEFAULT_DELAY_TIMEOUT_HOURS = 6;

	private readonly IInventoryService inventoryService;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly IServiceProxy serviceProxy;
	private readonly IDistributorServiceLookup distributorServiceLookup;
	private readonly ILogger<InventoryUpdateService> logger;
	private readonly IEmailQueueService emailQueueService; 
	
	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;

	public InventoryUpdateService(IInventoryService inventoryService, 
								IProductSkuProxy productSkuProxy, 
								IServiceProxy serviceProxy,
								IDistributorServiceLookup distributorServiceLookup,
								IEmailQueueService emailQueueService, 
								ILogger<InventoryUpdateService> logger)
	{
		this.inventoryService = inventoryService;
		this.productSkuProxy = productSkuProxy;
		this.serviceProxy = serviceProxy;
		this.distributorServiceLookup = distributorServiceLookup;
		this.logger = logger;
		this.emailQueueService = emailQueueService;
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(InventoryUpdateService));

		bool firstTime = true;
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateInventory())
			{
				CheckAndUpdateDelay(firstTime);
				firstTime = false;

				await Task.Delay(delayTimeoutHours * 60 * 60 * 1000, stoppingToken);
			}
		}
	}

	private void CheckAndUpdateDelay(bool firstTime = false)
	{
		int? hours = serviceProxy.GetInventoryCheckHours();
		if (hours == null)
		{
			if (firstTime)
			{
				logger.LogInformation($"Setting delay time for {nameof(InventoryUpdateService)} to default {DEFAULT_DELAY_TIMEOUT_HOURS} hours between processing tasks");
			}
			return;
		}
		
		if (hours != delayTimeoutHours)
		{
			logger.LogInformation($"Setting delay time for {nameof(InventoryUpdateService)} to {hours} hours between processing tasks");
			delayTimeoutHours = (int)hours;
		}
	}
	
	private bool UpdateInventory()
	{
		logger.LogInformation("Checking for inventory changes");

		ServiceLog? serviceLog = null;
		try
		{
			serviceLog = serviceProxy.CreateServiceLogFor(nameof(InventoryUpdateService));

			var products = LoadAllProducts();
			UpdateDatabaseWithProducts(products);

			var productMap = new Dictionary<long, List<SaleProduct>>();		// track all variants together
			foreach (var product in products)
			{
				foreach (var variant in product.Variants)
				{
					if (string.IsNullOrEmpty(variant.Sku))
					{
						continue;
					}
					var matchedProduct = productSkuProxy.GetProductByVariantId(variant.Id);
					if (matchedProduct == null)
					{
						logger.LogError($"Cannot load a database product for Variant Id {variant.Id} for further processing");
						continue;
					}
					var saleProduct = new SaleProduct(matchedProduct);
					GetInventoryLevelFor(saleProduct);

					if (!productMap.ContainsKey(saleProduct.ProductId))
					{
						productMap[saleProduct.ProductId] = new List<SaleProduct>();
					}
					productMap[saleProduct.ProductId].Add(saleProduct);
				}
			}

			UpdateInventoryLevelsFor(productMap);

			serviceProxy.CloseServiceLogFor(serviceLog);
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception: {ex}");
			if (serviceLog != null)
			{
				serviceProxy.CloseServiceLogFor(serviceLog, true);
			}
		}
		
		return false;
	}

	private List<Product> LoadAllProducts()
	{
		var products = new List<Product>();
		long? lastProductId = null;
		while (true)
		{
			var allProducts = inventoryService.GetProducts(lastProductId);
			products.AddRange(allProducts);
			if (allProducts.Count < 250)
			{
				break;
			}

			lastProductId = allProducts[allProducts.Count - 1].Id;
		}

		return products;
	}

	private void UpdateDatabaseWithProducts(List<Product> products)
	{
		try
		{
			foreach (var product in products)
			{
				foreach (var variant in product.Variants)
				{
					var matchedProduct = productSkuProxy.GetProductByVariantId(variant.Id);
					if (matchedProduct == null && !string.IsNullOrEmpty(variant.Sku))
					{
						var skuMatch = productSkuProxy.GetProductBySku(variant.Sku);
						if (skuMatch != null)
						{
							logger.LogError(
								$"Warning: Database already contains SKU {skuMatch.Sku} for Variant Id {skuMatch.VariantId} but it is duplicated in Variant Id {variant.Id}: not adding this record");
						}
						else
						{
							var record = new MakeMyCapServer.Model.Product();
							record.VariantId = variant.Id;
							record.Sku = variant.Sku;
							record.ProductId = product.Id;
							record.InventoryItemId = variant.InventoryItemId;
							record.Title = $"{product.Title}: {variant.Title}";
							record.Vendor = product.Vendor;
							productSkuProxy.AddProduct(record);
						}
					}
					// TODO: Maybe we need to update skus and so on if not correct??
				}
			}
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught exception in UpdateDatabaseWithProducts: {ex}");
		}
	}

	private void GetInventoryLevelFor(SaleProduct saleProduct)
	{
		if (saleProduct.InventoryItemId != null)
		{
			try
			{
				var inventoryItemIds = new List<long>();
				inventoryItemIds.Add((long)saleProduct.InventoryItemId);

				var matches = inventoryService.GetInventoryLevels(inventoryItemIds);
				if (matches.Count > 0)
				{
					saleProduct.LocationId = matches[0].LocationId;		// NOTE!!  this needs to change if multiple locations
					saleProduct.InventoryLevel = matches[0].Available;
					return;
				}
			}
			catch (Exception ex)
			{
				logger.LogError("Caught exception: " + ex);
			}
		}

		saleProduct.InventoryLevel = null;
	}

	private void UpdateInventoryLevelsFor(Dictionary<long, List<SaleProduct>> productMap)
	{
		foreach (var key in productMap.Keys)
		{
			var variants = new List<SaleProduct>();
			var distributorCode = string.Empty;
			foreach (var variant in productMap[key])
			{
				if (variant.InventoryItemId == null || string.IsNullOrEmpty(variant.Sku))
				{
					continue;
				}

				var skuMatch = productSkuProxy.GetSkuMapFor(variant.Sku);
				if (skuMatch == null)
				{
					logger.LogError($"Unable to find a SkuMap match for inventory level check for Sku {variant.Sku} which is in Shopify Product Id {variant.ProductId}");
					continue;
				}

				if (distributorCode.Length == 0)
				{
					distributorCode = skuMatch.DistributorCode;
				}
				else if (string.Compare(distributorCode, skuMatch.DistributorCode, true) != 0)
				{
					logger.LogError(
						$"Distributor code {skuMatch.DistributorCode} for Sku {variant.Sku} does not match others {distributorCode} in the same product variantss for Shopify Product Id {variant.ProductId}");
					continue;
				}

				variants.Add(variant);
			}

			if (variants.Count > 0)
			{
				var inventoryService = distributorServiceLookup.GetInventoryServiceFor(distributorCode);
				if (inventoryService == null)
				{
					logger.LogError($"Unable to find an inventory service for Distributor code {distributorCode} while checking for Skus in Shopify Product Id {variants[0].ProductId}");
					continue;
				}

				var inStockInventories = inventoryService.GetInStockInventoryFor(variants.Select(v => v.Sku).ToList());
				AdjustInStockInventoriesWithInHouseQuantities(inStockInventories);
				AdjustInventoryLevelsFor(inStockInventories, variants);
			}
		}
	}

	private void AdjustInStockInventoriesWithInHouseQuantities(List<InStockInventory> inStockInventories)
	{
		inStockInventories.ForEach(inventory =>
		{
			var inHouse = productSkuProxy.GetInHouseInventoryFor(inventory.Sku);
			if (inHouse != null && inHouse.OnHand > 0)
			{
				inventory.Quantity += inHouse.OnHand;
			}
		});
	}

	private void AdjustInventoryLevelsFor(List<InStockInventory> inStockInventories, List<SaleProduct> variants)
	{
		foreach (var inStockInventory in inStockInventories)
		{
			var variant = variants.SingleOrDefault(v => string.Compare(v.Sku, inStockInventory.Sku, true) == 0);
			if (variant == null)
			{
				logger.LogError($"Something went wrong - cannot find Sku {inStockInventory.Sku} from inventory level response among the variants!");
				continue;
			}

			if (variant.InventoryItemId == null || variant.LocationId == null)
			{
				logger.LogError($"Variant with Sku {variant.Sku} either does not have an InventoryItemId or a LocationId for Product Id {variant.ProductId}.");
				continue;
			}

			var variantLevel = variant.InventoryLevel == null ? 0 : (int)variant.InventoryLevel;
			var inStockLevel = inStockInventory.Quantity == null ? 0 : (int)inStockInventory.Quantity;
			var adjustment = inStockLevel - variantLevel;
			if (adjustment != 0)
			{
				logger.LogInformation($"*** Inventory Adjust of {variant.Sku} {variant.Product.Title} by {adjustment}");
				var response = inventoryService.AdjustInventoryLevel((long)variant.InventoryItemId, (long)variant.LocationId, adjustment);
				{
					variant.InventoryLevel = response.Available;
				}
			}
		}
	}
}