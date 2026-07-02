namespace Renee.Domain.Entity;

public class InsuranceType
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public virtual ICollection<PreWorkPlanInsuranceType> PreWorkPlanInsuranceTypes { get; set; } =
		new List<PreWorkPlanInsuranceType>();
}