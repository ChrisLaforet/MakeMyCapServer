namespace MakeMyCapServer.Configuration;

public interface IConfigurationLoader
{
	string GetKeyValueFor(string elementName);
}