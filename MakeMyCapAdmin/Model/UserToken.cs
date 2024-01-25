namespace MakeMyCapAdmin.Model;

public partial class UserToken
{
	public string Id { get; set; } = null!;

	public string UserId { get; set; } = null!;

	public DateTime Created { get; set; }

	public DateTime Expired { get; set; }

	public virtual User User { get; set; } = null!;
}