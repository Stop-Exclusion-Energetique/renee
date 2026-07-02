namespace Renee.Domain.Entity;

public class ProjectType
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<PreWorkPlanProjectType> PreWorkPlanProjectTypes { get; set; } = [];
	public virtual ICollection<WorkTypeProjectType> WorkTypeProjects { get; set; } = [];
}