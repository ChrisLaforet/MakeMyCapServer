﻿using System.Text.Json.Serialization;

namespace ShopifyInventoryFulfillment.Shopify.Dtos;

public class PresentmentPrice
{
    [JsonPropertyName("price")]
    public Price Price { get; set; }

    [JsonPropertyName("compare_at_price")]
    public object CompareAtPrice { get; set; }
}