using MakeMyCapServer.Model;

namespace MakeMyCapServer.Distributors;

public interface IOrderGenerator
{
	int GetNextPOSequence();
	
	Model.PurchaseDistributorOrder? GenerateOrderFor(string distributorCode, DistributorSkuMap? skuMap, long shopifyOrderId,  int quantity, int poSequence,
		string name, string correlation, string imageOrText, string position, string specialInstructions, string? shopifyName, List<int> otherPoNumbers);
}