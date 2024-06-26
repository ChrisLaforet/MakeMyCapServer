namespace MakeMyCapServer.CQS.Response;

public class UserDataResponse
{
	public string UserName { get; }
	public string Email { get; }
	public DateTime CreateDate { get; }

	public UserDataResponse(string userName, string email, DateTime createDate)
	{
		this.UserName = userName;
		this.Email = email;
		this.CreateDate = createDate;
	}
}