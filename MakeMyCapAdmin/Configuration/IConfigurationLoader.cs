namespace MakeMyCapAdmin.Configuration;

public interface IConfigurationLoader
{
	string GetKeyValueFor(string elementName);
}