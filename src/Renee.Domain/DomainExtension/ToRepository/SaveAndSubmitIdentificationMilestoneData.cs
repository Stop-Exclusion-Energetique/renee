using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.ToRepository;

public record SaveAndSubmitIdentificationMilestoneData(
	EntityChanges<SecondaryOccupant> SecondaryOccupantChanges,
	EntityChanges<HouseholdExpense> HouseholdExpenseChanges,
	EntityChanges<HouseholdResource> HouseholdResourceChanges,
	EntityChanges<HouseholdDifficulty> HouseholdDifficultyChanges,
	EntityChanges<HouseholdHeatingEnergy> HouseholdHeatingEnergyChanges);