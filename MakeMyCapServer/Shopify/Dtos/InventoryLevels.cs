﻿using System.Text.Json.Serialization;

namespace MakeMyCapServer.Shopify.Dtos;

public class InventoryLevels
{
	[JsonPropertyName("inventory_levels")]
	public InventoryLevel[] Items { get; set; }
}