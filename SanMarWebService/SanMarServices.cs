using System.Xml;

namespace SanMarWebService;

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

	public async Task<List<SanMarInventoryLevel>> GetInventoryLevelsFor(string style, string? color = null, string? size = null)
	{
		var inventoryLevels = new List<SanMarInventoryLevel>();
		
		using (var client = new SanMarInventory.SanMarWebServiceDelegateClient())
		{
			var response = await client.getInventoryQtyForStyleColorSizeAsync(customerNumber, userName, password, style, color, size);
			if (response.@return.errorOccurred)
			{
				throw new SanMarException($"Error while connecting to SanMar: {response.@return.message}");
			}
			
			var nodes = response.@return.response as XmlNode[];
			var listResponses = response.@return.listResponse;
			if (nodes != null)
			{
				inventoryLevels.AddRange(ExtractDetailedResponse(nodes));
			}
			else if (listResponses != null && color != null && size != null)
			{
				int quantity = SumListResponses(listResponses);
				inventoryLevels.Add(new SanMarInventoryLevel() { Style = style, Color = color, Size = size, Quantity = quantity });
			}			
		}

		return inventoryLevels;
	}

	private int SumListResponses(object[] responses)
	{
		int quantity = 0;
		foreach (object node in responses)
		{
			quantity += int.Parse(node.ToString().Trim());
		}
		
		return quantity;
	}

	private List<SanMarInventoryLevel> ExtractDetailedResponse(XmlNode[] nodes)
	{
		var inventoryLevels = new List<SanMarInventoryLevel>();

		if (nodes != null)
		{
			var style = "";
			foreach (XmlNode node in nodes)
			{
				if (node.Name == "style")
				{
					style = node.InnerText.Trim();
				}
				else if (node.Name == "skus")
				{
					inventoryLevels.AddRange(ExtractSkus(node.ChildNodes as XmlNodeList, style));
				}
			}
		}

		return inventoryLevels;
	}

	private List<SanMarInventoryLevel> ExtractSkus(XmlNodeList children, string style)
	{
		var inventoryLevels = new List<SanMarInventoryLevel>();

		if (children != null)
		{
			foreach (XmlNode child in children)
			{
				var elements = child.ChildNodes as XmlNodeList;

				var color = "";
				var size = "";
				int quantity = 0;

				foreach (XmlNode element in elements)
				{
					switch (element.Name)
					{
						case "color":
							color = element.InnerText.Trim();
							break;

						case "size":
							size = element.InnerText.Trim();
							break;

						case "whse":
							quantity = SumQuantities(child);
							break;
					}
				}
				inventoryLevels.Add(new SanMarInventoryLevel() { Style = style, Color = color, Size = size, Quantity = quantity });
			}
		}
		
		return inventoryLevels;
	}


	private int SumQuantities(XmlNode node)
	{
		int quantity = 0;
		var children = node.ChildNodes as XmlNodeList;
		if (children != null)
		{
			foreach (XmlNode child in children)
			{
				if (child.Name == "whse")
				{
					var elements = child.ChildNodes as XmlNodeList;

					foreach (XmlNode element in elements)
					{
						if (element.Name == "qty")
						{
							quantity += int.Parse(element.InnerText.Trim());
						}
					}
				}
			}
		}
		
		return quantity;
	}
}