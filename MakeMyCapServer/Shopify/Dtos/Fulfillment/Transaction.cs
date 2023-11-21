using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Transaction
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("admin_graphql_api_id")]
	public string AdminGraphqlApiId { get; set; }

	[JsonPropertyName("amount")]
	public string Amount { get; set; }

	[JsonPropertyName("authorization")]
	public string Authorization { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonPropertyName("currency")]
	public string Currency { get; set; }

	[JsonPropertyName("device_id")]
	public object DeviceId { get; set; }

	[JsonPropertyName("error_code")]
	public string ErrorCode { get; set; }

	[JsonPropertyName("gateway")]
	public string Gateway { get; set; }

	[JsonPropertyName("kind")]
	public string Kind { get; set; }

	[JsonPropertyName("location_id")]
	public object LocationId { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; }

	[JsonPropertyName("order_id")]
	public int OrderId { get; set; }

	[JsonPropertyName("parent_id")]
	public int ParentId { get; set; }

	[JsonPropertyName("payment_id")]
	public string PaymentId { get; set; }

	[JsonPropertyName("processed_at")]
	public DateTime ProcessedAt { get; set; }

	[JsonPropertyName("receipt")]
	public Receipt Receipt { get; set; }

	[JsonPropertyName("source_name")]
	public string SourceName { get; set; }

	[JsonPropertyName("status")]
	public string Status { get; set; }

	[JsonPropertyName("test")]
	public bool Test { get; set; }

	[JsonPropertyName("user_id")]
	public object UserId { get; set; }
}