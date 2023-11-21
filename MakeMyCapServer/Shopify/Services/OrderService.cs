using System.Text.Json;
using MakeMyCapServer.Shopify.Dtos.Fulfillment;
using MakeMyCapServer.Shopify.Store;
using ShopifyOrders = MakeMyCapServer.Shopify.Dtos.Fulfillment.Orders;

namespace MakeMyCapServer.Shopify.Services;

public class OrderService : IOrderService
{
	private readonly IShopifyStore shopifyStore;
	private readonly ILogger<OrderService> logger;
	
	public OrderService(IShopifyStore shopifyStore, ILogger<OrderService> logger)
	{
		this.shopifyStore = shopifyStore;
		this.logger = logger;
	}
	
	public List<Order> GetOpenOrders()
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, $"{shopifyStore.BaseUrl}/admin/api/2023-10/orders.json?status=open");
		request.Headers.Add("X-Shopify-Access-Token", shopifyStore.ApiToken);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
		var task = client.SendAsync(request);
		task.Wait();
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
// 			Console.WriteLine(response.Content.ReadAsStringAsync().Result);
// 			 var s = response.Content.ReadAsStringAsync().Result;
// 			 var content = JsonSerializer.Deserialize<ShopifyOrders>(s);
// return null;
			
			var content = response.Content.ReadFromJsonAsync<ShopifyOrders>().Result;
			return content == null ? new List<Order>() : new List<Order>(content.Items);
		}
		else
		{
			logger.LogError($"Error in GetOpenOrders: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetOpenOrders");
		}	}

	public Order? GetOrder(long orderId)
	{
		var client = new HttpClient();
		var request = new HttpRequestMessage(HttpMethod.Get, $"{shopifyStore.BaseUrl}/admin/api/2023-10/orders/{orderId}.json");
		request.Headers.Add("X-Shopify-Access-Token", shopifyStore.ApiToken);
		request.Headers.Add("Accept", "application/json");
		request.Headers.Add("User-Agent", "MakeMyCapServer/1.0");
		var task = client.SendAsync(request);
		task.Wait();
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
// 			Console.WriteLine(response.Content.ReadAsStringAsync().Result);
// 			 var s = response.Content.ReadAsStringAsync().Result;
// 			 var content = JsonSerializer.Deserialize<Order>(s);
// return null;
			 var content = response.Content.ReadFromJsonAsync<OrderWrapper>().Result;
			 return content.Order;
		}
		else
		{
			logger.LogError($"Error in GetOrder: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetInventoryItem");
		}
	}
}