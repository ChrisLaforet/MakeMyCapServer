using System.Net;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Text.Json;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Shopify.Dtos;

namespace MakeMyCapServer.Shopify;

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
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			//Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			// var s = response.Content.ReadAsStringAsync().Result;
			// var content = JsonSerializer.Deserialize<InventoryItemWrapper>(s);

			var content = response.Content.ReadFromJsonAsync<InventoryItemWrapper>().Result;
			return content == null ? null : content.InventoryItem;
		}
		else
		{
			logger.LogError($"Error in GetInventoryItem: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInventoryItem");
		}
	}
	
	public List<InventoryLevel> GetInventoryLevels(List<long> inventoryItemIds = null)
	{
		var client = new HttpClient();
		var uri = $"{ShopifyStore.BaseUrl}/admin/api/2023-10/inventory_levels.json?location_ids={ShopifyStore.Location}&limit=250";
		if (inventoryItemIds != null && inventoryItemIds.Count > 0)
		{
			var filter = "&inventory_item_ids=" + string.Join(",", inventoryItemIds);
			uri += filter;
		}
		var request = new HttpRequestMessage(HttpMethod.Get, uri);
		request.Headers.Add("X-Shopify-Access-Token", token);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
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
	
	public InventoryLevel? AdjustInventoryLevel(long inventoryItemId, long locationId, int adjustment)
	{
		var client = new HttpClient();
		var uri = $"{ShopifyStore.BaseUrl}/admin/api/2023-10/inventory_levels/adjust.json";

		var request = new HttpRequestMessage(HttpMethod.Post, uri);
		request.Headers.Add("X-Shopify-Access-Token", token);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");

		var jsonBodyContent = $"{{\"inventory_item_id\":{inventoryItemId},\"location_id\":{locationId},\"available_adjustment\":{adjustment}}}";
		request.Content = new StringContent(jsonBodyContent,
									Encoding.UTF8, 
									"application/json");
			
		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			// Parse the response body
			//Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			var content = response.Content.ReadFromJsonAsync<InventoryLevelWrapper>().Result;
			return content == null ? null : content.Item;
		}
		else
		{
			logger.LogError($"Error in AdjustInventoryLevel: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("AdjustInventoryLevel");
		}
	}

	public List<Product> GetProducts(long? sinceId = null)
	{
		var client = new HttpClient();
		var uri = $"{ShopifyStore.BaseUrl}/admin/api/2023-10/products.json?limit=250";
		if (sinceId != null)
		{
			uri += $"&since_id={sinceId}";
		}
		var request = new HttpRequestMessage(HttpMethod.Get, uri);
		request.Headers.Add("X-Shopify-Access-Token", token);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
		var task = client.SendAsync(request);
		
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			// Parse the response body
			//Console.WriteLine(response.Content.ReadAsStringAsync().Result);
			var content = response.Content.ReadFromJsonAsync<Products>().Result;
			return content == null ? new List<Product>() : new List<Product>(content.Items);
		}
		else
		{
			logger.LogError($"Error in GetProducts: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetProducts");
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