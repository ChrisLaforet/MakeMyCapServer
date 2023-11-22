using MakeMyCapServer.Model;

namespace MakeMyCapServer.Distributors;

public interface IOrderGenerator
{
	Model.PurchaseOrder? GenerateOrderFor(DistributorSkuMap skuMap, long shopifyOrderId,  int quantity);
}