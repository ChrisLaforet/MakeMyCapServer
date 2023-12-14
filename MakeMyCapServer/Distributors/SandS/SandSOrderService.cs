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
	public const string PAYMENT_PROFILE_ID = "SandSPaymentProfileId";
	public const string PAYMENT_PROFILE_EMAIL = "SandSPaymentProfileEmail";
	
	public const string ORDER_ENDPOINT = "https://api.ssactivewear.com/v2/orders/";

	public const bool IS_TEST_MODE = true;
	
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

	public bool PlaceOrder(DistributorOrders orders)
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
				return true;
			}
			jsonBodyContent = JsonSerializer.Serialize(orderDto);
		}
		catch (Exception ex)
		{
			logger.LogError($"Error preparing payload: {ex}");
			return false;
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
			return false;
		}

		return true;
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
		
		var lookup = productSkuProxy.GetSkuMapsFor(SandSInventoryService.S_AND_S_DISTRIBUTOR_CODE);
		var notFoundSkus = new List<IDistributorOrder>();
		foreach (var order in orders.PurchaseOrders)
		{
			var map = lookup.SingleOrDefault(map => string.Compare(map.Sku, order.Sku, true) == 0);
			if (map == null)
			{
				logger.LogError($"Unable to map sku {order.Sku} to place order for S&S!");
				notFoundSkus.Add(order);
				continue;
			}
			
			orderDto.Lines.Add(new Line() { Identifier = map.DistributorSku, Qty = order.Quantity });
		}

		if (notFoundSkus.Count > 0)
		{
			NotifyOfMissingSkuMatches(notFoundSkus);
		}

		return orderDto;
	}
	
	private void NotifyOfMissingSkuMatches(List<IDistributorOrder> notFoundSkus)
	{
		var subject = $"Urgent! Order SKUs for S&S cannot be found for sending PO {notFoundSkus[0].PoNumber}";

		var body = new StringBuilder();
		body.Append($"The following Line Items for PO {notFoundSkus[0].PoNumber} cannot be found in our mappings.\r\n");
		body.Append($"PLEASE ORDER THESE ITEMS MANUALLY NOW.  Once done, the SKU(s) need to be mapped in our mapping table.\r\n\r\n");
		foreach (var notFoundSku in notFoundSkus)
		{
			body.Append($"   Our SKU: {notFoundSku.Sku}\r\n  Style: {notFoundSku.Style}\r\n  Color: {notFoundSku.Color}\r\n  Size: {notFoundSku.Size}\r\n  Quantity: {notFoundSku.Quantity}\r\n");
		}

		body.Append("\r\n");
		body.Append("Any other items on this PO that were located will be ordered electronically.");
		body.Append("\r\n\r\n");

		logger.LogInformation($"Transmitting critical message concerning inability to map all SKUs in PO {notFoundSkus[0].PoNumber}.");
		notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
	}
}