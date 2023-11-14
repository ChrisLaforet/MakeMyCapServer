﻿using MakeMyCap.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Lookup;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.Email;
using MakeMyCapServer.Services.Inventory;
using MakeMyCapServer.Shopify;
using Product = MakeMyCapServer.Shopify.Dtos.Product;

namespace MakeMyCapServer.Services.Background;

public sealed class InventoryUpdateService : IInventoryProcessingService 
{
	public const int DEFAULT_DELAY_TIMEOUT_HOURS = 6;

	private readonly IInventoryService inventoryService;
	private readonly IServiceProxy serviceProxy;
	private readonly ILogger<InventoryUpdateService> logger;
	private readonly IEmailService emailService; 
	
	private int delayTimeoutHours = DEFAULT_DELAY_TIMEOUT_HOURS;
	
	private List<SaleProduct> saleProducts = new List<SaleProduct>();

	public InventoryUpdateService(IInventoryService inventoryService, IServiceProxy serviceProxy, IEmailService emailService, ILogger<InventoryUpdateService> logger)
	{
		this.inventoryService = inventoryService;
		this.serviceProxy = serviceProxy;
		this.logger = logger;
		this.emailService = emailService;

		//emailService.SendMail("laforet@chrislaforetsoftware.com", "Inventory Update Service Started", "This is to confirm that the inventory service has started up and is running.");
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