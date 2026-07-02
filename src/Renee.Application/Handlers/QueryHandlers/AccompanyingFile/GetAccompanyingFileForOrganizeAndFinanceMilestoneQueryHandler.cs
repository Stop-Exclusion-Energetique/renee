using MediatR;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.DTOs.WorkPackageWorkTypeCost;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAccompanyingFileForOrganizeAndFinanceMilestoneQuery,
		ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>>
{
	public async Task<ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>> Handle(
		GetAccompanyingFileForOrganizeAndFinanceMilestoneQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var accompanyingFile =
				await accompanyingFileRepository.GetAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(request.Id);

			if (accompanyingFile is null)
				return ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

		if (accompanyingFile is not null && (IsUserInSupportTeam(accompanyingFile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole)))
		{
			var copropertyWorkFinance = accompanyingFile.CopropertyProfileNavigation?.CopropertyWorkFinanceNavigation;
			var workPackages = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorkPackages;
			var copropertyWorkPackages = copropertyWorkFinance?.WorkPackages ?? [];

			return ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Success(
				new GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult
				{
					Status = (AccompanyingFileStatus)accompanyingFile.AccompanyingFileStatus,
					Stage = (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone,
					IsImported = accompanyingFile.ImportRunId != null,
                    ReportingStructureName = accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilderNavigation?.ReportingStructureNavigation?.Name,
                    UnderprivilegedHousingFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UnderprivilegedHousingFoundation,
					AdaptationBonus = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeAdapt,
					AraOpeningStatementSent =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.IsAraopeningStatementSent,
					AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
						(PartlyStateTreatment?)accompanyingFile.AccompanyingFilePreWorkPlanNavigation
							.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
					BankLoanType = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SolicitedBankLoanType,
					BayWindowCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfBayWindow,
					CeilingHeight = accompanyingFile.AccompanyingFileHousingNavigation.CeilingHeight,
					AccompanyingTimeDuration = (AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
					ClassicBankLoan = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.ClassicBankLoan,
					CoOwnershipBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovCoOwnerShip,
					DecentHousingBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeLogementDecent,
					Department = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.DepartmentalAids,
					DepartmentalHouseForDisabledPersons =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MdphFinancing,
					DisordersObservedCommentary =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.DisordersObservedCommentary,
					DoorCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfDoor,
					EnergySavingCertificates = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.CeeFinancing,
					EstimatedAnnualEnergyConsumptionAfterWork =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedAnnualEnergyConsumptionAfterWork,
					EstimatedAnnualEnergyConsumptionBeforeWork =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.AnnualEnergyConsumption,
					EstimatedAnnualGhgEmissionsAfterWork =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedAnnualGesemissionsAfterWork,
					EstimatedAnnualGhgEmissionsBeforeWork =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.AnnualGesemission,
					EstimatedEnergyClassJump =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedDpeclassJump,
					InitialDpeLabel	= (DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.Dpe,
					EstimatedEnergyDpeAfterWork =
						(DpeLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedDpeafterWork,
					EstimatedEnergyGesAfterWork =
						(GesLabel?)accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation
							.EstimatedGesafterWork,
					DateOfAgVote = copropertyWorkFinance?.DateOfAgVote,
					ExitEnergySieveBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.BonusForExitingEnergeticSieve,
					FamilyAllowanceFund = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.CafMsaFinancing,
					FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HouseholdAvailabilitiyToOrganizeArasite,
					FamilyCanMobilizeSocialCircleOnConstructionSite =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation
							.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean,
					FamilyPhysicalcCapabilitiesHaveBeenTakenIntoAccount =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation
							.AreHouseholdPhysicalCapacitiesTakenIntoAccount,
					GuidedPathwayBonus =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MaPrimeRenovGuidedPath,
					HasFaultyElectricalSystem =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.HasFaultyElectricalSystem,
					HasHeatingSystem =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.HasHeatingSystem,
					HasHotWaterProduction =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.HasHotWaterProduction,
					HasHousingCover =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.HasHousingCover,
					HasInsulation =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.HasInsulation,
					HasOpenings =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.HasOpenings,
					HasPestOrMold =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation.HasPestOrMold,
					HasVentilationSystem =
						accompanyingFile.AccompanyingFileHousingNavigation.HousingInitialStateNavigation
							.HasVentilationSystem,
					InterestInPossibleSupportedSelfRehabilitationAra =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasInterestInPossibleAraprocess,
					IsEmergencyWorks = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasEmergencyWorks,
					IsEnergeticsRenovationWorks =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasEnergeticsRenovationWorks,
					IsFamilyReadyForSupportedSelfRehabilitationApproach =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasInterestInPossibleAraprocess,
					IsInducedWorks = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasInducedWorks,
					IsRgeLabelUpToDate = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.IsRgeLabelUpToDate,
					IsSafetyAndHealthWorks =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasSafetyAndHealthWorks,
					LeroyMerlinFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.LeroyMerlinFoundation,
					HouseholdMaximumSavingAmountForRenovationProject =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.HouseholdMaximumSavingAmountForRenovationProject,
					MaximumAmountSupportFamilyMembersRenovationProject =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
					MprCoproAids = copropertyWorkFinance?.MprCoproAids,
					Municipality = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.MunicipalityAids,
					NeedTemporaryRehousingSolution =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.HasNeedForTemporaryReHousing,
					NextStepAndVigilancePoints =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.NextStepAndVigilancePoint,
					OtherQualification = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.OtherQualification,
					PatioDoorCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfPatioDoor,
					PensionFund = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.PensionFund,
					PreWorkPlanInsuranceTypes =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanInsuranceTypes
							.Select(pt => pt.InsuranceType).ToList(),
					PreWorkPlanProjectTypes =
						accompanyingFile.AccompanyingFilePreWorkPlanNavigation.PreWorkPlanProjectTypes
							.Select(pt => pt.ProjectType).ToList(),
					PublicEstablishmentsIntercommunalCooperation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation
							.PublicEstablishmentsForInterCommunalCooperationAids,
					Reference = accompanyingFile.AccompanyingFileReference,
					Region = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.RegionalAids,
					RenovationType =
						(RenovationType?)accompanyingFile.AccompanyingFilePreWorkPlanNavigation.RenovationType,
					RoofWindowCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfRoofDoor,
					RoomCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfRoom,
					SocialProtectionGroup =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.SocialProtectionGroup,
					SunExposure = (SunExposure?)accompanyingFile.AccompanyingFileHousingNavigation.SunExposure,
					TreatedAirTightness =
						(PartlyStateTreatment?)accompanyingFile.AccompanyingFilePreWorkPlanNavigation
							.TreatedAirTightness,
					TreatedThermalBridge =
						(PartlyStateTreatment?)accompanyingFile.AccompanyingFilePreWorkPlanNavigation
							.TreatedThermalBridge,
					WattForChangeFoundation =
						accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.WattForChangeFoundation,
					ComplementaryCopropertyAids = copropertyWorkFinance?.ComplementaryAids,
					WindowCounter = accompanyingFile.AccompanyingFileHousingNavigation.NumberOfWindow,
					WorkDetails = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.WorksDetails,
					WorkPackages = workPackages.Select(
						wp => new WorkPackageDto(
							wp.Id,
							wp.EnergeticsEffectAfterWorks,
							wp.WorkPackageWorkTypeCosts.Select(
									wtc => new WorkPackageWorkTypeCostDto(wtc.WorkType, wtc.Cost, wtc.Description))
								.ToList())).ToList(),
					CopropertyWorkPackages = copropertyWorkPackages.Select(
						wp => new WorkPackageDto(
							wp.Id,
							wp.EnergeticsEffectAfterWorks,
							wp.WorkPackageWorkTypeCosts.Select(
									wtc => new WorkPackageWorkTypeCostDto(wtc.WorkType, wtc.Cost, wtc.Description))
								.ToList())).ToList(),
					FundingModes = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.FundingModes
						.Select(fm => new FundingModeDto(fm.Id, fm.Label, fm.Value)).ToList(),
					IsInTzeeProgram = accompanyingFile.ZeroEnergyExclusionTerritoriesProgram ?? false,
					StopEnergyExclusionFunds = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.StopEnergyExclusionFunds,
					StartOfAccompanyingDate = accompanyingFile.StartOfAccompanyingDate,
					AnahFolderNumber = accompanyingFile.AnahFolderNumber,
					AnahFolderFilingDate = accompanyingFile.AnahFolderFilingDate
				}
			);
		}

			return ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static bool IsUserInSupportTeam(Domain.Entity.AccompanyingFile accompanyingFile, Guid userId)
		=> userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.TerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondTerritorialBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.SecondSolidarBuilder ||
			userId == accompanyingFile.AccompanyingFileSupportTeamNavigation?.ThirdSolidarBuilder ;

	private static bool IsUserACoordinatorOrAdmin(string userRole)
		=> userRole.Equals(Constants.DiffuseCoordinatorRole) ||
			userRole.Equals(Constants.TargetedCoordinatorRole) ||
			userRole.Equals(Constants.AdminRole);
}