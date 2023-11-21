using MakeMyCapServer.Model;

namespace MakeMyCapServer.Proxies;

public class ProductSkuProxy : IProductSkuProxy
{
	private readonly MakeMyCapServerContext context;

	public ProductSkuProxy(MakeMyCapServerContext context)
	{
		this.context = context;
	}
	
	public List<Product> GetProductsByShopifyId(long shopifyId)
	{
		return context.Products.Where(product => product.ProductId == shopifyId).ToList();
	}

	public Product? GetProductBySku(string sku)
	{
		return context.Products.FirstOrDefault(product => string.Compare(product.Sku, sku, true) == 0);
	}

	public Product? GetProductByVariantId(long variantId)
	{
		return context.Products.FirstOrDefault(product => product.VariantId == variantId);
	}

	public void AddProduct(Product product)
	{
		context.Products.Add(product);
		context.SaveChanges();
	}

	public List<DistributorSkuMap> GetSkuMaps()
	{
		return context.DistributorSkuMaps.ToList();
	}

	public List<DistributorSkuMap> GetSkuMapsFor(string distributorCode)
	{
		return context.DistributorSkuMaps.Where(skuMap => string.Compare(skuMap.DistributorCode, distributorCode, true) == 0).ToList();
	}

	public DistributorSkuMap? GetSkuMapFor(string sku)
	{
		return context.DistributorSkuMaps.FirstOrDefault(map => string.Compare(map.Sku, sku, true) == 0);
		throw new NotImplementedException();
	}
}