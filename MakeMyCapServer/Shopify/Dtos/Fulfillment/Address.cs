using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Address
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("customer_id")]
	public int CustomerId { get; set; }

	[JsonPropertyName("first_name")]
	public string FirstName { get; set; }

	[JsonPropertyName("last_name")]
	public string LastName { get; set; }

	[JsonPropertyName("company")]
	public string Company { get; set; }

	[JsonPropertyName("address1")]
	public string Address1 { get; set; }

	[JsonPropertyName("address2")]
	public string Address2 { get; set; }

	[JsonPropertyName("city")]
	public string City { get; set; }

	[JsonPropertyName("province")]
	public string Province { get; set; }

	[JsonPropertyName("country")]
	public string Country { get; set; }

	[JsonPropertyName("zip")]
	public string Zip { get; set; }

	[JsonPropertyName("phone")]
	public string Phone { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("province_code")]
	public string ProvinceCode { get; set; }

	[JsonPropertyName("country_code")]
	public string CountryCode { get; set; }

	[JsonPropertyName("country_name")]
	public string CountryName { get; set; }

	[JsonPropertyName("default")]
	public bool Default { get; set; }
}