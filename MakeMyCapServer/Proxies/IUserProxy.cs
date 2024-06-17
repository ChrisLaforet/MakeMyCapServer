using MakeMyCapServer.Model;
using MakeMyCapServer.Models;

namespace MakeMyCapServer.Proxies;

public interface IUserProxy
{
	User? GetUserById(string id);
	User? GetUserByGuid(Guid guid);
	User? GetUserByUsername(string username);
	User? GetUserByEmail(string email);
	IList<User> GetUsers();
	AuthenticatedUser AuthenticateUser(string username, string password);
	string CreateUser(User user);
	void ValidateEmailForUser(string username, string password);
	void UpdateUser(string userId, string userName, string email);

	void SetPasswordFor(string username, string confirmationCode, string password);
	void SetPasswordFor(string userId, string password);
	string HashPassword(string password);
	void ChangePasswordFor(string email);
	void LogoutUser(string username);
	User CreateUser(string userName, string email);
	
	void ExpireUserTokens();
	UserToken? ValidateUserToken(string token);
}