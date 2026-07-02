namespace Renee.Domain.Entity;

public class HouseholdHeatingEnergy
{
	public Guid Household { get; set; }

	public Guid HouseholdHeatingEnergyLabel { get; set; }

	public double Value { get; set; }

	public virtual Household HouseholdNavigation { get; set; } = null!;

	public virtual HouseholdHeatingEnergyLabel HouseholdHeatingEnergyNavigation { get; set; } = null!;
}
