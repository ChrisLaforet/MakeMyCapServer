using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Fulfillment
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("admin_graphql_api_id")]
	public string AdminGraphqlApiId { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonPropertyName("location_id")]
	public int LocationId { get; set; }

	[JsonPropertyName("name")]
	public string Name { get; set; }

	[JsonPropertyName("order_id")]
	public int OrderId { get; set; }

	[JsonPropertyName("origin_address")]
	public Address OriginAddress { get; set; }

	[JsonPropertyName("receipt")]
	public Receipt Receipt { get; set; }

	[JsonPropertyName("service")]
	public string Service { get; set; }

	[JsonPropertyName("shipment_status")]
	public object ShipmentStatus { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("tracking_company")]
	public string TrackingCompany { get; set; }

	[JsonPropertyName("tracking_number")]
	public string TrackingNumber { get; set; }

	[JsonPropertyName("tracking_numbers")]
	public List<string> TrackingNumbers { get; set; }

	[JsonPropertyName("tracking_url")]
	public string TrackingUrl { get; set; }

	[JsonPropertyName("tracking_urls")]
	public List<string> TrackingUrls { get; set; }

	[JsonPropertyName("updated_at")]
	public DateTime UpdatedAt { get; set; }

	[JsonPropertyName("line_items")]
	public List<LineItem> LineItems { get; set; }
}