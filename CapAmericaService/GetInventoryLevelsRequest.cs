namespace CapAmericaInventory;

public partial class GetInventoryLevelsRequest
{
	public GetInventoryLevelsRequest() { }

	public GetInventoryLevelsRequest(string id, string password, string productId) : base()
	{
		this.id = id;
		this.password = password;
		this.productId = productId;
	}
}