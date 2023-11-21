namespace SanMarOrderWebService;

public class SanMarOrderRequest
{
	public string? Attention { get; set; }
	public string PoNumber { get; set; }
	public string ShipTo { get; set; }
	public string Address1 { get; set; }
	public string? Address2 { get; set; }
	public string City { get; set; }
	public string State { get; set; }
	public string Zip { get; set; }
	public string Email { get; set; }
	public string ShipMethod { get; set; }

	public List<SanMarOrderDetail> Details { get; set; }
}

public class SanMarOrderDetail 
{
	public int Quantity { get; set; }
	public string Style { get; set; }
	public string? Size { get; set; }
	public string? Color { get; set; }
}