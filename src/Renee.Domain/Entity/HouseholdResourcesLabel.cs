namespace Renee.Domain.Entity;

public class HouseholdResourcesLabel
{
	public Guid Id { get; set; }

	public string Labels { get; set; } = null!;

	public virtual ICollection<HouseholdResource> HouseholdResources { get; set; } = new List<HouseholdResource>();
}