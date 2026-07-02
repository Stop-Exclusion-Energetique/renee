namespace Renee.Domain.Entity;

public class HouseholdHeatingEnergyLabel
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<HouseholdHeatingEnergy> HouseholdHeatingEnergies { get; set; } = new List<HouseholdHeatingEnergy>();
}
