namespace Renee.Domain.Entity;

public class HouseholdDifficulty
{
	public Guid Id { get; set; }

	public Guid Household { get; set; }

	public Guid Difficulty { get; set; }

	public virtual HouseholdDifficultiesLabel DifficultyNavigation { get; set; } = null!;

	public virtual Household HouseholdNavigation { get; set; } = null!;
}