namespace Renee.Domain.Entity;

public class WorkPackageWorkTypeCost
{
	public Guid WorkPackage { get; set; }

	public Guid WorkType { get; set; }

	public double Cost { get; set; }

	public string? Description { get; set; }

	public virtual WorkPackage WorkPackageNavigation { get; set; } = null!;

	public virtual WorkTypesLabel WorkTypeNavigation { get; set; } = null!;
}