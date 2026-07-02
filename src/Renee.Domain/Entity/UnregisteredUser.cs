namespace Renee.Domain.Entity;

public class UnregisteredUser
{
	public Guid Id { get; set; }

	public DateTime SubscriptionDateUtc { get; set; }

	public string? Email { get; set; }

	public string? LastName { get; set; }

	public string? FirstName { get; set; }

	public string? PhoneNumber { get; set; }

	public Guid AskedRole { get; set; }

	public string? ReportingStructure { get; set; }

	public string? Function { get; set; }

	public int UserState { get; set; }

	public Guid ReportingStructureId { get; set; }

	public string? SiretNumber { get; set; }

	public string? CguVersion { get; set; }

	public Guid? TerritoryId { get; set; }

	public virtual Role AskedRoleNavigation { get; set; } = null!;

	public virtual ReportingStructure ReportingStructureNavigation { get; set; } = null!;

	public virtual Territory? Territory { get; set; }
}