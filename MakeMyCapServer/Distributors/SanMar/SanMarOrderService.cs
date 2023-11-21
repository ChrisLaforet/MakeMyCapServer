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
	private const char COMMA = ',';
	private const string SHIPPING_CODE = "FEDEX GROUND";
	
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
		
		var customerNumber = configurationLoader.GetKeyValueFor(SanMarInventoryService.CUSTOMER_NUMBER);
		var userName = configurationLoader.GetKeyValueFor(SanMarInventoryService.USER_NAME);
		var password = configurationLoader.GetKeyValueFor(SanMarInventoryService.PASSWORD);
		services = new SanMarOrderServices(customerNumber, userName, password);
	}
	
	public bool PlaceOrder(IOrder order)
	{
		var shipping = orderingProxy.GetShipping();
		if (shipping == null)
		{
			throw new MissingDataException("Cannot order from SanMar - Shipping information is not configured.");
		}
		
		var request = PrepareOrderRequest(order, shipping);
		if (request.Details.Count == 0) 
		{
			logger.LogError($"No line items remain in PO {order.PoNumber} so there is nothing to transmit to SanMar.");
			return true;
		}

		var task = services.SubmitPurchaseOrder(request);
		task.Wait();
		var response = task.Result;

		if (!response.Success)
		{
			logger.LogInformation($"Error transmitting order for PO {order.PoNumber} with reason: {response.Message}");
		}
		return response.Success;
	}

	private SanMarOrderRequest PrepareOrderRequest(IOrder order, Shipping shipping)
	{
		var request = new SanMarOrderRequest();
		request.Attention = SanitizeString($"PO # ({order.PoNumber})");
		request.PoNumber = SanitizeString(order.PoNumber);
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
		var notFoundSkus = new List<IOrderItem>();
		foreach (var lineItem in order.LineItems)
		{
			var map = lookup.SingleOrDefault(map => string.Compare(map.Sku, lineItem.Sku, true) == 0);
			if (map == null)
			{
				logger.LogError($"Unable to map sku {lineItem.Sku} to place order for SanMar!");
				notFoundSkus.Add(lineItem);
				continue;
			}

			var detail = new SanMarOrderDetail();
			detail.Quantity = lineItem.Quantity;
			detail.Style = map.StyleCode;
			if (!string.IsNullOrEmpty(map.Color))
			{
				detail.Color = map.Color;
			}

			if (!string.IsNullOrEmpty(map.SizeCode))
			{
				detail.Size = map.SizeCode;
			}

			details.Add(detail);
		}
		request.Details = details;
		
		if (notFoundSkus.Count > 0)
		{
			NotifyOfMissingSkuMatches(order, notFoundSkus);
		}

		return request;
	}

	private void NotifyOfMissingSkuMatches(IOrder order, List<IOrderItem> notFoundSkus)
	{
		var subject = $"Urgent! Order SKUs for SanMar cannot be found for sending PO {order.PoNumber}";

		var body = new StringBuilder();
		body.Append($"The following Line Items for PO {order.PoNumber} cannot be found in our mappings.\r\n");
		body.Append($"PLEASE ORDER THESE ITEMS MANUALLY NOW.  Once done, the SKU(s) need to be mapped in our mapping table.\r\n\r\n");
		foreach (var notFoundSku in notFoundSkus)
		{
			body.Append($"   Our SKU: {notFoundSku.Sku}  Style: {notFoundSku.Style}  Color: {notFoundSku.Color}  Size: {notFoundSku.Size}  Quantity: {notFoundSku.Quantity}\r\n");
		}

		body.Append("\r\n");
		body.Append("Any other items on this PO that were located will be ordered electronically.");
		body.Append("\r\n\r\n");

		logger.LogInformation($"Transmitting critical message concerning inability to map all SKUs in PO {order.PoNumber}.");
		notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
	}

	private static string SanitizeString(string value)
	{
		// SanMar states: Please Note: Do Not Use Additional Commas in any Field Due to the Comma being our Delimiter in order files.
		if (string.IsNullOrEmpty(value))
		{
			return value;
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