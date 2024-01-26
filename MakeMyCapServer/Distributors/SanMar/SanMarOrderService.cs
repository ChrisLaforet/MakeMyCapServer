using System.Text;
using MakeMyCapServer.Configuration;
using MakeMyCapServer.Distributors.Exceptions;
using MakeMyCapServer.Model;
using MakeMyCapServer.Orders;
using MakeMyCapServer.Proxies;
using SanMarOrderWebService;

namespace MakeMyCapServer.Distributors.SanMar;

public class SanMarOrderService : IOrderService
{
	public const string CUSTOMER_NUMBER = "SanMarOrderCustomerNumber";
	public const string USER_NAME = "SanMarOrderUserName";
	public const string PASSWORD = "SanMarOrderPassword";

	private const char COMMA = ',';
	private const string OSFA = "OSFA";
	private const string SHIPPING_CODE = "UPS";
	
	private readonly SanMarOrderServices services;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly IOrderingProxy orderingProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<SanMarOrderService> logger;
	
	public SanMarOrderService(IConfigurationLoader configurationLoader, 
							IOrderingProxy orderingProxy, 
							IProductSkuProxy productSkuProxy,
							INotificationProxy notificationProxy,
							ILogger<SanMarOrderService> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.orderingProxy = orderingProxy;
		this.notificationProxy = notificationProxy;
		this.logger = logger;
		
		var customerNumber = configurationLoader.GetKeyValueFor(CUSTOMER_NUMBER);
		var userName = configurationLoader.GetKeyValueFor(USER_NAME);
		var password = configurationLoader.GetKeyValueFor(PASSWORD);
		services = new SanMarOrderServices(customerNumber, userName, password);
	}
	
	public bool PlaceOrder(DistributorOrders orders)
	{
		var shipping = orderingProxy.GetShipping();
		if (shipping == null)
		{
			throw new MissingDataException("Cannot order from SanMar - Shipping information is not configured.");
		}
		
		var request = PrepareOrderRequest(orders, shipping);
		if (request.Details.Count == 0) 
		{
			logger.LogError($"No line items remain in PO {orders.PoNumber} so there is nothing to transmit to SanMar.");
			return true;
		}

		var task = services.SubmitPurchaseOrder(request);
		task.Wait();
		var response = task.Result;

		if (!response.Success)
		{
			logger.LogInformation($"Error transmitting order for PO {orders.PoNumber} with reason: {response.Message}");
		}
		return response.Success;
	}

	private SanMarOrderRequest PrepareOrderRequest(DistributorOrders orders, Shipping shipping)
	{
		var request = new SanMarOrderRequest();
		request.Attention = SanitizeString($"PO # ({orders.PoNumber})");
		request.PoNumber = SanitizeString(orders.PoNumber);
		request.ShipMethod = SanitizeString(SHIPPING_CODE);
		request.ShipTo = SanitizeString(shipping.ShipTo);
		request.Address1 = SanitizeString(shipping.ShipAddress);
		request.City = SanitizeString(shipping.ShipCity);
		request.State = SanitizeString(shipping.ShipState);
		request.Zip = SanitizeString(shipping.ShipZip);
		if (!string.IsNullOrEmpty(shipping.ShipEmail))
		{
			request.Email = SanitizeString(shipping.ShipEmail);
		}

		var lookup = productSkuProxy.GetSkuMapsFor(SanMarInventoryService.SANMAR_DISTRIBUTOR_CODE);

		var details = new List<SanMarOrderDetail>();
		var notFoundSkus = new List<IDistributorOrder>();
		foreach (var order in orders.PurchaseOrders)
		{
			var map = lookup.SingleOrDefault(map => string.Compare(map.Sku, order.Sku, true) == 0);
			if (map == null)
			{
				logger.LogError($"Unable to map sku {order.Sku} to place order for SanMar!");
				notFoundSkus.Add(order);
				continue;
			}

			var detail = new SanMarOrderDetail();
			detail.Quantity = order.Quantity;
			detail.Style = map.StyleCode;
			if (!string.IsNullOrEmpty(map.Color))
			{
				detail.Color = map.Color;
			}

			if (!string.IsNullOrEmpty(map.SizeCode))
			{
				detail.Size = map.SizeCode;
			}
			else
			{
				detail.Size = OSFA;
			}

			details.Add(detail);
		}
		request.Details = details;
		
		if (notFoundSkus.Count > 0)
		{
			NotifyOfMissingSkuMatches(notFoundSkus);
		}
		
		return request;
	}

	// handy code to show what a webservice payload will look like
	// var serxml = new System.Xml.Serialization.XmlSerializer(request.GetType());
	// var ms = new MemoryStream();
	// serxml.Serialize(ms, request);
	// string xml = Encoding.UTF8.GetString(ms.ToArray());

	
	private void NotifyOfMissingSkuMatches(List<IDistributorOrder> notFoundSkus)
	{
		var subject = $"Urgent! Order SKUs for SanMar cannot be found for sending PO {notFoundSkus[0].PoNumber}";

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

	private static string SanitizeString(string? value)
	{
		// SanMar states: Please Note: Do Not Use Additional Commas in any Field Due to the Comma being our Delimiter in order files.
		if (string.IsNullOrEmpty(value))
		{
			return "";
		}

		if (value.Contains(COMMA))
		{
			var sanitized = new StringBuilder();
			foreach (char ch in value)
			{
				if (ch != COMMA)
				{
					sanitized.Append(value);
				}
				else
				{
					sanitized.Append(' ');
				}
			}
			return sanitized.ToString().Trim();
		}

		return value.Trim();
	}
}