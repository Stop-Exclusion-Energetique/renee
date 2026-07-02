namespace Renee.Domain.Entity;

public class FundingMode
{
	public Guid Id { get; set; }

	public Guid PreFinancingPlan { get; set; }

	public string Label { get; set; } = null!;

	public double Value { get; set; }

	public virtual PreFinancingPlan PreFinancingPlanNavigation { get; set; } = null!;
}