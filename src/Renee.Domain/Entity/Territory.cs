namespace Renee.Domain.Entity;

public class Territory
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();
	public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();
	public virtual ICollection<UnregisteredUser> UnregisteredUsers { get; set; } = new List<UnregisteredUser>();
	public virtual ICollection<User> Users { get; set; } = new List<User>();
}