using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Inventory;

public class InventoryItem
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("sku")]
	public string Sku { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonPropertyName("updated_at")]
	public DateTime UpdatedAt { get; set; }

	[JsonPropertyName("requires_shipping")]
	public bool RequiresShipping { get; set; }

	[JsonPropertyName("cost")]
	public string Cost { get; set; }

	[JsonPropertyName("country_code_of_origin")]
	public object CountryCodeOfOrigin { get; set; }

	[JsonPropertyName("province_code_of_origin")]
	public object ProvinceCodeOfOrigin { get; set; }

	[JsonPropertyName("harmonized_system_code")]
	public object HarmonizedSystemCode { get; set; }

	[JsonPropertyName("tracked")]
	public bool Tracked { get; set; }

	[JsonPropertyName("country_harmonized_system_codes")]
	public List<object> CountryHarmonizedSystemCodes { get; set; }

	[JsonPropertyName("admin_graphql_api_id")]
	public string AdminGraphqlApiId { get; set; }

}