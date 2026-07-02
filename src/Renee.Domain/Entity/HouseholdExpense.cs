namespace Renee.Domain.Entity;

public class HouseholdExpense
{
	public Guid Id { get; set; }

	public Guid Household { get; set; }

	public int Type { get; set; }

	public double? Value { get; set; }

	public virtual Household HouseholdNavigation { get; set; } = null!;
}