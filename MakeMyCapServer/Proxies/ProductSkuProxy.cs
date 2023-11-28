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
		return context.Products.FirstOrDefault(product => product.Sku.ToUpper() == sku.ToUpper());
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
		return context.DistributorSkuMaps.Where(skuMap => skuMap.DistributorCode.ToUpper() == distributorCode.ToUpper()).ToList();
	}

	public DistributorSkuMap? GetSkuMapFor(string sku)
	{
		return context.DistributorSkuMaps.FirstOrDefault(map => map.Sku.ToUpper() == sku.ToUpper());
	}
}