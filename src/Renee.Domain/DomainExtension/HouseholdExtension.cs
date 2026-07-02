using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension;

public static class HouseholdExtension
{
	public static void UpdateHousehold(this Household household, Household updatedHousehold)
	{
		household.HouseholdTypology = updatedHousehold.HouseholdTypology;
		household.IsFollowedByAnSocialWorker = updatedHousehold.IsFollowedByAnSocialWorker;
		household.HasAnOccupantWithDisabilities = updatedHousehold.HasAnOccupantWithDisabilities;
		household.HasAnOccupantWithLongTermIllness = updatedHousehold.HasAnOccupantWithLongTermIllness;
		household.HasAnOccupantWithIndependenceLoss = updatedHousehold.HasAnOccupantWithIndependenceLoss;
		household.HasAnOccupantUnderCuratorship = updatedHousehold.HasAnOccupantUnderCuratorship;
		household.HasAnOccupantUnderGuardianship = updatedHousehold.HasAnOccupantUnderGuardianship;
		household.ReferenceIncomeTax = updatedHousehold.ReferenceIncomeTax;
		household.AnahCategory = updatedHousehold.AnahCategory;
		household.SocialContext = updatedHousehold.SocialContext;
		household.HouseholdProject = updatedHousehold.HouseholdProject;
		household.HouseholdAvailabilityForVisits = updatedHousehold.HouseholdAvailabilityForVisits;
		household.CommentsOnHouseholdDifficulties = updatedHousehold.CommentsOnHouseholdDifficulties;
		household.HasOverdueInvoice = updatedHousehold.HasOverdueInvoice;
		household.EnergyEffortRate = updatedHousehold.EnergyEffortRate;
	}

	public static EntityChanges<HouseholdDifficulty> UpdateHouseholdDifficulties(
		this Household household,
		List<Guid?>? difficulties)
	{
		if (difficulties is null) return EntityChanges<HouseholdDifficulty>.Empty;

		var difficultiesToRemove = household.HouseholdDifficulties
			.Where(d => !difficulties.Exists(ud => ud == d.Difficulty)).ToList();

		foreach (var difficulty in difficultiesToRemove) difficulty.Household = household.Id;

		var difficultiesToAddId = difficulties.Where(ud => household.HouseholdDifficulties.All(d => d.Difficulty != ud))
			.ToList();

		var difficultiesToAdd = difficultiesToAddId.OfType<Guid>().Select(
			difficulty => new HouseholdDifficulty { Household = household.Id, Difficulty = difficulty }).ToList();

		return new EntityChanges<HouseholdDifficulty>(
			difficultiesToAdd,
			[],
			difficultiesToRemove);
	}

	public static EntityChanges<HouseholdExpense> UpdateHouseholdExpense(
		this Household household,
		List<HouseholdExpense> updatedHouseholdExpenses)
	{
		var expenseToRemove = household.HouseholdExpenses
			.Where(he => !updatedHouseholdExpenses.Exists(uhe => uhe.Id == he.Id)).ToList();

		foreach (var expense in expenseToRemove) expense.Household = household.Id;

		var expenseToAdd = updatedHouseholdExpenses.Where(uhe => household.HouseholdExpenses.All(he => he.Id != uhe.Id))
			.ToList();

		foreach (var expense in expenseToAdd) expense.Household = household.Id;

		var expenseToUpdate = updatedHouseholdExpenses
			.Where(uhe => household.HouseholdExpenses.Any(he => he.Id == uhe.Id)).ToList();

		foreach (var expense in expenseToUpdate) expense.Household = household.Id;

		return new EntityChanges<HouseholdExpense>(
			expenseToAdd,
			expenseToUpdate,
			expenseToRemove);
	}

	public static EntityChanges<HouseholdResource> UpdateHouseholdResources(
		this Household household,
		List<HouseholdResource> updatedHouseholdResources)
	{
		var resourceToRemove = household.HouseholdResources.Where(
			hr => !updatedHouseholdResources.Exists(uhr => uhr.HouseholdResources == hr.HouseholdResources)).ToList();

		foreach (var resource in resourceToRemove) resource.Household = household.Id;

		var resourceToAdd = updatedHouseholdResources.Where(
			uhr => household.HouseholdResources.All(hr => hr.HouseholdResources != uhr.HouseholdResources)).ToList();

		foreach (var resource in resourceToAdd) resource.Household = household.Id;

		var resourceToUpdate = updatedHouseholdResources.Where(
			uhr => household.HouseholdResources.Any(hr => hr.HouseholdResources == uhr.HouseholdResources)).ToList();

		foreach (var resource in resourceToUpdate) resource.Household = household.Id;

		return new EntityChanges<HouseholdResource>(
			resourceToAdd,
			resourceToUpdate,
			resourceToRemove);
	}

