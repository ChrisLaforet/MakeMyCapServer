using MakeMyCapServer.CQS.Interfaces;
using MakeMyCapServer.CQS.Query;
using MakeMyCapServer.CQS.Response;
using MakeMyCapServer.Proxies;

namespace MakeMyCapServer.CQS.QueryHandler;

public class UsersQueryHandler : IQueryHandler<UsersQuery, List<UserDataResponse>>
{
	private readonly IUserProxy userProxy;
	private readonly ILogger<UserQueryHandler> logger;
	
	public UsersQueryHandler(IUserProxy userProxy, ILogger<UserQueryHandler> logger)
	{
		this.userProxy = userProxy;
		this.logger = logger;
	}

	public List<UserDataResponse> Handle(UsersQuery query)
	{
		var userData = new List<UserDataResponse>();
		foreach (var user in userProxy.GetUsers())
		{
			userData.Add(new UserDataResponse(user.Username, user.Email, user.CreateDate));
		}
		return userData;
	}
}