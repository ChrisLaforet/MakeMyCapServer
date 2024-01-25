using MakeMyCapAdmin.CQS.Query;
using MakeMyCapAdmin.CQS.Response;
using MakeMyCapAdmin.Proxies;

namespace MakeMyCapAdmin.CQS.QueryHandler;

public class UserQueryHandler : IQueryHandler<UserQuery, UserStatusResponse>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<UserQueryHandler> logger;
	
	public UserQueryHandler(IUserProxy userProxy, ILogger<UserQueryHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}
	
	public UserStatusResponse Handle(UserQuery query)
	{
		var userByName = userProxy.GetUserByUsername(query.UserName);
		var userByEmail = userProxy.GetUserByEmail(query.UserEmail);

		return new UserStatusResponse(userByEmail != null, userByName != null);
	}
}