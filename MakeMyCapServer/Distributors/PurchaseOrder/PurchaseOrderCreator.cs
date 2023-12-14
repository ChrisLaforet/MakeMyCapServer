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
	
	public Model.PurchaseDistributorOrder? GenerateOrderFor(DistributorSkuMap skuMap, long shopifyOrderId, int quantity, int poSequence)
	{
		var distributor = orderingProxy.GetDistributorByCode(skuMap.DistributorCode);
		if (distributor == null)
		{
			logger.LogError($"Cannot locate a distributor with code {skuMap.DistributorCode} for generating an order on Shopify Order Id {shopifyOrderId}!");
			TransmitErrorMessage(skuMap, shopifyOrderId, quantity);
			return null;
		}
		
		var poNumber = $"MMC{poSequence.ToString("D9")}";
		
		var purchaseOrder = new Model.PurchaseDistributorOrder();
		purchaseOrder.DistributorId = distributor.Id;
		purchaseOrder.Ponumber = poNumber;
		purchaseOrder.PoNumberSequence = poSequence;
		purchaseOrder.CreateDate = DateTime.Now;
		purchaseOrder.ShopifyOrderId = shopifyOrderId;
		purchaseOrder.Sku = skuMap.DistributorSku;
		purchaseOrder.Quantity = quantity;
		purchaseOrder.Style = skuMap.StyleCode;
		purchaseOrder.Color = skuMap.Color;
		purchaseOrder.Size = skuMap.SizeCode;
		purchaseOrder.SubmittedDateTime = DateTime.Now;
		
		orderingProxy.SavePurchaseOrder(purchaseOrder);
		return purchaseOrder;
	}
	
	private void TransmitErrorMessage(DistributorSkuMap skuMap, long shopifyOrderId, int quantity)
	{
		try
		{
			var subject = $"ERROR: Cannot find distributor - manual order is needed";
			
			var body = new StringBuilder();
			body.Append($"ERROR: CANNOT FIND DISTRIBUTOR CODE {skuMap.DistributorCode} TO ORDER {skuMap.Sku} IN SHOPIFY ORDER {shopifyOrderId}!  Manual intervention is needed!!\r\n\r\n");
			body.Append($"This order requires an order of {quantity} of SKU {skuMap.DistributorCode}.\r\n");
			body.Append("\r\n");
			body.Append("The service cannot order this product.  It requires human intervention to send the order.\r\n\r\n");

			logger.LogInformation($"Transmitting ERROR message that cannot find distributor code {skuMap.DistributorCode} in Shopify Order {shopifyOrderId}.");
			notificationProxy.SendCriticalErrorNotification(subject, body.ToString());
		}
		catch (Exception ex)
		{
			logger.LogCritical($"Error transmitting ERROR message that distributor code {skuMap.DistributorCode} cannot be found in Shopify Order {shopifyOrderId}: {ex}");
		}
	}
}