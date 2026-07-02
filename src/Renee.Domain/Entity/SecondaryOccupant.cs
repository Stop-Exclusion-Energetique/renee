namespace Renee.Domain.Entity;

public class SecondaryOccupant
{
	public Guid Id { get; set; }

	public string Trigram { get; set; } = null!;

	public DateTime? Birthdate { get; set; }

	public int? Age { get; set; }

	public Guid? Household { get; set; }

	public bool? IsDependent { get; set; }

	public virtual Household? HouseholdNavigation { get; set; }
}