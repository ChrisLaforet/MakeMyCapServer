using MakeMyCapServer.Lookup;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Shopify;
using Product = MakeMyCapServer.Shopify.Dtos.Product;

namespace MakeMyCapServer.Services.Inventory;

public sealed class InventoryUpdateService : IInventoryProcessingService 
{
	private const int DEFAULT_DELAY_TIMEOUT_HOURS = 6;

	private readonly IInventoryService inventoryService;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<InventoryUpdateService> logger;
	private readonly IEmailQueueService emailQueueService; 
	
	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;

	public InventoryUpdateService(IInventoryService inventoryService, IProductSkuProxy productSkuProxy, IServiceProxy serviceProxy, IEmailQueueService emailQueueService, ILogger<InventoryUpdateService> logger)
	{
		this.inventoryService = inventoryService;
		this.productSkuProxy = productSkuProxy;
		this.serviceProxy = serviceProxy;
		this.logger = logger;
		this.emailQueueService = emailQueueService;

//emailQueueService.Add("laforet@chrislaforetsoftware.com", "Inventory Update Service Started", "This is to confirm that the inventory service has started up and is running.");
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
		var saleProducts = new List<SaleProduct>();

		ServiceLog? serviceLog = null;
		try
		{
			serviceLog = serviceProxy.CreateServiceLogFor(nameof(InventoryUpdateService));

			var products = LoadAllProducts();
			UpdateDatabaseWithProducts(products);
			foreach (var product in products)
			{
				foreach (var variant in product.Variants)
				{
					var matchedProduct = productSkuProxy.GetProductByVariantId(variant.Id);
					if (matchedProduct == null)
					{
						logger.LogError($"Cannot load a database product for Variant Id {variant.Id} for further processing");
						continue;
					}
					var saleProduct = new SaleProduct(matchedProduct);
					saleProducts.Add(saleProduct);
					GetInventoryLevelFor(saleProduct);
				}
			}
			
			foreach (var saleProduct in saleProducts.Where(saleProduct => saleProduct.InventoryItemId != null && !string.IsNullOrEmpty(saleProduct.Sku)).ToList())
			{
				Console.WriteLine($"{saleProduct.Sku} has a level of {saleProduct.InventoryLevel}");
			}

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

// TODO: CML - fix the updater and make it do what it supposed to do		
TestUpdateAdjustments(saleProducts);
		
		return false;
	}

	private List<Product> LoadAllProducts()
	{
		var products = new List<Product>();
		long? lastProductId = null;
		while (true)
		{
			var prods = inventoryService.GetProducts(lastProductId);
			products.AddRange(prods);
			if (prods.Count < 250)
			{
				break;
			}

			lastProductId = prods[prods.Count - 1].Id;
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
				if (matches != null && matches.Count > 0)
				{
// TODO: CML - are we dealing with only ONE location?					
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

	private void TestUpdateAdjustments(List<SaleProduct> saleProducts)
	{
		int level = new Random().Next() % 300;
		if (level == 0)
		{
			level = 400;
		}
		
		Console.WriteLine("Setting level of " + level);

		foreach (var saleProduct in saleProducts.Where(saleProduct => saleProduct.InventoryItemId != null && 
		                                                              saleProduct.LocationId != null && saleProduct.LocationId != 0 &&
		                                                              !string.IsNullOrEmpty(saleProduct.Sku)).ToList())
		{
			int adjustment;
			if (saleProduct.InventoryLevel == null)
			{
				adjustment = level;
			}
			else
			{
				adjustment = level - (int)saleProduct.InventoryLevel;
			}
			
			try
			{
				var response = inventoryService.AdjustInventoryLevel((long)saleProduct.InventoryItemId, (long)saleProduct.LocationId, adjustment);
				if (response != null)
				{
					saleProduct.InventoryLevel = response.Available;
				}
			}
			catch (Exception ex)
			{
				logger.LogError($"Caught exception processing Variant Id {saleProduct.VariantId}: {ex}");
			}
		}
	}
}