using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class Refund
{
	[JsonPropertyName("id")]
	public long Id { get; set; }

	[JsonPropertyName("admin_graphql_api_id")]
	public string AdminGraphqlApiId { get; set; }

	[JsonPropertyName("created_at")]
	public DateTime CreatedAt { get; set; }

	[JsonPropertyName("note")]
	public string Note { get; set; }

	[JsonPropertyName("order_id")]
	public long OrderId { get; set; }

	[JsonPropertyName("processed_at")]
	public DateTime ProcessedAt { get; set; }

	[JsonPropertyName("restock")]
	public bool Restock { get; set; }

	[JsonPropertyName("total_additional_fees_set")]
	public AmountSet TotalAdditionalFeesSet { get; set; }

	[JsonPropertyName("total_duties_set")]
	public AmountSet TotalDutiesSet { get; set; }

	[JsonPropertyName("user_id")]
	public long UserId { get; set; }

	[JsonPropertyName("order_adjustments")]
	public List<object> OrderAdjustments { get; set; }

	[JsonPropertyName("transactions")]
	public List<Transaction> Transactions { get; set; }

	[JsonPropertyName("refund_line_items")]
	public List<RefundLineItem> RefundLineItems { get; set; }

	[JsonPropertyName("duties")]
	public List<object> Duties { get; set; }

	[JsonPropertyName("additional_fees")]
	public List<object> AdditionalFees { get; set; }
}