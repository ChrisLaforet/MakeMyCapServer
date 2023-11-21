using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos.Fulfillment;

public class RefundLineItem
{
	[JsonPropertyName("id")]
	public int Id { get; set; }

	[JsonPropertyName("line_item_id")]
	public int LineItemId { get; set; }

	[JsonPropertyName("location_id")]
	public int LocationId { get; set; }

	[JsonPropertyName("quantity")]
	public int Quantity { get; set; }

	[JsonPropertyName("restock_type")]
	public string RestockType { get; set; }

	[JsonPropertyName("subtotal")]
	public double Subtotal { get; set; }

	[JsonPropertyName("subtotal_set")]
	public AmountSet SubtotalSet { get; set; }

	[JsonPropertyName("total_tax")]
	public double TotalTax { get; set; }

	[JsonPropertyName("total_tax_set")]
	public AmountSet TotalTaxSet { get; set; }

	[JsonPropertyName("line_item")]
	public LineItem LineItem { get; set; }
}