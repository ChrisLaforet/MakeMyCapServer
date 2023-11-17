using MakeMyCapServer.Configuration;
using MakeMyCapServer.Configuration.Exceptions;

namespace MakeMyCapServer.Shopify;

public class ShopifyStore : IShopifyStore
{
	public const string API_TOKEN = "ShopifyApiToken";
	public const string STORE_NAME = "ShopifyStoreName";
	public const string LOCATION = "ShopifyLocation";
	
	public ShopifyStore(IConfigurationLoader configurationLoader, ILogger<ShopifyStore> logger)
	{

		try
		{
			ApiToken = configurationLoader.GetKeyValueFor(API_TOKEN);
			StoreName = configurationLoader.GetKeyValueFor(STORE_NAME);
			Location = configurationLoader.GetKeyValueFor(LOCATION);
		}
		catch (ConfigurationKeyNotFoundException ex)
		{
			logger.LogCritical($"Unable to find a configured value for Shopify {ex}");
		}
	}

	public string StoreName { get; private set; }
	public string DomainName => "myshopify.com";
	public string BaseUrl => $"https://{StoreName}.{DomainName}";
	public string Location { get; private set; }
	public string ApiToken { get; private set; }
}