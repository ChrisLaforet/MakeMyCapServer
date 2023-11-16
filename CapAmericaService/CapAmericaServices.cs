namespace CapAmericaInventory;

public class CapAmericaServices
{
	private readonly string userName;
	private readonly string password;

	public CapAmericaServices(string userName, string password)
	{
		this.userName = userName;
		this.password = password;
	}
}