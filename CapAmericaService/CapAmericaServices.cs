namespace CapAmericaInventory;

public class CapAmericaServices
{
	private readonly string userId;
	private readonly string password;

	public CapAmericaServices(string userId, string password)
	{
		this.userId = userId;
		this.password = password;
	}

	public async Task<List<CapAmericaInventoryLevel>> GetInventoryLevelsFor(string productId)
	{
		using (var client = new CapAmericaInventory.InventoryServiceClient())
		{
			var request = new GetInventoryLevelsRequest(userId, password, productId);
			var response = await client.getInventoryLevelsAsync(request);
//Console.WriteLine(response);
			if (response != null && response.GetInventoryLevelsResponse != null)
			{
				if (!CheckRecordsWereFound(response.GetInventoryLevelsResponse.ServiceMessageArray))
				{
					return ExtractInventoryLevelsFrom(response.GetInventoryLevelsResponse.Inventory);
				}
			}
		}
		
		return new List<CapAmericaInventoryLevel>();
	}

	private bool CheckRecordsWereFound(ServiceMessage[] messages)
	{
		if (messages == null)
		{
			return false;
		}
		foreach (var message in messages)
		{
			if (message.description != null && string.Compare(message.description, "No Record Found", true) == 0) {
				return false;
			}
		}

		return true;
	}
	
	private List<CapAmericaInventoryLevel> ExtractInventoryLevelsFrom(Inventory inventory)
	{
		var levels = new List<CapAmericaInventoryLevel>();
		             
		var productId = inventory.productId;
		Console.WriteLine("ProductId =" + productId);
		foreach (var partInventory in inventory.PartInventoryArray)
		{
			var level = new CapAmericaInventoryLevel();
			level.PartId = partInventory.partId;
			level.PartColor = partInventory.partColor;
			level.LabelSize = partInventory.labelSize.ToString();

			level.Quantity = ExtractQuantityFrom(partInventory.quantityAvailable);
			levels.Add(level);
		}

		return levels;
	}
	
	private int ExtractQuantityFrom(quantityAvailable available)
	{
// CML - do we need to check the UOM field for a valid type??		
//		if (available.Quantity.uom == "EA")
		var amount = available.Quantity.value;
		return Convert.ToInt32(amount);
	}
}