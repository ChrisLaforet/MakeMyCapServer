using System.Text;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Shopify.Store;
using MakeMyCapServer.Shopify.Dtos.Inventory;

namespace MakeMyCapServer.Shopify.Services;

public class InventoryService : IInventoryService
{
	private readonly IShopifyStore shopifyStore;
	private readonly ILogger<InventoryService> logger;
	
	public InventoryService(IShopifyStore shopifyStore, ILogger<InventoryService> logger)
	{
		this.shopifyStore = shopifyStore;
		this.logger = logger;
	}

	public InventoryItem? GetInventoryItem(long id)
	{
		var task = HttpCommon.SendRequest(HttpMethod.Get, $"{shopifyStore.BaseUrl}/admin/api/2023-10/inventory_items/{id}.json", shopifyStore);
		task.Wait();
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
		var uri = $"{shopifyStore.BaseUrl}/admin/api/2023-10/inventory_levels.json?location_ids={shopifyStore.Location}&limit=250";
		if (inventoryItemIds.Count > 0)
		{
			var filter = "&inventory_item_ids=" + string.Join(",", inventoryItemIds);
			uri += filter;
			uri += filter;
		}
		
		var task = HttpCommon.SendRequest(HttpMethod.Get, uri, shopifyStore);
		task.Wait();
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
		var jsonBodyContent = $"{{\"inventory_item_id\":{inventoryItemId},\"location_id\":{locationId},\"available_adjustment\":{adjustment}}}";
		
		var task = HttpCommon.SendRequest(HttpMethod.Post, $"{shopifyStore.BaseUrl}/admin/api/2023-10/inventory_levels/adjust.json", shopifyStore, jsonBodyContent);
		task.Wait();
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
//			throw new HttpRequestException("AdjustInventoryLevel");
		}
	}

	public List<Product> GetProducts(long? sinceId = null)
	{
		var uri = $"{shopifyStore.BaseUrl}/admin/api/2023-10/products.json?limit=250";
		if (sinceId != null)
		{
			uri += $"&since_id={sinceId}";
		}
		
		var task = HttpCommon.SendRequest(HttpMethod.Get, uri, shopifyStore);
		task.Wait();
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
}