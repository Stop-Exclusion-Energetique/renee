using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Application.Helpers;

public static class StatisticsHelper
{
	public static double CalculateEstimatedRemainingAmount(AccompanyingFile accompanyingFile) =>
		accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.EstimatedRemainingAmount ?? 0;

	public static double CalculateTotalWorkPackageCosts(AccompanyingFile accompanyingFile) =>
		accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorkPackages.Sum(CalculateWorkPackageCosts);

	public static int GetAccompanyingFilesCount(
		List<AccompanyingFile> accompanyingFiles,
		AccompanyingFileStage? stage = null)
	{
		return stage.HasValue
			? accompanyingFiles.Count(af => af.AccompanyingFileMilestone == (int)stage.Value)
			: accompanyingFiles.Count(af => af.AccompanyingFileMilestone != (int)AccompanyingFileStage.Finished);
	}

	public static double? GetAverageAgeMainOccupant(List<AccompanyingFile> accompanyingFiles) =>
		accompanyingFiles.Where(e => e.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Age != null)
			.Average(e => e.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.Age);


	public static double? GetAverageDeliveryTime(List<AccompanyingFile> accompanyingFiles) =>
		accompanyingFiles.Where(e => e.DeliveryTime != null).Average(e => e.DeliveryTime);

	public static double? GetAverageEnergyEffortBeforeWork(List<AccompanyingFile> accompanyingFiles) =>
		accompanyingFiles.Where(e => e.AccompanyingFileHouseholdNavigation.EnergyEffortRate != null)
			.Average(e => e.AccompanyingFileHouseholdNavigation.EnergyEffortRate);

	public static Dictionary<FundingType, double?> GetAverageFundingByType(List<AccompanyingFile> accompanyingFiles)
	{
		var averageFundingByType = new Dictionary<FundingType, double?>();

		var region = accompanyingFiles.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.RegionalAids != null)
						 .Average(e => e.AccompanyingFilePreFinancingPlanNavigation.RegionalAids) ?? 0;
		averageFundingByType.Add(FundingType.Region, Math.Round(region, 1));

