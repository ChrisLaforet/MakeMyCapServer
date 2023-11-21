using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class AmountSet
{
	[JsonPropertyName("shop_money")]
	public Money ShopMoney { get; set; }

	[JsonPropertyName("presentment_money")]
	public Money Money { get; set; }
}