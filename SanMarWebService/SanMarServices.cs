namespace SanMarWebService
{
	public class SanMarServices
	{
		private readonly string customerNumber;
		private readonly string userName;
		private readonly string password;

		public SanMarServices(string customerNumber, string userName, string password)
		{
			this.customerNumber = customerNumber;
			this.userName = userName;
			this.password = password;
		}

		public List<SanMarInventoryLevel> GetInventoryLevelsFor(string Style, string Color = null, string Size = null)
		{

		}
	}
}