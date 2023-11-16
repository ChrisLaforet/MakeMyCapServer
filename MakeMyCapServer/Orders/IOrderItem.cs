namespace MakeMyCapServer.Orders;

public interface IOrderItem
{
	string? Sku { get; }
	string? Style { get; }
	string? Color { get; }
	string? Size { get; }
	int Quantity { get; }
}