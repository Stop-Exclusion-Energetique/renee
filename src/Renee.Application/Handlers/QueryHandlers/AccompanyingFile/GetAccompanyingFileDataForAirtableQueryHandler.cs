using System.Text;
using Renee.Application.Abstraction.Query;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileDataForAirtableQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileDataForAirtableQuery,
		ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>> HandleQuery(
		GetAccompanyingFileDataForAirtableQuery request)
	{
		try
		{
			var user = await userRepository.GetUserById(request.UserId);

			if (user == null)
				return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Failure(
					Labels.Errors.UnhandledErrorOccured);

			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileForAirtable(request.AccompanyingFileReference);

			if (accompanyingFile == null)
				return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Failure(
					GenerateFilesLabels.Errors.AccompanyingFileNotFound);

			if (accompanyingFile.AccompanyingFileMilestone < (int)AccompanyingFileStage.RealisationAndFollowing)
				return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Failure(
					Labels.Errors.AccompanyingFileIsNotInThirdStage);

			if (IsUserAllowedToCreateAirtableRecord(accompanyingFile, user))
			{
				var objectResultValue = new GetAccompanyingFileDataForAirtableQueryObjectResult
				{
					NumberOfPeopleInHousehold =
						accompanyingFile.AccompanyingFileHouseholdNavigation.SecondaryOccupants.Count + 1,
					MainOccupantLastName =
						accompanyingFile.AccompanyingFileHouseholdNavigation.MainOccupantNavigation.LastName ??
						string.Empty,
					HousingPostalCode =
						int.TryParse(
							accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.PostalCode,
							out var postalCode)
							? postalCode
							: 0,
					HousingCity = accompanyingFile.AccompanyingFileHousingNavigation.HousingAddressNavigation.City,
					IncomeTaxReference = accompanyingFile.AccompanyingFileHouseholdNavigation.ReferenceIncomeTax,
					HouseholdResourcesTypology =
						BuildHouseholdResourcesTypology(
							accompanyingFile.AccompanyingFileHouseholdNavigation.HouseholdResources),
					HouseholdSocialSituation =
						accompanyingFile.AccompanyingFileHouseholdNavigation.SocialContext ?? string.Empty,
					HousingType =
						((HousingType)accompanyingFile.AccompanyingFileHousingNavigation.HousingType!)
						.GetDescription(),
					HousingYearOfConstruction = CalculateConstructionYear((HousingYearConstruction)accompanyingFile.AccompanyingFileHousingNavigation.ConstructionYear!),
					HousingSurface = accompanyingFile.AccompanyingFileHousingNavigation.LivingSpace,
					EnergeticPerformanceBeforeRenovation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.AnnualEnergyConsumption,
					EnergeticPerformanceAfterRenovation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedAnnualEnergyConsumptionAfterWork,
					GesEmissionBeforeRenovation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.AnnualGesemission,
					GesEmissionAfterRenovation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedAnnualGesemissionsAfterWork,
					DpeLabelBeforeRenovation =
						((DpeLabel)accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.Dpe!).GetDescription(),
					DpeLabelAfterRenovation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedDpeafterWork is not null
							? ((DpeLabel)accompanyingFile.AccompanyingFileHousingNavigation
								.HousingAfterWorkStateNavigation.EstimatedDpeafterWork).GetDescription()
							: string.Empty,
					TotalDevisValue =
						CalculateTotalWorkPackageCost(
							accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorkPackages),
					AnahTotalAid =
						CalculateAnahTotalAid(accompanyingFile.AccompanyingFilePreFinancingPlanNavigation),
					RegionAid = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.RegionalAids,
					DepartmentAid = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids,
					CityAid =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.PublicEstablishmentsForInterCommunalCooperationAids,
					CommunityAid = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids,
					CeeAid = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.CeeFinancing,
					UnderprivilegedHousingFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation,
					SocialProtectionGroupAid =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup,
					MdphAid = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MdphFinancing,
					StopFoundAidAsked =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.EstimatedRemainingAmount,
					HouseholdMaximumSavingsForRenovation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.HouseholdMaximumSavingAmountForRenovationProject,
					HouseholdMaximalFamilyAid =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
					TypeOfBankLoanRequested =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SolicitedBankLoanType ??
						string.Empty,
					EnergyDepravation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.EnergyDepravation is not null
							? ((EnergyDeprivation)accompanyingFile.AccompanyingFileHousingNavigation
								.HousingInitialStateNavigation.EnergyDepravation).GetDescription()
							: string.Empty,
					FamilyMonthlyIncome =
						CalculateHouseholdMonthlyEarned(accompanyingFile.AccompanyingFileHouseholdNavigation),
					IsInStopProgram = accompanyingFile.ZeroEnergyExclusionTerritoriesProgram,
					WattForChangeAIds = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
						.WattForChangeFoundation
				};

				return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Success(
					objectResultValue,
					null);
			}

			return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Failure(
				Labels.Errors.UserNotAllowedToCreateAirtableRecord);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>.Failure(
				Labels.Errors.SystemError);
		}
	}

	private static string BuildHouseholdResourcesTypology(IEnumerable<HouseholdResource> householdResources)
	{
		if (householdResources.Any(hr => hr.HouseholdResourcesNavigation.Labels == EnergyDeprivationLabel.None))
		{
			return "0";
		}

		var householdResourcesTypology = new StringBuilder();

		foreach (var householdResource in householdResources)
		{
			householdResourcesTypology.Append(householdResource.HouseholdResourcesNavigation.Labels);
			householdResourcesTypology.Append(' ');
		}

		return householdResourcesTypology.ToString();
	}

	private static double? CalculateAnahTotalAid(PreFinancingPlan preFinancingPlan) =>
		preFinancingPlan.MaPrimeRenovGuidedPath +
		preFinancingPlan.MaPrimeRenovCoOwnerShip +
		preFinancingPlan.MaPrimeLogementDecent +
		preFinancingPlan.MaPrimeAdapt +
		preFinancingPlan.BonusForExitingEnergeticSieve;

	private static double? CalculateHouseholdMonthlyEarned(Household household) =>
		household.HouseholdResources.Sum(x => x.Value);

	private static double CalculateTotalWorkPackageCost(ICollection<WorkPackage> workPackages)
	{
		double totalCost = 0;

		foreach (var workPackage in workPackages)
			foreach (var workPackageWorkTypeCost in workPackage.WorkPackageWorkTypeCosts)
				totalCost += workPackageWorkTypeCost.Cost;

		return totalCost;
	}

	private static bool
		IsUserAllowedToCreateAirtableRecord(Domain.Entity.AccompanyingFile accompanyingFile, Domain.Entity.User connectedUser) =>
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.SolidarBuilder ||
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder ||
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder ||
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.TargetCoordinator ||
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator ||
		connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.TerritorialBuilder ||
        connectedUser.Id == accompanyingFile.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder ||
		(connectedUser.Role.Name == Constants.StructuralReferentRole &&
		connectedUser.ReportingStructureNavigation?.NationalStructureId == accompanyingFile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation?.NationalStructureId);

	private static int CalculateConstructionYear(HousingYearConstruction housingYearConstruction)
	{
		switch(housingYearConstruction)
		{
			case HousingYearConstruction.HouseConstructionPeriod1:
				return 1948;
			case HousingYearConstruction.HouseConstructionPeriod2:
				return 1961;
			case HousingYearConstruction.HouseConstructionPeriod3:
				return 1976;
			case HousingYearConstruction.HouseConstructionPeriod4:
				return 1980;
			case HousingYearConstruction.HouseConstructionPeriod5:
				return 1986;
			case HousingYearConstruction.HouseConstructionPeriod6:
				return 1995;
			case HousingYearConstruction.HouseConstructionPeriod7:
				return 2003;
			case HousingYearConstruction.HouseConstructionPeriod8:
				return 2009;
			case HousingYearConstruction.HouseConstructionPeriod9:
				return 2017;
			case HousingYearConstruction.HouseConstructionPeriod10:
				return 2021;
			default:
				return 0;
		}
	}

}