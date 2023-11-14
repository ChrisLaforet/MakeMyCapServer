﻿using System.Xml.Linq;

namespace MakeMyCapServer.Configuration;

public class XmlConfigurationLoader : IConfigurationLoader
{
	private readonly string pathToConfiguration;

	public XmlConfigurationLoader(string pathToConfiguration) => this.pathToConfiguration = pathToConfiguration;

	public string GetKeyValueFor(string elementName)
	{
		var fileElements = XElement.Load(pathToConfiguration);
		var element = fileElements.Element(elementName);
		if (element != null)
		{
			return element.Value;
		}
		throw new KeyNotFoundException();
	}
}