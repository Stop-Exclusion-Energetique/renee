namespace Renee.Domain.Entity;

public class PreWorkPlanProjectType
{
	public Guid Id { get; set; }

	public Guid PreWorkPlan { get; set; }

	public Guid ProjectType { get; set; }

	public virtual PreWorkPlan PreWorkPlanNavigation { get; set; } = null!;

	public virtual ProjectType ProjectTypeNavigation { get; set; } = null!;
}