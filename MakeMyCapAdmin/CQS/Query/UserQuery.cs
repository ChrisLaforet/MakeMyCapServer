namespace MakeMyCapAdmin.CQS.Query;

public class UserQuery : IQuery
{
	public string UserName { get; }
	public string UserEmail { get;  }

	public UserQuery(string userName, string userEmail)
	{
		UserName = userName;
		UserEmail = userEmail;
	}
}