namespace ShopifyInventoryFulfillment.Configuration;

public interface IConfigurationLoader
{
	string GetKeyValueFor(string elementName);
}