namespace Renee.Domain.Entity;

public class ReportingStructure
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public Guid? NationalStructureId { get; set; }

	public virtual ICollection<UnregisteredUser> UnregisteredUsers { get; set; } = new List<UnregisteredUser>();

	public virtual ICollection<User> Users { get; set; } = new List<User>();

	public virtual NationalStructure? NationalStructureNavigation { get; set; }
}