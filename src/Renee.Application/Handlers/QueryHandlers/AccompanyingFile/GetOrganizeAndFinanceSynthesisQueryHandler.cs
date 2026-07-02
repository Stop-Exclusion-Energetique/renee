using MediatR;
using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;
using PreWorkPlanProjectType = Renee.Domain.Entity.PreWorkPlanProjectType;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetOrganizeAndFinanceSynthesisQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetOrganizeAndFinanceSynthesisQuery, ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>>
{
	public async Task<ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>> Handle(
		GetOrganizeAndFinanceSynthesisQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFileEntity =
				await accompanyingFileRepository.GetAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(request.Id);

			if (accompanyingFileEntity is null) return ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Failure(Labels.Errors.StageValidationErrorAccompanyingFileNotFound);

			return ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Success(new GetOrganizeAndFinanceSynthesisQueryObjectResult
			{
				Id = accompanyingFileEntity.Id,
				AccompanyingFileReference = accompanyingFileEntity.AccompanyingFileReference,
				SocialProtectionGroup =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup,
				WattForChangeFoundation =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation,
				UnderprivilegedHousingFoundation =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation,
				AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
					((PartlyStateTreatment?)accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation
						.AreExistingHumidityAndVaporMigrationManagedAfterTreatment!).GetDescription(),
				EstimatedAnnualEnergyConsumptionAfterWork =
					accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
						.EstimatedAnnualEnergyConsumptionAfterWork,
				EstimatedEnergyClassJump =
					accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
						.EstimatedDpeclassJump.ToString(),
				EstimatedEnergyDpeAfterWork =
					((DpeLabel?)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
						.EstimatedDpeafterWork).GetDescription(),
				EstimatedRemainingAmount =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.EstimatedRemainingAmount,
				IsEmergencyWorks =
					StringHelper.BoolToString(
						accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation.HasEmergencyWorks),
				NextStepAndVigilancePoints =
					accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation.NextStepAndVigilancePoint,
				Stage = (AccompanyingFileStage)accompanyingFileEntity.AccompanyingFileMilestone,
				Status = (AccompanyingFileStatus)accompanyingFileEntity.AccompanyingFileStatus,

				SolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SolidarBuilder,
				SecondSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilder,
				ThirdSolidarBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilder,
				DiffuseCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator,
				TargetedCoordinator = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TargetCoordinator,
				TerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.TerritorialBuilder,
				SecondTerritorialBuilder = accompanyingFileEntity.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder,

				AdaptationBonus = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MaPrimeAdapt,
				BankLoanType = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.SolicitedBankLoanType,
				ClassicBankLoan =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.ClassicBankLoan,
				CoOwnershipBonus =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovCoOwnerShip,
				DecentHousingBonus =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MaPrimeLogementDecent,
				Department = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids,
				GuidedPathwayBonus =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovGuidedPath,
				IsFinancingAsked =
					StringHelper.BoolToString(
						accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation
							.IsFinancingAskedToStopAssociation),
				LeroyMerlinFoundation =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation,
				RenovationType =
					((RenovationType?)accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation.RenovationType)
					.GetDescription(),
				PreWorkPlanProjectType =
					GetPreWorkPlanProjectType(
						accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes),
				TreatedAirTightness =
					((PartlyStateTreatment?)accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation
						.TreatedAirTightness).GetDescription(),
				TreatedThermalBridge =
					((PartlyStateTreatment?)accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation
						.TreatedThermalBridge).GetDescription(),
				WorkPackageSummary =
					GetWorkPackagesSummary(accompanyingFileEntity.AccompanyingFilePreWorkPlanNavigation.WorkPackages),
				Region = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.RegionalAids,
				ExitEnergySieveBonus =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.BonusForExitingEnergeticSieve,
				DepartmentalHouseForDisabledPersons =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MdphFinancing,
				PensionFund = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.PensionFund,
				Municipality = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids,
				PublicEstablishmentsIntercommunalCooperation =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation
						.PublicEstablishmentsForInterCommunalCooperationAids,
				EnergySavingCertificates =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.CeeFinancing,
				FamilyAllowanceFund = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.CafMsaFinancing,
				FundingModes = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.FundingModes
					.Select(fm => new FundingModeSummary
					{
						Label = fm.Label,
						Value = fm.Value
					}).ToList(),
				InitialDpeLabel = ((DpeLabel)accompanyingFileEntity.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe!).GetDescription(),
				HouseholdMaximumSavingAmountForRenovationProject =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation
						.HouseholdMaximumSavingAmountForRenovationProject,
				MaximumAmountSupportFamilyMembersRenovationProject =
					accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation
						.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
				StopEnergyExclusionFunds = accompanyingFileEntity.AccompanyingFilePreFinancingPlanNavigation.StopEnergyExclusionFunds,
				AnahFolderNumber = accompanyingFileEntity.AnahFolderNumber,
				AnahFolderFilingDate = accompanyingFileEntity.AnahFolderFilingDate
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static string GetPreWorkPlanProjectType(ICollection<PreWorkPlanProjectType> projectTypes)
	{
		return string.Join(", ", projectTypes.Select(pt => pt.ProjectTypeNavigation.Label));
	}

	private static List<WorkPackageSummaryDto> GetWorkPackagesSummary(ICollection<WorkPackage> workPackages)
	{
		var summary = new List<WorkPackageSummaryDto>();

		foreach (var workPackage in workPackages) { 
			var workPackageWorkTypeCosts = workPackage.WorkPackageWorkTypeCosts;
			var currentWorkPackagesWorkTypes = string.Join(
				", ",
				workPackageWorkTypeCosts.Select(wptc => wptc.WorkTypeNavigation.Label));
            var currentWorkPackageTotalCost = workPackageWorkTypeCosts.Sum(wptc => wptc.Cost);

			summary.Add(new WorkPackageSummaryDto(workPackage.Id, currentWorkPackagesWorkTypes, currentWorkPackageTotalCost));
        }

		return summary;
	}
}