	public static EntityChanges<HouseholdHeatingEnergy> UpdateHouseholdHeatingEnergy(
		this Household household,
		List<HouseholdHeatingEnergy> updatedHouseholdHeatingEnergy)
	{
		var HeatingEnergyToRemove = household.HouseholdHeatingEnergies.Where(
			he => !updatedHouseholdHeatingEnergy.Exists(uhe => uhe.HouseholdHeatingEnergyLabel == he.HouseholdHeatingEnergyLabel)).ToList();

		foreach (var resource in HeatingEnergyToRemove) resource.Household = household.Id;

		var HeatingEnergyToAdd = updatedHouseholdHeatingEnergy.Where(
			uhr => household.HouseholdHeatingEnergies.All(hr => hr.HouseholdHeatingEnergyLabel != uhr.HouseholdHeatingEnergyLabel)).ToList();

		foreach (var resource in HeatingEnergyToAdd) resource.Household = household.Id;

		var HeatingEnergyToUpdate = updatedHouseholdHeatingEnergy.Where(
			uhr => household.HouseholdHeatingEnergies.Any(hr => hr.HouseholdHeatingEnergyLabel == uhr.HouseholdHeatingEnergyLabel)).ToList();

		foreach (var resource in HeatingEnergyToUpdate) resource.Household = household.Id;

		return new EntityChanges<HouseholdHeatingEnergy>(
			HeatingEnergyToAdd,
			HeatingEnergyToUpdate,
			HeatingEnergyToRemove);
	}

	public static EntityChanges<SecondaryOccupant>
		UpdateHouseholdSecondaryOccupants(this Household household, List<SecondaryOccupant> updatedSecondaryOccupants)
	{
		var occupantsToRemove = household.SecondaryOccupants
			.Where(o => !updatedSecondaryOccupants.Exists(uo => uo.Id == o.Id)).ToList();

		foreach (var occupant in occupantsToRemove) occupant.Household = household.Id;

		var occupantToAdd = updatedSecondaryOccupants.Where(uo => household.SecondaryOccupants.All(o => o.Id != uo.Id))
			.ToList();

		foreach (var occupant in occupantToAdd) occupant.Household = household.Id;

		var occupantToUpdate = updatedSecondaryOccupants
			.Where(uo => household.SecondaryOccupants.Any(o => o.Id == uo.Id)).ToList();

		foreach (var occupant in occupantToUpdate) occupant.Household = household.Id;

		return new EntityChanges<SecondaryOccupant>(
			occupantToAdd,
			occupantToUpdate,
			occupantsToRemove);
	}

	public static Household UpdateMainOccupant(this Household household, MainOccupant updatedMainOccupant)
	{
		household.MainOccupantNavigation.Id = updatedMainOccupant.Id;
		household.MainOccupantNavigation.Trigram = updatedMainOccupant.Trigram;
		household.MainOccupantNavigation.Birthdate = updatedMainOccupant.Birthdate;
		household.MainOccupantNavigation.Age = updatedMainOccupant.Age;
		household.MainOccupantNavigation.SocioProfessionalCategory = updatedMainOccupant.SocioProfessionalCategory;
		household.MainOccupantNavigation.PhoneNumber = updatedMainOccupant.PhoneNumber;
		household.MainOccupantNavigation.Email = updatedMainOccupant.Email;
		household.MainOccupantNavigation.Job = updatedMainOccupant.Job;
		household.MainOccupantNavigation.SocialProtectionFund = updatedMainOccupant.SocialProtectionFund;
		household.MainOccupantNavigation.CommentOnSocialProtectionFund =
			updatedMainOccupant.CommentOnSocialProtectionFund;
		household.MainOccupantNavigation.PensionFund = updatedMainOccupant.PensionFund;
		household.MainOccupantNavigation.CommentOnPensionFund = updatedMainOccupant.CommentOnPensionFund;
		household.MainOccupantNavigation.AdditionnalFund = updatedMainOccupant.AdditionnalFund;
		household.MainOccupantNavigation.CommentOnAdditionnalFund = updatedMainOccupant.CommentOnAdditionnalFund;
		household.MainOccupantNavigation.FirstName = updatedMainOccupant.FirstName;
		household.MainOccupantNavigation.LastName = updatedMainOccupant.LastName;

		return household;
	}
}