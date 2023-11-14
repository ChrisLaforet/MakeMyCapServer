using MakeMyCap.Model;

namespace MakeMyCapServer.Proxies;

public interface IProductSkuProxy
{
	List<Product> GetProductsByShopifyId(long shopifyId);
	Product? GetProductBySku(string sku);
	Product? GetProductByVariantId(long variantId);

	void AddProduct(Product product);
}