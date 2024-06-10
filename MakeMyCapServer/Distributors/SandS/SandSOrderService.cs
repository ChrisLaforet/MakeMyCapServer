using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors.Exceptions;
using MakeMyCapServer.Distributors.SandS.Dtos;
using MakeMyCapServer.Model;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Services.OrderPlacement;
using Order = MakeMyCapServer.Distributors.SandS.Dtos.Order;

namespace MakeMyCapServer.Distributors.SandS;

public class SandSOrderService : IOrderService
{
	public const string PAYMENT_PROFILE_ID = "SandSPaymentProfileId";
	public const string PAYMENT_PROFILE_EMAIL = "SandSPaymentProfileEmail";
	
	public const string ORDER_ENDPOINT = "https://api.ssactivewear.com/v2/orders/";

	public const bool IS_TEST_MODE = false;
	
	public const string SHIPPING_CODE = "1";
	// 1 = S&S decides - preferred by CapAmerica
	// 14 = FedEx Ground
	// 27 = FedEx Next Day Standard
	// 26 = FedEx Next Day Priority
	// 48 = FedEx 2nd Day Air
	
	private readonly IConfigurationLoader configurationLoader;
	private readonly IOrderingProxy orderingProxy;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<SandSOrderService> logger;
	
	public SandSOrderService(IConfigurationLoader configurationLoader, 
							IOrderingProxy orderingProxy,
							IProductSkuProxy productSkuProxy,
							INotificationProxy notificationProxy,
							ILogger<SandSOrderService> logger)
	{
		this.configurationLoader = configurationLoader;
		this.orderingProxy = orderingProxy;
		this.productSkuProxy = productSkuProxy;
		this.notificationProxy = notificationProxy;
		this.logger = logger;
	}

	private void PrepareHeadersFor(HttpClient client)
	{
		client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json; charset=utf-8");
		client.DefaultRequestHeaders.TryAddWithoutValidation("Accept", "application/json; charset=utf-8");
	}

	public OrderStatus PlaceOrder(DistributorOrders orders)
	{
		var accountNumber = configurationLoader.GetKeyValueFor(SandSInventoryService.ACCOUNT_NUMBER);
		var apiKey = configurationLoader.GetKeyValueFor(SandSInventoryService.API_KEY);

		if (IS_TEST_MODE)
		{
			logger.LogCritical("S&S is running in a TEST mode");
		}

		var client = new HttpClient();
		PrepareHeadersFor(client);
		var authenticationValue = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{accountNumber}:{apiKey}"));
		client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authenticationValue);

		string jsonBodyContent = null;
		try
		{
			var orderDto = PrepareOrderFrom(orders);
			if (orderDto.Lines.Count == 0)
			{
				logger.LogError($"No line items remain in PO {orders.PoNumber} so there is nothing to transmit to S&S.");
				return new OrderStatus(true);
			}
			jsonBodyContent = JsonSerializer.Serialize(orderDto);
		}
		catch (Exception ex)
		{
			logger.LogError($"Error preparing payload: {ex}");
			return new OrderStatus(false);
		}

		var request = new HttpRequestMessage(HttpMethod.Post, ORDER_ENDPOINT);
		request.Content = new StringContent(jsonBodyContent,
			Encoding.UTF8, 
			"application/json");
			
		var task = client.SendAsync(request);
		task.Wait();
		var response = task.Result; 
		if (!response.IsSuccessStatusCode)
		{
			logger.LogError($"Error in S&S Order: {(int)response.StatusCode} ({response.ReasonPhrase})");
			var outOfStockItems = DetermineOutOfStockItems(response);

			return new OrderStatus(false, outOfStockItems);
		}

		return new OrderStatus(true);
	}

	private List<IOutOfStockItem> DetermineOutOfStockItems(HttpResponseMessage response)
	{
		var outOfStockItems = new List<IOutOfStockItem>();
		List<DistributorSkuMap> lookup = null;
		try
		{
			var contents = GetResponseContents(response);
			using (JsonDocument doc = JsonDocument.Parse(contents))
			{
				var root = doc.RootElement;
				var errors = root.GetProperty("errors");
				for (var index = 0; index < errors.GetArrayLength(); index++)
				{
					var element = errors[index];
//					var field = element.GetProperty("field");
					var message = element.GetProperty("message");
					if (message.ToString().ToUpper().Contains(" OUT OF STOCK "))
					{
						var item = OutOfStockItem.ParseOutOfStockError(message.ToString().Trim());
						if (item != null)
						{
							if (lookup == null)
							{
								lookup = productSkuProxy.GetSkuMapsFor(SandSInventoryService.S_AND_S_DISTRIBUTOR_CODE);
							}
							// local mapping to system-level SKU
							var match = lookup.SingleOrDefault(map => string.Compare(item.VendorSku, map.DistributorSku, true) == 0);
							if (match != null)
							{
								item.DistributorSkuMap = match;
							}
							outOfStockItems.Add(item);
						}
					}
				}
			}
		}
		catch (Exception ex)
		{
			logger.LogError($"Caught error while attempting to extract Out Of Stock from S&S: {ex}");
		}

		return outOfStockItems;
	}

	private static string GetResponseContents(HttpResponseMessage response)
	{
		var task = response.Content.ReadAsStringAsync();
		task.Wait();
		return task.Result;
	}
		
	public Order PrepareOrderFrom(DistributorOrders orders)
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
		shippingAddress.Attn = $"{shipping.ShipTo}  PO # {orders.PoNumber}".Trim();

		var profileEmail = configurationLoader.GetKeyValueFor(PAYMENT_PROFILE_EMAIL);
		var profileId = long.Parse(configurationLoader.GetKeyValueFor(PAYMENT_PROFILE_ID));

		var paymentProfile = new PaymentProfile();
		paymentProfile.Email = profileEmail;
		paymentProfile.ProfileId = profileId;
		
		var orderDto = new Order();
		orderDto.ShippingMethod = SHIPPING_CODE;
		orderDto.PoNumber = orders.PoNumber;
		orderDto.ShippingAddress = shippingAddress;
		orderDto.PaymentProfile = paymentProfile;

		orderDto.TestOrder = IS_TEST_MODE;
		
		foreach (var order in orders.PurchaseOrders)
		{
			orderDto.Lines.Add(new Line() { Identifier = order.Sku, Qty = order.Quantity });
		}

		return orderDto;
	}
}