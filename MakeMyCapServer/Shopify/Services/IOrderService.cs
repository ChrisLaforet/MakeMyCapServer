using MakeMyCapServer.Shopify.Dtos.Fulfillment;

namespace MakeMyCapServer.Shopify.Services;

public interface IOrderService
{
	List<Order> GetOpenOrders();
	Order? GetOrder(long orderId);
}