using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class DistributorSkusQueryHandler : IQueryHandler<DistributorSkusQuery, DistributorSkusResponse>
{
	private readonly IOrderingProxy orderingProxy;
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<DistributorSkusQueryHandler> logger;
	
	public DistributorSkusQueryHandler(IOrderingProxy orderingProxy, IProductSkuProxy productSkuProxy, ILogger<DistributorSkusQueryHandler> logger)
	{
		this.orderingProxy = orderingProxy;
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public DistributorSkusResponse Handle(DistributorSkusQuery query)
	{
		var distributor = orderingProxy.GetDistributorByCode(query.DistributorCode);
		if (distributor == null)
		{
			logger.LogInformation($"Request for skus for non-existent distributor code {query.DistributorCode}");
			return new DistributorSkusResponse("", query.DistributorCode, new List<AssignedSkuResponse>());
		}
		
		var maps = productSkuProxy.GetSkuMapsFor(query.DistributorCode);
		var assignedSkus = new List<AssignedSkuResponse>();
		foreach (var map in maps)
		{
			assignedSkus.Add(new AssignedSkuResponse(map));
		}
		
		return new DistributorSkusResponse(distributor.Name, distributor.LookupCode, assignedSkus);
	}
}