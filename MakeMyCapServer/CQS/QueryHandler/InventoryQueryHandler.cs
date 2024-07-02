using System.Text;
using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Model;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class InventoryQueryHandler : IQueryHandler<InventoryQuery, List<InventoryQueryResponse>>
{
	private readonly IOrderingProxy orderingProxy;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<InventoryQueryHandler> logger;
	
	public InventoryQueryHandler(IOrderingProxy orderingProxy, IProductSkuProxy productSkuProxy, ILogger<InventoryQueryHandler> logger)
	{
		this.orderingProxy = orderingProxy;
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}

	public List<InventoryQueryResponse> Handle(InventoryQuery query)
	{
		var response = new List<InventoryQueryResponse>();
		
		productSkuProxy.GetInHouseInventories().ForEach(item =>
		{
			var skuMap = productSkuProxy.GetSkuMapFor(item.Sku);
			var description = "Sku not found";
			var distributorCode = "";
			var distributorName = "";
			if (skuMap == null)
			{
				logger.LogError($"Unable to locate sku map for sku {item.Sku}");
			}
			else
			{
				description = FormatDescription(skuMap);
				var distributor = orderingProxy.GetDistributorByCode(skuMap.DistributorCode);
				if (distributor != null)
				{
					distributorCode = distributor.LookupCode;
					distributorName = distributor.Name;
				}
			}

			response.Add(new InventoryQueryResponse(item.Sku, description, item.OnHand, item.LastUsage, distributorCode, distributorName));
		});
		return response;
	}

	public static string FormatDescription(DistributorSkuMap skuMap)
	{
		var description = new StringBuilder();
		if (!string.IsNullOrEmpty(skuMap.Brand))
		{
			description.Append(skuMap.Brand);
		}

		if (!string.IsNullOrEmpty(skuMap.StyleCode))
		{
			if (description.Length > 0)
			{
				description.Append(", ");
			}

			description.Append(skuMap.StyleCode);
		}

		if (!string.IsNullOrEmpty(skuMap.Color))
		{
			description.Append(", ");
			description.Append(skuMap.Color);
		}

		if (!string.IsNullOrEmpty(skuMap.SizeCode) && skuMap.SizeCode.ToUpper() != "CUSTOM")
		{
			description.Append(", ");
			description.Append(skuMap.SizeCode);
		}

		return description.ToString();
	}
}