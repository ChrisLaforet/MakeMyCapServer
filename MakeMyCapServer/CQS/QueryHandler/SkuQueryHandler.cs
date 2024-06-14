using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class SkuQueryHandler : IQueryHandler<SkuQuery, AssignedSkuResponse?>
{
	private readonly IProductSkuProxy productSkuProxy;
	private readonly ILogger<SkuQueryHandler> logger;
	
	public SkuQueryHandler(IProductSkuProxy productSkuProxy, ILogger<SkuQueryHandler> logger)
	{
		this.productSkuProxy = productSkuProxy;
		this.logger = logger;
	}
	
	public AssignedSkuResponse? Handle(SkuQuery query)
	{
		var map = productSkuProxy.GetSkuMapFor(query.Sku);
		if (map == null)
		{
			return null;
		}

		return new AssignedSkuResponse(map);
	}
}