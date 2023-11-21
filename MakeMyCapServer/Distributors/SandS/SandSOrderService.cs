using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors.Exceptions;
using MakeMyCapServer.Distributors.SandS.Dtos;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.Distributors.SandS;

public class SandSOrderService : IOrderService
{
	public const string ORDER_ENDPOINT = "https://api.ssactivewear.com/v2/orders/";

	public const string SHIPPING_CODE = "14";
	// 14 = FedEx Ground
	// 27 = FedEx Next Day Standard
	// 26 = FedEx Next Day Priority
	// 48 = FedEx 2nd Day Air
	
	private readonly IConfigurationLoader configurationLoader;
	private readonly IOrderingProxy orderingProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<SandSOrderService> logger;
	
	public SandSOrderService(IConfigurationLoader configurationLoader, 
							IOrderingProxy orderingProxy, 
							INotificationProxy notificationProxy,
							ILogger<SandSOrderService> logger)
	{
		this.configurationLoader = configurationLoader;
		this.orderingProxy = orderingProxy;
		this.notificationProxy = notificationProxy;
		this.logger = logger;
	}

	private void PrepareHeadersFor(HttpClient client)
	{
		client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
		client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json; charset=utf-8");
	}

	public bool PlaceOrder(IOrder order)
	{
		var accountNumber = configurationLoader.GetKeyValueFor(SandSInventoryService.ACCOUNT_NUMBER);
		var apiKey = configurationLoader.GetKeyValueFor(SandSInventoryService.API_KEY);
		
		var client = new HttpClient();
		PrepareHeadersFor(client);
		var authenticationValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountNumber}:{apiKey}"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationValue);

		try
		{
			var body = PrepareOrderBodyFrom(order);
		}
		catch (Exception ex)
		{
			logger.LogError($"Error preparing payload: {ex}");
			return false;
		}

		var request = new HttpRequestMessage(HttpMethod.Post, ORDER_ENDPOINT);
		var jsonBodyContent = PrepareOrderBodyFrom(order);
		request.Content = new StringContent(jsonBodyContent,
			Encoding.UTF8, 
			"application/json");
			
		var task = client.SendAsync(request);
		task.Wait();
		var response = task.Result; 
		if (!response.IsSuccessStatusCode)
		{
			logger.LogError($"Error in S&S Order: {(int)response.StatusCode} ({response.ReasonPhrase})");
			return false;
		}

		return true;
	}

	public string PrepareOrderBodyFrom(IOrder order)
	{
		var shipping = orderingProxy.GetShipping();
		if (shipping == null)
		{
			logger.LogError("There is no Ship-To addressing in the database!");
			throw new MissingDataException("There is no Ship-To addressing in the database!");
		}

		var shippingAddress = new ShippingAddress();
		shippingAddress.Customer = shipping.Name;
		shippingAddress.Address = shipping.ShipAddress;
		shippingAddress.City = shipping.ShipCity;
		shippingAddress.State = shipping.ShipState;
		shippingAddress.Zip = shipping.ShipZip;
		shippingAddress.Attn = $"{shipping.ShipTo}  PO # {order.PoNumber}".Trim();

		var orderDto = new Order();
		orderDto.ShippingMethod = SHIPPING_CODE;
		orderDto.PoNumber = order.PoNumber;
		orderDto.ShippingAddress = shippingAddress;
		foreach (var lineItem in order.LineItems)
		{
			orderDto.Lines.Add(new Line() { Identifier = lineItem.Sku, Qty = lineItem.Quantity });
		}

		return JsonSerializer.Serialize(orderDto);
	}
}