namespace Renee.Domain.Entity;

public class AnahCategory
{
	public Guid Id { get; set; }

	public int PeopleNumber { get; set; }

	public double? VeryLowIncomeHouseholdsAmount { get; set; }

	public double? LowIncomeHouseholdsAmount { get; set; }

	public bool IsInIleDeFrance { get; set; }

	public int? Year { get; set; }

	public DateTime? AnahRuleDebutDate {get; set;}

	public DateTime? AnahRuleEndDate {get; set;}

	public DateTime CreationDatetimeUtc { get; set; }

	public Guid CreatedById { get; set; }

	public DateTime LastUpdateDatetimeUtc { get; set; }

	public Guid LastUpdateById { get; set; }

	public virtual User CreatedBy { get; set; } = null!;

	public virtual User LastUpdateBy { get; set; } = null!;
}