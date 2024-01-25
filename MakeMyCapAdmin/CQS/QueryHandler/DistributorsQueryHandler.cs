using MakeMyCapAdmin.CQS.Query;
using MakeMyCapAdmin.CQS.Response;
using MakeMyCapAdmin.Proxies;

namespace MakeMyCapAdmin.CQS.QueryHandler;

public class DistributorsQueryHandler : IQueryHandler<DistributorsQuery, List<DistributorResponse>>
{
	private readonly IOrderingProxy orderingProxy;
	private readonly ILogger<DistributorsQueryHandler> logger;
	
	public DistributorsQueryHandler(IOrderingProxy orderingProxy, ILogger<DistributorsQueryHandler> logger)
	{
		this.orderingProxy = orderingProxy;
		this.logger = logger;
	}

	public List<DistributorResponse> Handle(DistributorsQuery query)
	{
		var response = new List<DistributorResponse>();
		return orderingProxy.GetDistributors().Select(distributor => new DistributorResponse(distributor.Name, distributor.LookupCode)).ToList();
	}
}