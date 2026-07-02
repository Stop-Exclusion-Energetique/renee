namespace Renee.Domain.Entity;

public class HouseholdResource
{
	public Guid Household { get; set; }

	public Guid HouseholdResources { get; set; }

	public double Value { get; set; }

	public virtual Household HouseholdNavigation { get; set; } = null!;

	public virtual HouseholdResourcesLabel HouseholdResourcesNavigation { get; set; } = null!;
}