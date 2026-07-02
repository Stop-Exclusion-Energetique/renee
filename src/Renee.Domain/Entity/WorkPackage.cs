namespace Renee.Domain.Entity;

public class WorkPackage
{
	public Guid Id { get; set; }

	public Guid? PreWorkPlan { get; set; }

	public Guid? CopropertyWorkFinance { get; set; }

	public string? EnergeticsEffectAfterWorks { get; set; }

	public virtual PreWorkPlan? PreWorkPlanNavigation { get; set; }

	public virtual CopropertyWorkFinance? CopropertyWorkFinanceNavigation { get; set; }

    public virtual ICollection<WorkPackageWorkTypeCost> WorkPackageWorkTypeCosts { get; set; } =
		new List<WorkPackageWorkTypeCost>();
}