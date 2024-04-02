using System.Text;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;
using MakeMyCapServer.Shopify.Dtos.Fulfillment;

namespace MakeMyCapServer.Distributors.PurchaseOrder;

public class PurchaseOrderCreator : IOrderGenerator
{
	private readonly IServiceProxy serviceProxy;
	private readonly IOrderingProxy orderingProxy;
	private readonly INotificationProxy notificationProxy;
	private readonly ILogger<PurchaseOrderCreator> logger;

	private static Mutex mutex = new Mutex();
	private int nextPoNumberSequence;
	
	public PurchaseOrderCreator(IServiceProxy serviceProxy, 
								IOrderingProxy orderingProxy, 
								INotificationProxy notificationProxy,
								ILogger<PurchaseOrderCreator> logger)
	{
		this.serviceProxy = serviceProxy; 
		this.orderingProxy = orderingProxy;
		this.notificationProxy = notificationProxy;
		this.logger = logger;
		
		ValidateNextPoNumberSequence(this.serviceProxy, this.orderingProxy);
	}

	private void ValidateNextPoNumberSequence(IServiceProxy serviceProxy, IOrderingProxy orderingProxy)
	{
		nextPoNumberSequence = serviceProxy.GetNextPoNumberSequence();
		var lastPoNumberSequence = orderingProxy.GetMaxPoNumberSequence();
		if (lastPoNumberSequence > nextPoNumberSequence)
		{
			nextPoNumberSequence = lastPoNumberSequence + 1;
			serviceProxy.UpdateNextPoNumberSequence(nextPoNumberSequence);
		}
	}

	public int GetNextPOSequence()
	{
		mutex.WaitOne();
		serviceProxy.UpdateNextPoNumberSequence(++nextPoNumberSequence);
		var poNumberSequence = nextPoNumberSequence;
		mutex.ReleaseMutex();

		return poNumberSequence;
	}
	
	public Model.PurchaseDistributorOrder? GenerateOrderFor(string distributorCode, DistributorSkuMap? skuMap, long shopifyOrderId, 
															int quantity, int poSequence, string name, string correlation, string imageOrText, 
															string position, string specialInstructions, string? shopifyName,
															int? distributorPo)
	{
		var distributor = orderingProxy.GetDistributorByCode(distributorCode);
		if (distributor == null)
		{
			logger.LogError($"Cannot locate a distributor with code {distributorCode} for generating an order on Shopify Order Id {shopifyOrderId}!");
			TransmitErrorMessage(distributorCode, shopifyOrderId, quantity);
			return null;
		}
		
		var poNumber = GeneratePONumber(poSequence);
		
		var purchaseOrder = new Model.PurchaseDistributorOrder();
		purchaseOrder.DistributorId = distributor.Id;
		purchaseOrder.Ponumber = poNumber;
		purchaseOrder.PoNumberSequence = poSequence;
		purchaseOrder.CreateDate = DateTime.Now;
		purchaseOrder.ShopifyOrderId = shopifyOrderId;
		purchaseOrder.Sku = skuMap == null || string.IsNullOrEmpty(skuMap.DistributorSku) ? "" : skuMap.DistributorSku;
		purchaseOrder.Quantity = quantity;
		purchaseOrder.Style = skuMap != null ? skuMap.StyleCode : "";
		purchaseOrder.Color = skuMap != null ? skuMap.Color : "";
		purchaseOrder.Size = skuMap != null ? skuMap.SizeCode : "";
		purchaseOrder.Name = name;
		purchaseOrder.Correlation = correlation;
		purchaseOrder.ImageOrText = imageOrText;
		purchaseOrder.Position = position;
		purchaseOrder.SpecialInstructions = specialInstructions;
		purchaseOrder.ShopifyName = shopifyName;
		if (skuMap != null && distributorPo != null)
		{
			purchaseOrder.Supplier = skuMap.DistributorCode;
			purchaseOrder.SupplierPoNumber = GeneratePONumber((int)distributorPo);
		}
		purchaseOrder.SubmittedDateTime = DateTime.Now;
		
		orderingProxy.SavePurchaseOrder(purchaseOrder);
		return purchaseOrder;
	}

	private string GeneratePONumber(int sequence)
	{
		return $"MMC{sequence.ToString("D9")}";
	}
	
	private void TransmitErrorMessage(string distributorCode, long shopifyOrderId, int quantity)
	{
		try
		{
			var subject = $"ERROR: Cannot find distributor - manual order is needed";
			
			var body = new StringBuilder();
			body.Append($"ERROR: CANNOT FIND DISTRIBUTOR CODE {distributorCode} IN SHOPIFY ORDER {shopifyOrderId}!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"This order requires an order of {quantity} of SKU {distributorCode}.\r\n");
			body.Append("\r\n");
			body.Append("The service cannot order this product.  It requires human intervention to send the order.\r\n\r\n");

			logger.LogInformation($"Transmitting ERROR message that cannot find distributor code {distributorCode} in Shopify Order {shopifyOrderId}.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that distributor code {distributorCode} cannot be found in Shopify Order {shopifyOrderId}: {ex}");
		}
	}
}