﻿namespace MakeMyCapServer.Controllers.Model;

public class CreateSku
{
	public string Sku { get; set; }

	public string DistributorCode { get; set; }

	public string? DistributorSku { get; set; }

	public string? Brand { get; set; }

	public string StyleCode { get; set; }

	public string? PartId { get; set; }

	public string? Color { get; set; }

	public string? ColorCode { get; set; }

	public string? SizeCode { get; set; }
}