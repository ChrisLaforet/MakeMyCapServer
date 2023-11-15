using MakeMyCap.Model;

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

	public List<SanMarSkuMap> GetSanMarSkuMaps()
	{
		return context.SanMarSkuMaps.ToList();
	}

	public SanMarSkuMap? GetSanMarSkuMapFor(string sku)
	{
		return context.SanMarSkuMaps.FirstOrDefault(map => string.Compare(map.Sku, sku, true) == 0);
	}
}