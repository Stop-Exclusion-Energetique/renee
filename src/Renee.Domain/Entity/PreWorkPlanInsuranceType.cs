namespace Renee.Domain.Entity;

public class PreWorkPlanInsuranceType
{
	public Guid Id { get; set; }

	public Guid PreWorkPlan { get; set; }

	public Guid InsuranceType { get; set; }

	public virtual InsuranceType InsuranceTypeNavigation { get; set; } = null!;

	public virtual PreWorkPlan PreWorkPlanNavigation { get; set; } = null!;
}