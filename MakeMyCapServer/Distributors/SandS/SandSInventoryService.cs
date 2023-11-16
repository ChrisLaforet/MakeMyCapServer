using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MakeMyCapServer.Distributors.SandS.Dtos;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Services;

namespace MakeMyCapServer.Distributors.SandS;

public class SandSInventoryService : IInventoryService
{
	public const string ACCOUNT_NUMBER = "SandSAccountNumber";
	public const string API_KEY = "SandSApiKey";
	
	public const string INVENTORY_ENDPOINT = "https://api.ssactivewear.com/v2/inventory/";
	
	private readonly IConfigurationLoader configurationLoader;
	private readonly ILogger<SandSInventoryService> logger;
	
	public SandSInventoryService(IConfigurationLoader configurationLoader, ILogger<SandSInventoryService> logger)
	{
		this.configurationLoader = configurationLoader;
		this.logger = logger;
	}

	private void PrepareHeadersFor(HttpClient client)
	{
		client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
		client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json; charset=utf-8");
	}
	
	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
		var accountNumber = configurationLoader.GetKeyValueFor(ACCOUNT_NUMBER);
		var apiKey = configurationLoader.GetKeyValueFor(API_KEY);
		
		var client = new HttpClient();
		PrepareHeadersFor(client);

		string url = $"{INVENTORY_ENDPOINT}{string.Join(",", skus.ToArray())}";
		var request = new HttpRequestMessage(HttpMethod.Get, url);
		var authenticationValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountNumber}:{apiKey}"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationValue);
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");

		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			var content = response.Content.ReadFromJsonAsync<QuantityResponse[]>().Result;
			return ExtractStockInventoryFrom(content, skus);
		}
		else
		{
			logger.LogError($"Error in getting inventory: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInStockInventoryFor");
		}
	}

	private List<InStockInventory> ExtractStockInventoryFrom(QuantityResponse[] responses, List<string> skus)
	{
		var found = new List<string>();
		var inStock = new List<InStockInventory>();
		foreach (var response in responses)
		{
			var inStockInventory = new InStockInventory();
			inStockInventory.OurSku = response.YourSku;
			inStockInventory.Sku = response.Sku;
			long available = 0;
			foreach (var warehouse in response.Warehouses)
			{
				available += warehouse.Quantity;
			}
			inStockInventory.Quantity = available;
			found.Add(response.Sku);
			if (!string.IsNullOrEmpty(response.YourSku))
			{
				found.Add(response.YourSku);
			}
			else
			{
				inStockInventory.OurSku = response.Sku;
			}
			inStock.Add(inStockInventory);
		}

		foreach (var sku in skus)
		{
			if (found.Contains(sku))
			{
				continue;
			}
			inStock.Add(new InStockInventory() { Sku = sku });
		}
		
		return inStock;
	}
}