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
		var task = HttpCommon.SendRequest(HttpMethod.Get, $"{shopifyStore.BaseUrl}/admin/api/2023-10/orders.json?status=open", shopifyStore);
		task.Wait();
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
			var content = response.Content.ReadFromJsonAsync<ShopifyOrders>().Result;
			if (content == null)
			{
				return new List<Order>();
			}

			var orders = new List<Order>(content.Items);
			orders.Sort((a,b) =>
			{
				var aValue = a.OrderNumber == null ? 0 : (int)a.OrderNumber;
				var bValue = b.OrderNumber == null ? 0 : (int)b.OrderNumber;
				return aValue - bValue;
			});
			
			return orders;
		}
		else
		{
			logger.LogError($"Error in GetOpenOrders: {(int)response.StatusCode} ({response.ReasonPhrase})");
			throw new HttpRequestException("GetOpenOrders");
		}	
	}

	public Order? GetOrder(long orderId)
	{
		var task = HttpCommon.SendRequest(HttpMethod.Get, $"{shopifyStore.BaseUrl}/admin/api/2023-10/orders/{orderId}.json", shopifyStore);
		task.Wait();
		var response = task.Result; 
		if (response.IsSuccessStatusCode)
		{
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