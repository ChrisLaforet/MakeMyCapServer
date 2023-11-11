using System.Text.RegularExpressions;
using ShopifyInventoryFulfillment.Configuration;
using ShopifyInventoryFulfillment.Lookup;
using ShopifyInventoryFulfillment.Shopify;
using ShopifyInventoryFulfillment.Shopify.Dtos;

namespace ShopifyInventoryFulfillment.Services;

public sealed class InventoryUpdateService : IScopedProcessingService 
{
	// TODO: pick this up from a database and check for updates to implement changed timeouts
	public const int DELAY_TIMEOUT_MSEC = 10 * 1000;
//	public const int DELAY_TIMEOUT_MSEC = 6 * 60 * 60 * 1000;

	private IInventoryService inventoryService;
	private readonly ILogger<InventoryUpdateService> logger;

	private List<SaleProduct> saleProducts = new List<SaleProduct>();

	public InventoryUpdateService(IInventoryService inventoryService, ILogger<InventoryUpdateService> logger)
	{
		this.inventoryService = inventoryService;
		this.logger = logger;
	}

	public async Task DoWorkAsync(CancellationToken stoppingToken)
	{
		logger.LogInformation("{ServiceName} working", nameof(InventoryUpdateService));
		while (!stoppingToken.IsCancellationRequested)
		{
			if (!UpdateInventory())
			{
				await Task.Delay(DELAY_TIMEOUT_MSEC, stoppingToken);
			}
		}
	}

	private bool UpdateInventory()
	{
		logger.LogInformation("Checking for inventory changes");
		try
		{
			var products = LoadAllProducts();
			foreach (var product in products)
			{
				var match = saleProducts.SingleOrDefault(saleProduct => saleProduct.ProductId == product.Id && saleProduct.VariantId == null);
				if (match == null)
				{
					AddProduct(product);
				}
				else
				{
					GetInventoryLevelFor(match);
				}
			}
			
			foreach (var saleProduct in saleProducts.Where(saleProduct => saleProduct.InventoryItemId != null && !string.IsNullOrEmpty(saleProduct.Sku)).ToList())
			{
				Console.WriteLine($"{saleProduct.Sku} has a level of {saleProduct.InventoryLevel}");
			}
		}
		catch (Exception ex)
		{
			logger.LogError("Caught exception: " + ex);
		}

		TestUpdateAdjustments();
		
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

	private void AddProduct(Product product)
	{
		if (product.Variants == null || product.Variants.Count == 0)
		{
			var saleProduct = new SaleProduct();
			saleProduct.ProductId = product.Id;
			saleProduct.Title = product.Title;
			saleProducts.Add(saleProduct);
			return;
		}
		
		foreach (var variant in product.Variants)
		{
			var saleProduct = new SaleProduct();
			saleProduct.ProductId = product.Id;
			saleProduct.Title = product.Title;
			saleProduct.VariantId = variant.Id;
			saleProduct.InventoryItemId = variant.InventoryItemId;
			saleProduct.Sku = variant.Sku;
			
			GetInventoryLevelFor(saleProduct);
			
			saleProducts.Add(saleProduct);
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
					saleProduct.LocationId = matches[0].LocationId;		// this needs to change if multiple locations
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

	private void TestUpdateAdjustments()
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
				logger.LogError("Caught exception: " + ex);
			}
		}
	}
}