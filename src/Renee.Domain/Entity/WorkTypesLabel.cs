namespace Renee.Domain.Entity;

public class WorkTypesLabel
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<WorkPackageWorkTypeCost> WorkPackageWorkTypeCosts { get; set; } = [];
	public virtual ICollection<WorkTypeProjectType> WorkTypeProjectTypes { get; set; } = [];
}