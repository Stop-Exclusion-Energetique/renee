namespace Renee.Domain.Entity;

public class Household
{
	public Guid Id { get; set; }

	public int? HouseholdTypology { get; set; }

	public bool? IsFollowedByAnSocialWorker { get; set; }

	public bool? HasAnOccupantWithDisabilities { get; set; }

	public bool? HasAnOccupantWithLongTermIllness { get; set; }

	public bool? HasAnOccupantWithIndependenceLoss { get; set; }

	public bool? HasAnOccupantUnderCuratorship { get; set; }

	public bool? HasAnOccupantUnderGuardianship { get; set; }

	public double? ReferenceIncomeTax { get; set; }

	public string? AnahCategory { get; set; }

	public string? SocialContext { get; set; }

	public string? HouseholdProject { get; set; }

	public string? HouseholdAvailabilityForVisits { get; set; }

	public string? CommentsOnHouseholdDifficulties { get; set; }

	public bool? HasOverdueInvoice { get; set; }

	public Guid MainOccupant { get; set; }

	public double? EnergyEffortRate { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();

	public virtual ICollection<HouseholdDifficulty> HouseholdDifficulties { get; set; } =
		new List<HouseholdDifficulty>();

	public virtual ICollection<HouseholdExpense> HouseholdExpenses { get; set; } = new List<HouseholdExpense>();

	public virtual ICollection<HouseholdResource> HouseholdResources { get; set; } = new List<HouseholdResource>();

	public virtual ICollection<HouseholdHeatingEnergy> HouseholdHeatingEnergies { get; set; } = new List<HouseholdHeatingEnergy>();

	public virtual MainOccupant MainOccupantNavigation { get; set; } = null!;

	public virtual ICollection<SecondaryOccupant> SecondaryOccupants { get; set; } = new List<SecondaryOccupant>();
}