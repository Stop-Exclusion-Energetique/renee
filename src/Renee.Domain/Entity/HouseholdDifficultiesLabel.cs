namespace Renee.Domain.Entity;

public class HouseholdDifficultiesLabel
{
	public Guid Id { get; set; }

	public string Labels { get; set; } = null!;

	public virtual ICollection<HouseholdDifficulty> HouseholdDifficulties { get; set; } =
		new List<HouseholdDifficulty>();
}