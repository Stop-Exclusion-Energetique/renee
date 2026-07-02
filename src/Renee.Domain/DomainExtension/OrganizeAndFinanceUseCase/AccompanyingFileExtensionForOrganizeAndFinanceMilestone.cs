using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;

public static class AccompanyingFileExtensionForOrganizeAndFinanceMilestone
{
	public static AccompanyingFile UpdateHousing(
		this AccompanyingFile accompanyingFile,
		Housing housing,
		HousingInitialState initialState,
		HousingAfterWorkState afterWorkState)
	{
		accompanyingFile.AccompanyingFileHousingNavigation.UpdateHousing(housing).UpdateInitialState(initialState)
			.UpdateHousingAfterWorkState(afterWorkState);

		return accompanyingFile;
	}

	public static void UpdatePreFinancingPlan(this AccompanyingFile accompanyingFile, PreFinancingPlan preFinancingPlan)
	{
		accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UpdatePreFinancingPlan(preFinancingPlan);
	}

	public static AccompanyingFile UpdatePreWorkPlan(this AccompanyingFile accompanyingFile, PreWorkPlan preWorkPlan)
	{
		accompanyingFile.AccompanyingFilePreWorkPlanNavigation.UpdatePreWorkPlan(preWorkPlan);

		return accompanyingFile;
	}
}