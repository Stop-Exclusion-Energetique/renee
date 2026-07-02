namespace Renee.Domain.Entity;

public class Role
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public string LongName { get; set; } = null!;

	public bool IsVisibled { get; set; }

	public virtual ICollection<UnregisteredUser> UnregisteredUsers { get; set; } = new List<UnregisteredUser>();

	public virtual ICollection<User> Users { get; set; } = new List<User>();
}