		var department = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids) ?? 0;
		averageFundingByType.Add(FundingType.Department, Math.Round(department, 1));

		var publicEstablishmentsIntercommunalCooperationWithoutUnit = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.PublicEstablishmentsForInterCommunalCooperationAids != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.PublicEstablishmentsForInterCommunalCooperationAids) ?? 0;
		averageFundingByType.Add(
			FundingType.PublicEstablishmentsInterCooperation,
			Math.Round(publicEstablishmentsIntercommunalCooperationWithoutUnit, 1));

		var municipality = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids) ?? 0;
		averageFundingByType.Add(FundingType.Municipality, Math.Round(municipality, 1));

		var privateActors = CalculatePrivateActorsTotal(accompanyingFiles);
		averageFundingByType.Add(FundingType.PrivateActors, Math.Round(privateActors, 1));

		var pensionFunds = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.PensionFund != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.PensionFund) ?? 0;
		averageFundingByType.Add(FundingType.PensionFunds, Math.Round(pensionFunds, 1));

		var HouseholdMaximumSavingAmountForRenovationProjectWithoutUnit = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.HouseholdMaximumSavingAmountForRenovationProject != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.HouseholdMaximumSavingAmountForRenovationProject) ?? 0;
		averageFundingByType.Add(
			FundingType.HouseholdMaximumSavingAmountForRenovationProject,
			Math.Round(HouseholdMaximumSavingAmountForRenovationProjectWithoutUnit, 1));

		var maximumAmountSupportFamilyMembersRenovationProjectWithoutUnit = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.OtherFamilyMemberMaximumSupportAmountForRenovationProject != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.OtherFamilyMemberMaximumSupportAmountForRenovationProject) ?? 0;
		averageFundingByType.Add(
			FundingType.MaximumAmountSupportFamilyMembersRenovationProject,
			Math.Round(maximumAmountSupportFamilyMembersRenovationProjectWithoutUnit, 1));

		return averageFundingByType;
	}

	public static Dictionary<EstimatedJumpClass, double> GetAverageWorkCostByEnergyJump(
		List<AccompanyingFile> accompanyingFiles,
		List<DpeLabel?>? dpeLabels)
	{
		var result = new Dictionary<EstimatedJumpClass, double>();

		foreach (var jumpClass in Enum.GetValues<EstimatedJumpClass>())
		{
			var filteredFiles = accompanyingFiles.Where(
				af =>
					af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeclassJump !=
					null &&
					af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeclassJump ==
					(int)jumpClass);

			if (dpeLabels is { Count: > 0 })
				filteredFiles = filteredFiles.Where(
					af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe != null &&
						  dpeLabels.Contains(
							  (DpeLabel)af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe));

			var filteredFilesList = filteredFiles.ToList();
			if (filteredFilesList.Any())	
				result[jumpClass] =
					filteredFilesList.Average(af => af.AccompanyingFileWorkMonitoringNavigation?.WorkTotalCost ?? 0);
			else
				result[jumpClass] = 0;
		}

		return result;
	}

	public static double GetEnergyPrivationRate(List<AccompanyingFile> accompanyingFiles)
	{
		var accompanyingFileWithEnergyPrivationCount = accompanyingFiles.Count(
			e => e.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.EnergyDepravation != null);
		var accompanyingFileWithTotalEnergyPrivationCount = accompanyingFiles.Count(
			e => e.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.EnergyDepravation ==
				 (int)EnergyDeprivation.Total);

		return accompanyingFileWithEnergyPrivationCount == 0
			? 0
			: (double)accompanyingFileWithTotalEnergyPrivationCount / accompanyingFileWithEnergyPrivationCount;
	}

	public static Dictionary<EstimatedJumpClass, int>
		GetEstimatedEnergyJumpCount(List<AccompanyingFile> accompanyingFiles) =>
		GetCountByEnum<EstimatedJumpClass>(
			accompanyingFiles,
			af => af.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.EstimatedDpeclassJump);

	public static double GetHouseholdCategoryAnahCount(List<AccompanyingFile> accompanyingFiles, string category) =>
		accompanyingFiles.Where(af => af.AccompanyingFileHouseholdNavigation.AnahCategory != null).Count(
			af => af.AccompanyingFileHouseholdNavigation.AnahCategory != null &&
				  af.AccompanyingFileHouseholdNavigation.AnahCategory.Equals(category));

	public static Dictionary<DpeLabel, int> GetHouseHoldsByInitialDpeCount(List<AccompanyingFile> accompanyingFiles) =>
		GetCountByEnum<DpeLabel>(
			accompanyingFiles,
			af => af.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe);

	public static Dictionary<MarkerNature, int>
		GetHouseHoldsByMarkerNatureCount(List<AccompanyingFile> accompanyingFiles) =>
		GetCountByEnum<MarkerNature>(accompanyingFiles, af => af.AccompanyingFileSupportTeamNavigation.MarkerNature);

	public static Dictionary<AnahType, int> GetHouseholdsByTypesOfANAHCount(List<AccompanyingFile> accompanyingFiles)
	{
		var householdsByTypesOfANAH = new Dictionary<AnahType, int>();
		var guidedPathwayBonus = accompanyingFiles.Count(
			e => e.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovGuidedPath is > 0);
		var coOwnershipBonus = accompanyingFiles.Count(
			e => e.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovCoOwnerShip is > 0);
		var decentHousingBonus = accompanyingFiles.Count(
			e => e.AccompanyingFilePreFinancingPlanNavigation.MaPrimeLogementDecent is > 0);
		var guidedPathwayBonusAdaptationBonus = accompanyingFiles.Count(
			                                        e => e.AccompanyingFilePreFinancingPlanNavigation.MaPrimeAdapt is > 0) +
		                                        guidedPathwayBonus;

		householdsByTypesOfANAH.Add(AnahType.GuidedPathwayBonus, guidedPathwayBonus);
		householdsByTypesOfANAH.Add(AnahType.CoOwnershipBonus, coOwnershipBonus);
		householdsByTypesOfANAH.Add(AnahType.DecentHousingBonus, decentHousingBonus);
		householdsByTypesOfANAH.Add(AnahType.GuidedPathwayBonusAdaptationBonus, guidedPathwayBonusAdaptationBonus);

		return householdsByTypesOfANAH;
	}

	public static Dictionary<HouseholdTypology, int>
		GetHouseholdTypologiesCount(List<AccompanyingFile> accompanyingFiles) =>
		GetCountByEnum<HouseholdTypology>(
			accompanyingFiles,
			af => af.AccompanyingFileHouseholdNavigation.HouseholdTypology);

	public static long
		GetOwnershipStatusCount(List<AccompanyingFile> accompanyingFiles, OwnershipStatus ownershipStatus) =>
		accompanyingFiles.Where(af => af.AccompanyingFileHousingNavigation.OwnershipStatus != null).Count(
			af => af.AccompanyingFileHousingNavigation.OwnershipStatus == (int)ownershipStatus);

	public static double GetPassageRateFromFirstStageToSecondStage(List<AccompanyingFile> accompanyingFiles)
	{
		var numberOfContractSigned = accompanyingFiles.Count(af => af.StartOfAccompanyingDate != null);
		var numberOfFirstEncounter = accompanyingFiles.Count(af => af.FirstEncounterDate != null);

		return numberOfFirstEncounter == 0 ? 0 : (double)numberOfContractSigned / numberOfFirstEncounter;
	}


	public static double GetPassageRateFromFirstStageToThirdStage(List<AccompanyingFile> accompanyingFiles)
	{
		var numberOfAccompanyingEndDate = accompanyingFiles.Count(af => af.EndOfAccompanyingDate != null);
		var numberOfFirstEncounter = accompanyingFiles.Count(af => af.FirstEncounterDate != null);

		return numberOfFirstEncounter == 0 ? 0 : (double)numberOfAccompanyingEndDate / numberOfFirstEncounter;
	}

	public static double? GetTaxRevenueAverage(List<AccompanyingFile> accompanyingFiles) =>
		accompanyingFiles
			.Where(
				e => e.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax is > 0).Average(
				e => e.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax);

	public static long SocioProfessionnalCategoryCount(
		List<AccompanyingFile> accompanyingFiles,
		SocioProfessionalCategory category)
	{
		return accompanyingFiles.Count(
			af => af.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.SocioProfessionalCategory ==
				  (int)category);
	}

	private static double CalculatePrivateActorsTotal(List<AccompanyingFile> accompanyingFiles)
	{
		var underprivilegedHousingFoundation = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation != null)
			.Average(e => e.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation);
		var leroyMerlinFoundation = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation != null).Average(
				e => e.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation);
		var wattForChangeFoundation = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation != null).Average(
				e => e.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation);
		var socialProtectionGroup = accompanyingFiles
			.Where(e => e.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup != null).Average(
				e => e.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup);

		return (underprivilegedHousingFoundation ?? 0) +
			   (leroyMerlinFoundation ?? 0) +
			   (wattForChangeFoundation ?? 0) +
			   (socialProtectionGroup ?? 0);
	}

	private static double CalculateWorkPackageCosts(WorkPackage wp) => wp.WorkPackageWorkTypeCosts.Sum(wpt => wpt.Cost);

	private static Dictionary<TEnum, int> GetCountByEnum<TEnum>(
		List<AccompanyingFile> accompanyingFiles,
		Func<AccompanyingFile, int?> selector) where TEnum : Enum
	{
		var result = new Dictionary<TEnum, int>();
		foreach (var value in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
			result[value] = accompanyingFiles.Count(af => selector(af) == Convert.ToInt32(value));
		return result;
	}

	public static Dictionary<AccompanyingFileStage, List<CompletionSpeedDataItem>> GetCompletionSpeed(
		List<AccompanyingFile> accompanyingFiles,
		DateTime? dateFrom,
		DateTime? dateTo)
	{
		var now = DateTime.Now;
		var endDate = dateTo ?? new DateTime(now.Year, now.Month, DateTime.DaysInMonth(now.Year, now.Month), 23, 59, 59, DateTimeKind.Utc);

		var currentMonthStart = new DateTime(endDate.Year, endDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
		var startDate = dateFrom ?? currentMonthStart.AddMonths(-5);

		var result = new Dictionary<AccompanyingFileStage, List<CompletionSpeedDataItem>>
		{
			{ AccompanyingFileStage.Identify, new List<CompletionSpeedDataItem>() },
			{ AccompanyingFileStage.OrganizingAndFinancing, new List<CompletionSpeedDataItem>() },
			{ AccompanyingFileStage.RealisationAndFollowing, new List<CompletionSpeedDataItem>() }
		};

		for (DateTime iterator = startDate; iterator <= endDate; iterator = iterator.AddMonths(1))
		{
			var currentMonthStartDate = new DateTime(
				iterator.Year,
				iterator.Month,
				1,
				0, 0, 0,
				DateTimeKind.Utc
			);

			var currentMonthEndDate = new DateTime(
				iterator.Year,
				iterator.Month,
				DateTime.DaysInMonth(iterator.Year, iterator.Month),
				23, 59, 59,
				DateTimeKind.Utc
			);

			foreach (AccompanyingFileStage stage in Enum.GetValues<AccompanyingFileStage>().Where(s => s != AccompanyingFileStage.Finished))
			{
				int count = GetCountForPhase(accompanyingFiles, stage, currentMonthStartDate, currentMonthEndDate);
				result[stage].Add(new CompletionSpeedDataItem
				{
					AbbreviatedMonth = iterator.ToString("MMM"),
					AccompanyingFileCount = count
				});
			}
		}

		return result;
	}

	private static int GetCountForPhase(List<AccompanyingFile> files, AccompanyingFileStage stage, DateTime monthStart, DateTime monthEnd)
	{
		return files.Count(af => IsFileInPhase(af, stage, monthStart, monthEnd));
	}

	private static bool IsFileInPhase(AccompanyingFile file, AccompanyingFileStage stage, DateTime monthStart, DateTime monthEnd)
	{
		return stage switch
		{
			AccompanyingFileStage.Identify => file.OpeningDate <= monthEnd &&
								   (file.IdentifySynthesisValidationDate == null || file.IdentifySynthesisValidationDate >= monthStart),
			AccompanyingFileStage.OrganizingAndFinancing => file.IdentifySynthesisValidationDate <= monthEnd &&
								   (file.OrganizeAndFinanceSynthesisValidationDate == null || file.OrganizeAndFinanceSynthesisValidationDate >= monthStart),
			AccompanyingFileStage.RealisationAndFollowing => file.OrganizeAndFinanceSynthesisValidationDate <= monthEnd &&
								   (file.RealizeAndFollowSynthesisValidationDate == null || file.RealizeAndFollowSynthesisValidationDate >= monthStart),
			_ => false
		};
	}
}