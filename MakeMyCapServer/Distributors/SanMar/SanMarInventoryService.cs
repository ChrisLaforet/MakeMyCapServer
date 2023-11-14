using MakeMyCapServer.Configuration;
using SanMarWebService;

namespace MakeMyCapServer.Distributors.SanMar;

public class SanMarInventoryService : IInventoryService
{
	public const string CUSTOMER_NUMBER = "SanMarCustomerNumber";
	public const string USER_NAME = "SanMarUserName";
	public const string PASSWORD = "SanMarPassword";

	private readonly IConfigurationLoader configurationLoader;
	private readonly ILogger<SanMarInventoryService> logger;
	
	private readonly SanMarServices services;
	
	public SandSInventoryService( IConfigurationLoader configurationLoader, ILogger<SanMarInventoryService> logger)
	{
		this.configurationLoader = configurationLoader;
		this.logger = logger;
		
		var customerNumber = configurationLoader.GetKeyValueFor(CUSTOMER_NUMBER);
		var userName = configurationLoader.GetKeyValueFor(USER_NAME);
		var password = configurationLoader.GetKeyValueFor(PASSWORD);
		services = new SanMarServices(customerNumber, userName, password);
	}

	
	public List<InStockInventory> GetInStockInventoryFor(List<string> skus)
	{
		// 1. map skus
		
		// 2. call and get responses
		
		throw new NotImplementedException();
	}
}