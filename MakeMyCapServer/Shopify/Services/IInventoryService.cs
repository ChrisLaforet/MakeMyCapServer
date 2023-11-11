using ShopifyInventoryFulfillment.Shopify.Dtos;

namespace ShopifyInventoryFulfillment.Shopify;

public interface IInventoryService
{
	InventoryItem? GetInventoryItem(long id);
	List<InventoryLevel> GetInventoryLevels();
}