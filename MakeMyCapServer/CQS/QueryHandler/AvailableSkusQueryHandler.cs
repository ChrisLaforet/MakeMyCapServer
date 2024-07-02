using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class AvailableSkusQueryHandler : IQueryHandler<AvailableSkusQuery, List<AvailableSkuResponse>>
{
	private readonly IOrderingProxy orderingProxy;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<AvailableSkusQueryHandler> logger;
	
	public AvailableSkusQueryHandler(IOrderingProxy orderingProxy, IProductSkuProxy productSkuProxy, ILogger<AvailableSkusQueryHandler> logger)
	{
		this.orderingProxy = orderingProxy;
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}

	public List<AvailableSkuResponse> Handle(AvailableSkusQuery query)
	{
		var existingSkus = productSkuProxy.GetInHouseInventories().Select(item => item.Sku);
		var response = new List<AvailableSkuResponse>();

		orderingProxy.GetDistributors().ForEach(distributor =>
		{
			productSkuProxy.GetSkuMapsFor(distributor.LookupCode).ForEach(map =>
			{
				if (!existingSkus.Contains(map.Sku))
				{
					response.Add(new AvailableSkuResponse(map.Sku, InventoryQueryHandler.FormatDescription(map), distributor.LookupCode, distributor.Name));
				}
			});
		});
		
		response.Sort((a, b) => string.Compare(a.Sku, b.Sku, StringComparison.CurrentCultureIgnoreCase));
		return response;
	}
}