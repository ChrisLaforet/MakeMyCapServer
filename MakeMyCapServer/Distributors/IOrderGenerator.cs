using MakeMyCapServer.Model;

namespace MakeMyCapServer.Distributors;

public interface IOrderGenerator
{
	int GetNextPOSequence();
	
	Model.PurchaseDistributorOrder? GenerateOrderFor(DistributorSkuMap skuMap, long shopifyOrderId,  int quantity, int poSequence);
}