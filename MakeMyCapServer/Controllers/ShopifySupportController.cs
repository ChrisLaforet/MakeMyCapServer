using MakeMyCapServer.Controllers.Model;
using MakeMyCapServer.CQS.Command;
using MakeMyCapServer.CQS.CommandHandler;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.QueryHandler;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MakeMyCapServer.Controllers;

[ApiController]
[Route("[controller]")]
public class ShopifySupportController : ControllerBase
{
	private readonly CreateSkuCommandHandler createSkuCommandHandler;
	private readonly UpdateSkuCommandHandler updateSkuCommandHandler;
	private readonly DeleteSkuCommandHandler deleteSkuCommandHandler;
	
	private readonly DistributorsQueryHandler distributorsQueryHandler;
	private readonly DistributorSkusQueryHandler distributorSkusQueryHandler;
	private readonly SkuQueryHandler skuQueryHandler;

	private readonly ILogger<ShopifySupportController> logger;

	public ShopifySupportController(IServiceProvider serviceProvider, ILogger<ShopifySupportController> logger)
	{
		this.logger = logger;

		createSkuCommandHandler = ActivatorUtilities.CreateInstance<CreateSkuCommandHandler>(serviceProvider);
		updateSkuCommandHandler = ActivatorUtilities.CreateInstance<UpdateSkuCommandHandler>(serviceProvider);
		deleteSkuCommandHandler = ActivatorUtilities.CreateInstance<DeleteSkuCommandHandler>(serviceProvider);

		distributorsQueryHandler = ActivatorUtilities.CreateInstance<DistributorsQueryHandler>(serviceProvider);
		distributorSkusQueryHandler = ActivatorUtilities.CreateInstance<DistributorSkusQueryHandler>(serviceProvider);
		skuQueryHandler = ActivatorUtilities.CreateInstance<SkuQueryHandler>(serviceProvider);
	}
	
	[Authorize]
	[HttpGet("distributors")]
	public IActionResult Distributors()
	{
		var distributors = distributorsQueryHandler.Handle(new DistributorsQuery());
		return Ok(new {distributors = distributors});
	}

	[Authorize]
	[HttpGet("skus")]
	public IActionResult Skus(string id)
	{
		var response = distributorSkusQueryHandler.Handle(new DistributorSkusQuery(id));
		return Ok(response);
	}
	
	[Authorize]
	[HttpPost("create-sku")]
	public IActionResult CreateSku(CreateSku model)
	{
		var skuResponse = skuQueryHandler.Handle(new SkuQuery(model.Sku.Trim()));
		if (skuResponse != null)
		{
			return BadRequest($"There is already an assigned record with SKU {model.Sku} for {skuResponse.DistributorCode}");
		}
	
		try
		{
			createSkuCommandHandler.Handle(new CreateSkuCommand(model));
			return Ok();
		}
		catch (Exception)
		{
			return BadRequest( $"There was an error while creating record with SKU {model.Sku}");
		}
	}
	
	[Authorize]
	[HttpPost("update-sku")]
	public IActionResult UpdateSku(UpdateSku model)
	{
		if (!string.Equals(model.NewSku, model.OriginalSku, StringComparison.CurrentCultureIgnoreCase))
		{
			var skuResponse = skuQueryHandler.Handle(new SkuQuery(model.NewSku.Trim()));
			if (skuResponse != null)
			{
				return BadRequest($"There is already an assigned record with SKU {model.NewSku} for {skuResponse.DistributorCode}");
			}	
		}
	
		try
		{
			updateSkuCommandHandler.Handle(new UpdateSkuCommand(model));
			return Ok();
		}
		catch (Exception)
		{
			return BadRequest( $"There was an error while updating record with SKU {model.OriginalSku}");
		}
	}
	
	[Authorize]
	[HttpPost("delete-sku")]
	public IActionResult DeleteSku(DeleteSku model)
	{
		try
		{
			deleteSkuCommandHandler.Handle(new DeleteSkuCommand(model.Sku));
			return Ok();
		}
		catch (Exception)
		{
			return BadRequest( $"There was an error while deleting record with SKU {model.Sku}");
		}
	}
}