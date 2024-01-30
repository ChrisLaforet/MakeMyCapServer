using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MakeMyCapServer.Distributors.SandS.Dtos;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services;

namespace MakeMyCapServer.Distributors.SandS;

public class SandSInventoryService : IInventoryService
{
	public const string ACCOUNT_NUMBER = "SandSAccountNumber";
	public const string API_KEY = "SandSApiKey";
	
	public const string S_AND_S_DISTRIBUTOR_CODE = "SS";

	public const string INVENTORY_ENDPOINT = "https://api.ssactivewear.com/v2/inventory/";
	
	private readonly IConfigurationLoader configurationLoader;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<SandSInventoryService> logger;
	
	public SandSInventoryService(IConfigurationLoader configurationLoader, IProductSkuProxy productSkuProxy, ILogger<SandSInventoryService> logger)
	{
		this.configurationLoader = configurationLoader;
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}

	private void PrepareHeadersFor(HttpClient client)
	{
		client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
		client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json; charset=utf-8");
	}
	
	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{

		var lookup = productSkuProxy.GetSkuMapsFor(S_AND_S_DISTRIBUTOR_CODE);
		var maps = MapSkusFrom(skus);
		
		var accountNumber = configurationLoader.GetKeyValueFor(ACCOUNT_NUMBER);
		var apiKey = configurationLoader.GetKeyValueFor(API_KEY);
		
		var client = new HttpClient();
		PrepareHeadersFor(client);

		string url = $"{INVENTORY_ENDPOINT}{string.Join(",", maps.Select(map => map.DistributorSku).ToArray())}";
		var request = new HttpRequestMessage(HttpMethod.Get, url);
		var authenticationValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountNumber}:{apiKey}"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationValue);
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");

		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			var content = response.Content.ReadFromJsonAsync<QuantityResponse[]>().Result;
			return ExtractStockInventoryFrom(content, maps);
		}
		else
		{
			logger.LogError($"Error in getting inventory: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInStockInventoryFor");
		}
	}

	private List<DistributorSkuMap> MapSkusFrom(List<string> skus)
	{
		var lookup = productSkuProxy.GetSkuMapsFor(S_AND_S_DISTRIBUTOR_CODE);
		var maps = new List<DistributorSkuMap>();
		foreach (string sku in skus.Distinct())
		{
			var match = lookup.SingleOrDefault(map => string.Compare(map.Sku, sku, true) == 0);
			if (match == null)
			{
				logger.LogInformation($"There is no matching inventory level for sku {sku} in S&S!");
			}
			else
			{
				maps.Add(match);
			}
		}
		
		return maps;
	}

	private List<InStockInventory> ExtractStockInventoryFrom(QuantityResponse[] responses, List<DistributorSkuMap> maps)
	{
		var found = new List<string>();
		var inStock = new List<InStockInventory>();

		var warehouseMap = AssembleSkuWarehouses(responses);
		foreach (var distributorSku in warehouseMap.Keys)
		{
			var match = maps.SingleOrDefault(map => string.Compare(map.DistributorSku, distributorSku, true) == 0);
			if (match == null)
			{
				logger.LogError($"S&S returned a SKU ({distributorSku}) that does not match one we requested.");
				continue;
			}
			
			var inStockInventory = new InStockInventory();
			inStockInventory.DistributorSku = distributorSku;
			inStockInventory.Sku = match.Sku;
			long available = 0;
			foreach (var warehouse in warehouseMap[distributorSku])
			{
				available += warehouse.Quantity;
			}
			inStockInventory.Quantity = available;
			
			found.Add(match.Sku);
			inStock.Add(inStockInventory);
		}

		// populate all that were not returned with zero quantity
		foreach (var sku in maps.Select(map => map.Sku))
		{
			if (found.Contains(sku))
			{
				continue;
			}
			inStock.Add(new InStockInventory() { Sku = sku });
		}
		
		return inStock;
	}

	private Dictionary<string, List<Warehouse>> AssembleSkuWarehouses(QuantityResponse[] responses)
	{
		var map = new Dictionary<string, List<Warehouse>>();
		foreach (var response in responses)
		{
			var key = response.Sku;
			if (!map.ContainsKey(key))
			{
				map[key] = new List<Warehouse>();
			}

			var warehouses = map[key];
			warehouses.AddRange(response.Warehouses);
		}

		return map;
	}
}