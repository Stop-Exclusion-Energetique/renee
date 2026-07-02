using Radzen;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.DTOs.WorkPackageWorkTypeCost;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.HousingInitialState.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.
	FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.SupportedSelfRehabilitation.
	ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.Presenter;

public class OrganizeAndFinancePresenter
{
	private OrganizeAndFinanceViewModel? _viewModel;
	private string? _errorMessage;

	public OrganizeAndFinancePresenter FromQuery(
		ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult> queryObjectResult)
	{
		if (queryObjectResult.IsSuccess)
		{
			var resultValue = queryObjectResult.Value;

			_viewModel = new OrganizeAndFinanceViewModel
			{
				Reference = resultValue!.Reference,
				AccompanyingFileStage = resultValue.Stage,
				AccompanyingFileStatus = resultValue.Status,
				IsInTzeeProgram = resultValue.IsInTzeeProgram,
				IsImported = resultValue.IsImported,
				ReportingStructureName = resultValue.ReportingStructureName,
				StartOfAccompanyingDate = resultValue.StartOfAccompanyingDate,
				InitialHousingStateViewModel =
				new InitialHousingStateViewModel
				{
					BayWindowCounter = resultValue.BayWindowCounter,
					CeilingHeight = resultValue.CeilingHeight,
					DisordersObservedCommentary = resultValue.DisordersObservedCommentary,
					DoorCounter = resultValue.DoorCounter,
					HasFaultyElectricalSystem = resultValue.HasFaultyElectricalSystem,
					HasHeatingSystem = resultValue.HasHeatingSystem,
					HasHotWaterProduction = resultValue.HasHotWaterProduction,
					HasHousingCover = resultValue.HasHousingCover,
					HasInsulation = resultValue.HasInsulation,
					HasOpenings = resultValue.HasOpenings,
					HasPestOrMold = resultValue.HasPestOrMold,
					HasVentilationSystem = resultValue.HasVentilationSystem,
					PatioDoorCounter = resultValue.PatioDoorCounter,
					RoofWindowCounter = resultValue.RoofWindowCounter,
					RoomCounter = resultValue.RoomCounter,
					SunExposure = resultValue.SunExposure,
					WindowCounter = resultValue.WindowCounter
				},
				PreWorkPlanTabViewModel =
				new PreWorkPlanTabViewModel
				{
					AraOpeningStatementSent = resultValue.AraOpeningStatementSent,
					AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
						resultValue.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
					EstimatedAnnualEnergyConsumptionAfterWork =
						resultValue.EstimatedAnnualEnergyConsumptionAfterWork,
					EstimatedAnnualEnergyConsumptionBeforeWork =
						resultValue.EstimatedAnnualEnergyConsumptionBeforeWork,
					EstimatedAnnualGhgEmissionsAfterWork =
						resultValue.EstimatedAnnualGhgEmissionsAfterWork,
					EstimatedAnnualGhgEmissionsBeforeWork =
						resultValue.EstimatedAnnualGhgEmissionsBeforeWork,
					EstimatedEnergyClassJump = resultValue.EstimatedEnergyClassJump,
					EstimatedEnergyDpeAfterWork = resultValue.EstimatedEnergyDpeAfterWork,
					EstimatedEnergyGesAfterWork = resultValue.EstimatedEnergyGesAfterWork,
					InterestInPossibleSupportedSelfRehabilitationAra =
						resultValue.InterestInPossibleSupportedSelfRehabilitationAra,
					IsEmergencyWorks = resultValue.IsEmergencyWorks,
					IsEnergeticsRenovationWorks = resultValue.IsEnergeticsRenovationWorks,
					IsInducedWorks = resultValue.IsInducedWorks,
					IsRgeLabelUpToDate = resultValue.IsRgeLabelUpToDate,
					IsSafetyAndHealthWorks = resultValue.IsSafetyAndHealthWorks,
					NeedTemporaryRehousingSolution = resultValue.NeedTemporaryRehousingSolution,
					NextStepAndVigilancePoints = resultValue.NextStepAndVigilancePoints,
					OtherQualification = resultValue.OtherQualification,
					PreWorkPlanInsuranceTypes = resultValue.PreWorkPlanInsuranceTypes ?? [],
					PreWorkPlanProjectTypes = resultValue.PreWorkPlanProjectTypes ?? [],
					RenovationType = resultValue.RenovationType,
					TreatedAirTightness = resultValue.TreatedAirTightness,
					TreatedThermalBridge = resultValue.TreatedThermalBridge,
					WorkPackages =
						resultValue.WorkPackages?.Select(CreateWorkPackageViewModel).ToList() ?? [],
					EstimatedEnergyDpeBeforeWork = resultValue.InitialDpeLabel
				},
				PreFinancingPlanViewModel =
				new PreFinancingPlanViewModel
				{
					DateOfAgVote = resultValue.DateOfAgVote,
					MprCoproAids = resultValue.MprCoproAids,
					ComplementaryCopropertyAids = resultValue.ComplementaryCopropertyAids,
					CopropertyWorkPackages =
						resultValue.CopropertyWorkPackages?.Select(CreateWorkPackageViewModel).ToList() ?? [],
					UnderprivilegedHousingFoundation = resultValue.UnderprivilegedHousingFoundation,
					AdaptationBonus = resultValue.AdaptationBonus,
					BankLoanType = resultValue.BankLoanType,
					ClassicBankLoan = resultValue.ClassicBankLoan,
					CoOwnershipBonus = resultValue.CoOwnershipBonus,
					DecentHousingBonus = resultValue.DecentHousingBonus,
					Department = resultValue.Department,
					DepartmentalHouseForDisabledPersons = resultValue.DepartmentalHouseForDisabledPersons,
					EnergySavingCertificates = resultValue.EnergySavingCertificates,
					ExitEnergySieveBonus = resultValue.ExitEnergySieveBonus,
					FamilyAllowanceFund = resultValue.FamilyAllowanceFund,
					GuidedPathwayBonus = resultValue.GuidedPathwayBonus,
					LeroyMerlinFoundation = resultValue.LeroyMerlinFoundation,
					HouseholdMaximumSavingAmountForRenovationProject =
						resultValue.HouseholdMaximumSavingAmountForRenovationProject,
					MaximumAmountSupportFamilyMembersRenovationProject =
						resultValue.MaximumAmountSupportFamilyMembersRenovationProject,
					Municipality = resultValue.Municipality,
					PensionFund = resultValue.PensionFund,
					PublicEstablishmentsIntercommunalCooperation =
						resultValue.PublicEstablishmentsIntercommunalCooperation,
					Region = resultValue.Region,
					SocialProtectionGroup = resultValue.SocialProtectionGroup,
					WattForChangeFoundation = resultValue.WattForChangeFoundation,
					StopEnergyExclusionFunds = resultValue.StopEnergyExclusionFunds,
					FundingModes =
						resultValue.FundingModes?.Select(
								fm => new FundingModeViewModel { Id = fm.Id, Name = fm.Label, Amount = fm.Value })
							.ToList() ?? []
				},
				SupportedSelfRehabilitationViewModel = 
				new SupportedSelfRehabilitationViewModel
				{
					AccompanyingTimeDuration = resultValue.AccompanyingTimeDuration,
					FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite =
						resultValue.FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite,
					FamilyCanMobilizeSocialCircleOnConstructionSite =
						resultValue.FamilyCanMobilizeSocialCircleOnConstructionSite,
					FamilyPhysicalCapabilitiesHaveBeenTakenIntoAccount =
						resultValue.FamilyPhysicalcCapabilitiesHaveBeenTakenIntoAccount,
					IsFamilyReadyForSupportedSelfRehabilitationApproach =
						resultValue.IsFamilyReadyForSupportedSelfRehabilitationApproach,
					WorkDetails = resultValue.WorkDetails,
					AnahFolderNumber = resultValue.AnahFolderNumber,
					AnahFolderFilingDate = resultValue.AnahFolderFilingDate
				}
			};
		}

		if(!queryObjectResult.IsSuccess)
			_errorMessage = queryObjectResult.Message;

		return this;
	}

	public (OrganizeAndFinanceViewModel?, string?) Present()
	{
		return (_viewModel, _errorMessage);
	}

	private static WorkPackageViewModel CreateWorkPackageViewModel(WorkPackageDto dto) =>
		new()
		{
			Id = dto.Id,
			EnergeticsEffectOfWorks = dto.EnergeticsEffectOfWorks,
			WorkTypes = dto.TypeCostDtos?.Select(CreateWorkType).ToList() ?? []
		};

	private static WorkPackageViewModel.WorkType CreateWorkType(WorkPackageWorkTypeCostDto dto) =>
		new(dto.Id, string.Empty, false) { Description = dto.Description, Price = dto.Cost };
}