namespace MakeMyCapServer.Model;

public partial class User
{
	public string Id { get; set; } = null!;

	public string Username { get; set; } = null!;

	public string Email { get; set; } = null!;

	public DateTime CreateDate { get; set; }

	public string? PasswordHash { get; set; }

	public string? ResetKey { get; set; }

	public DateTime? ResetExpirationDatetime { get; set; }
}