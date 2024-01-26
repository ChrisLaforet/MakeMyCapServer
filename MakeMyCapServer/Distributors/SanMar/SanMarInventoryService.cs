using System.Text;
using MakeMyCapServer.Model;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Proxies;
using SanMarWebService;

namespace MakeMyCapServer.Distributors.SanMar;

public class SanMarInventoryService : IInventoryService
{
	public const string CUSTOMER_NUMBER = "SanMarInventoryCustomerNumber";
	public const string USER_NAME = "SanMarInventoryUserName";
	public const string PASSWORD = "SanMarInventoryPassword";
	
	public const string ONE_SIZE_FITS_ALL_CODE = "OSFA";
	public const string SANMAR_DISTRIBUTOR_CODE = "SM";

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
		var lookup = productSkuProxy.GetSkuMapsFor(SANMAR_DISTRIBUTOR_CODE);

		var styles = new List<string>();
		var inventoryLevels = new List<SanMarInventoryLevel>();
		var responses = new List<InStockInventory>();
		foreach (var sku in skus)
		{
			var skuMap = lookup.Find(map => string.Compare(map.Sku, sku, true) == 0);
			if (skuMap != null)
			{
				if (!styles.Contains(skuMap.StyleCode.ToUpper()))
				{
					styles.Add(skuMap.StyleCode.ToUpper());

					try
					{
						var task = services.GetInventoryLevelsFor(skuMap.StyleCode); // get them all at one time for the style
						task.Wait();
						inventoryLevels.AddRange(task.Result);
					}
					catch (Exception ex)
					{
						logger.LogError($"SanMar failed call to get inventory for StyleCode {skuMap.StyleCode} for Sku {skuMap.Sku}: {ex}");
					}
				}

				var match = inventoryLevels.Find(level => string.Compare(SanitizeStringValue(level.Style), SanitizeStringValue(skuMap.StyleCode), true) == 0 &&
				                                          string.Compare(SanitizeStringValue(level.Color), SanitizeStringValue(skuMap.Color), true) == 0 &&
				                                          IsSizeEquivalent(level, skuMap));
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

	private bool IsSizeEquivalent(SanMarInventoryLevel level, DistributorSkuMap skuMap)
	{
		var sanitizedLevel = SanitizeStringValue(level.Size).ToUpper();
		var sanitizedSku = SanitizeStringValue(skuMap.SizeCode).ToUpper();
		return sanitizedLevel == sanitizedSku || 
		       (sanitizedLevel == ONE_SIZE_FITS_ALL_CODE && string.IsNullOrEmpty(sanitizedSku));
	}
	
	private string SanitizeStringValue(string? value)
	{
		if (value == null)
		{
			return string.Empty;
		}
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