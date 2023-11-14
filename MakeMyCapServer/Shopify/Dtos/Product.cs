using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos;

public class Product
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("body_html")]
    public string BodyHtml { get; set; }

    [JsonPropertyName("vendor")]
    public string Vendor { get; set; }

    [JsonPropertyName("product_type")]
    public string ProductType { get; set; }

    [JsonPropertyName("created_at")]
    public DateTime CreatedAt { get; set; }

    [JsonPropertyName("handle")]
    public string Handle { get; set; }

    [JsonPropertyName("updated_at")]
    public DateTime? UpdatedAt { get; set; }

    [JsonPropertyName("published_at")]
    public DateTime? PublishedAt { get; set; }

    [JsonPropertyName("template_suffix")]
    public object TemplateSuffix { get; set; }

    [JsonPropertyName("published_scope")]
    public string PublishedScope { get; set; }

    [JsonPropertyName("tags")]
    public string Tags { get; set; }

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("admin_graphql_api_id")]
    public string AdminGraphqlApiId { get; set; }

    [JsonPropertyName("variants")]
    public List<Variant> Variants { get; set; }

    [JsonPropertyName("options")]
    public List<Option> Options { get; set; }

    [JsonPropertyName("images")]
    public List<Image> Images { get; set; }

    [JsonPropertyName("image")]
    public Image? Image { get; set; }

}