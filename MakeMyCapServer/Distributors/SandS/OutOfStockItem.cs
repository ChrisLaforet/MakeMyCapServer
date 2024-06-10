using System.Text.RegularExpressions;
using MakeMyCapServer.Model;
using MakeMyCapServer.Services.OrderPlacement;

namespace MakeMyCapServer.Distributors.SandS;

internal class OutOfStockItem : IOutOfStockItem
{
	public DistributorSkuMap? DistributorSkuMap { get; set; } = null;
	public string VendorSku { get; private set; }
	public string Description { get; private set; }
	public int OrderedQuantity { get; private set; }
	public int AvailableQuantity { get; private set; }

	private OutOfStockItem(string vendorSku, string description, int orderedQuantity, int availableQuantity)
	{
		VendorSku = vendorSku;
		Description = description;
		OrderedQuantity = orderedQuantity;
		AvailableQuantity = availableQuantity;
	}

	public static OutOfStockItem? ParseOutOfStockError(string message)
	{
		var pattern = "^Identifier ([A-Za-z0-9]*) \\(([^\\)]*?)\\) - Out Of Stock \\(Ordered: ([0-9]*).*? Available: ([0-9]*)\\)$";
		var match = Regex.Match(message, pattern);
		if (match.Success)
		{
			return new OutOfStockItem(match.Groups[1].Value, match.Groups[2].Value, int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
		}

		return null;
	}
}