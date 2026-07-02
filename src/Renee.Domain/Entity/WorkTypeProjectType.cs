namespace Renee.Domain.Entity;

public class WorkTypeProjectType
{
	public Guid WorkTypeId { get; set; }
	public Guid ProjectTypeId { get; set; }

	public WorkTypesLabel WorkType { get; set; } = null!;
	public ProjectType ProjectType { get; set; } = null!;
}
