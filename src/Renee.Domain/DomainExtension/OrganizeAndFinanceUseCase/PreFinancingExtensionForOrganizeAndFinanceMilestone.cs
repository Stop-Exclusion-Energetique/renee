using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;

public static class PreFinancingExtensionForOrganizeAndFinanceMilestone
{
	public static void UpdatePreFinancingPlan(
		this PreFinancingPlan preFinancingPlan,
		PreFinancingPlan updatedPrefinancingPlan)
	{
		preFinancingPlan.MaPrimeRenovGuidedPath = updatedPrefinancingPlan.MaPrimeRenovGuidedPath;
		preFinancingPlan.MaPrimeRenovCoOwnerShip = updatedPrefinancingPlan.MaPrimeRenovCoOwnerShip;
		preFinancingPlan.MaPrimeLogementDecent = updatedPrefinancingPlan.MaPrimeLogementDecent;
		preFinancingPlan.MaPrimeAdapt = updatedPrefinancingPlan.MaPrimeAdapt;
		preFinancingPlan.BonusForExitingEnergeticSieve = updatedPrefinancingPlan.BonusForExitingEnergeticSieve;
		preFinancingPlan.RegionalAids = updatedPrefinancingPlan.RegionalAids;
		preFinancingPlan.DepartmentalAids = updatedPrefinancingPlan.DepartmentalAids;
		preFinancingPlan.PublicEstablishmentsForInterCommunalCooperationAids =
			updatedPrefinancingPlan.PublicEstablishmentsForInterCommunalCooperationAids;
		preFinancingPlan.MunicipalityAids = updatedPrefinancingPlan.MunicipalityAids;
		preFinancingPlan.SolicitedBankLoanType = updatedPrefinancingPlan.SolicitedBankLoanType;
		preFinancingPlan.ClassicBankLoan = updatedPrefinancingPlan.ClassicBankLoan;
		preFinancingPlan.IsFinancingAskedToStopAssociation = updatedPrefinancingPlan.IsFinancingAskedToStopAssociation;
		preFinancingPlan.EstimatedRemainingAmount = updatedPrefinancingPlan.EstimatedRemainingAmount;
		preFinancingPlan.MdphFinancing = updatedPrefinancingPlan.MdphFinancing;
		preFinancingPlan.CeeFinancing = updatedPrefinancingPlan.CeeFinancing;
		preFinancingPlan.OtherFamilyMemberMaximumSupportAmountForRenovationProject =
			updatedPrefinancingPlan.OtherFamilyMemberMaximumSupportAmountForRenovationProject;
		preFinancingPlan.PensionFund = updatedPrefinancingPlan.PensionFund;
		preFinancingPlan.LeroyMerlinFoundation = updatedPrefinancingPlan.LeroyMerlinFoundation;
		preFinancingPlan.UnderprivilegedHousingFoundation = updatedPrefinancingPlan.UnderprivilegedHousingFoundation;
		preFinancingPlan.WattForChangeFoundation = updatedPrefinancingPlan.WattForChangeFoundation;
		preFinancingPlan.SocialProtectionGroup = updatedPrefinancingPlan.SocialProtectionGroup;
		preFinancingPlan.StopEnergyExclusionFunds = updatedPrefinancingPlan.StopEnergyExclusionFunds;
		preFinancingPlan.CafMsaFinancing = updatedPrefinancingPlan.CafMsaFinancing;
		preFinancingPlan.HouseholdMaximumSavingAmountForRenovationProject =
			updatedPrefinancingPlan.HouseholdMaximumSavingAmountForRenovationProject;
	}

	public static EntityChanges<FundingMode> UpdatePreFinancingPlanFundingModes(
		this PreFinancingPlan preFinancingPlan,
		List<FundingMode> fundingModes)
	{
        var fundingModesToRemove = preFinancingPlan.FundingModes
			.Where(fm => !fundingModes.Exists(ufm => ufm.Id == fm.Id)).ToList();

		foreach (var fundingMode in fundingModesToRemove) fundingMode.PreFinancingPlan = preFinancingPlan.Id;

		var fundingModesToAddId = fundingModes.Where(ufm => preFinancingPlan.FundingModes.All(fm => fm.Id != ufm.Id))
			.ToList();

		var fundingModesToAdd = fundingModesToAddId.Select(
			fundingMode => new FundingMode
			{
				PreFinancingPlan = preFinancingPlan.Id, Label = fundingMode.Label, Value = fundingMode.Value
			}).ToList();

		var fundingModesToUpdate = fundingModes.Where(uwp => preFinancingPlan.FundingModes.Any(wp => wp.Id == uwp.Id))
			.ToList();

		foreach (var fundingMode in fundingModesToUpdate) fundingMode.PreFinancingPlan = preFinancingPlan.Id;

		return new EntityChanges<FundingMode>(
			fundingModesToAdd,
			fundingModesToUpdate,
			fundingModesToRemove);
	}
}