using MakeMyCapServer.Shopify.Dtos.Inventory;

namespace MakeMyCapServer.Shopify.Services;

public interface IInventoryService
{
	InventoryItem? GetInventoryItem(long id);
	List<InventoryLevel> GetInventoryLevels(List<long> inventoryItemIds = null);
	List<Product> GetProducts(long? sinceId = null);
	InventoryLevel? AdjustInventoryLevel(long inventoryItemId, long locationId, int adjustment);
}