using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

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