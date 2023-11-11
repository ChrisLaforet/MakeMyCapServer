using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using ShopifyInventoryFulfillment.Configuration;
using ShopifyInventoryFulfillment.Shopify.Dtos;

namespace ShopifyInventoryFulfillment.Shopify;

public class InventoryService : IInventoryService
{
	private string token;
	private ILogger<InventoryService> logger;
	
	public InventoryService(IConfigurationLoader configurationLoader, ILogger<InventoryService> logger)
	{
		this.token = configurationLoader.GetKeyValueFor("ShopifyApiToken");
		this.logger = logger;
	}

	public InventoryItem? GetInventoryItem(long id)
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, $"{ShopifyStore.BaseUrl}/admin/api/2023-10/inventory_items/{id}.json");
		request.Headers.Add("X-Shopify-Access-Token", token);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "ShopifyInventoryFulfillment/1.0");
		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			//Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			var content = response.Content.ReadFromJsonAsync<InventoryItemWrapper>().Result;
			return content == null ? null : content.Item;
		}
		else
		{
			logger.LogError($"Error in GetInventoryItem: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInventoryItem");
		}
	}
	
	public List<InventoryLevel> GetInventoryLevels()
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, $"{ShopifyStore.BaseUrl}/admin/api/2023-10/inventory_levels.json?location_ids={ShopifyStore.Location}&limit=250");
		request.Headers.Add("X-Shopify-Access-Token", token);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "ShopifyInventoryFulfillment/1.0");
		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			// Parse the response body
			//Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			var content = response.Content.ReadFromJsonAsync<InventoryLevels>().Result;
			return content == null ? new List<InventoryLevel>() : new List<InventoryLevel>(content.Items);
		}
		else
		{
			logger.LogError($"Error in GetInventoryLevels: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInventoryLevels");
		}
	}
	
	// For Fiddler use with Http
	// var handler = new HttpClientHandler();
	// handler.ClientCertificateOptions = ClientCertificateOption.Manual;
	// handler.ServerCertificateCustomValidationCallback = 
	// 	(httpRequestMessage, cert, cetChain, policyErrors) =>
	// 	{
	// 		return true;
	// 	};
	// 	
	// 	var client = new HttpClient(handler);
//		var client = new HttpClient();
}