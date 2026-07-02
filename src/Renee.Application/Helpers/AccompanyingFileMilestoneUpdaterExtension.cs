using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;
using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Application.Helpers;

public static class AccompanyingFileMilestoneUpdaterExtension
{
	public static SaveAndSubmitRealizeAndFollowMilestoneData SaveRealizeAndFollowMilestoneUpdater(
		SaveAccompanyingFileRealizeAndFollowCommandInput request,
		AccompanyingFile accompanyingFile)
	{
		var invoiceChanges = accompanyingFile.UpdateAccompanyingFileForRealizeAndFollowMilestone(
			request.EndOfAccompaniementDate,
			request.EndOfFollowingDate,
			request.AccompanyingTimeDuration,
			request.AnahGrantDate,
			request.ConnectedUserId,
			request.GetUpdatedInvoices());

		accompanyingFile.AccompanyingFileWorkMonitoringNavigation?.UpdateWorkMonitoring(request.CreateWorkMonitoring());
		accompanyingFile.UpdatePreFinancingPlan(request.UpdatedFinancingPlan.CreateUpdatePreFinancingPlan());
		accompanyingFile.SiteSupervision?.UpdateSiteSupervision(request.CreateSiteSupervision());
		accompanyingFile.UpdateHousingAfterWorkStateForRealizeAndFollowMilestone(
			request.UpdatedHousingAfterWorkState.FinalDpe,
			request.UpdatedHousingAfterWorkState.FinalDpeClassJump);

		var fundingModeChanges = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UpdatePreFinancingPlanFundingModes(
			request.GetUpdatedFundingModes());
		var workParticipantChanges = accompanyingFile.SiteSupervision?.UpdateSiteSupervisionWorkParticipants(request.GetUpdatedWorkParticipants());

		return new(
			invoiceChanges,
			fundingModeChanges,
			workParticipantChanges ?? EntityChanges<WorkParticipant>.Empty);
	}

	public static SaveAndSubmitOrganizeAndFinanceMilestoneData SaveOrganizeAndFinanceMilestoneUpdater(
	SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput inputOrganizeAndFinance, AccompanyingFile accompanyingFile)
	{
		accompanyingFile.UpdateHousing(
			inputOrganizeAndFinance.UpdateHousing.CreateUpdateHousing(),
			inputOrganizeAndFinance.UpdatedHousingInitialState.CreateUpdateHousingInitialState(),
			inputOrganizeAndFinance.UpdatedHousingAfterWorkState.CreateUpdateHousingAfterWorkState())
			.UpdatePreWorkPlan(inputOrganizeAndFinance.UpdatePreWorkPlan.CreateUpdatePreWorkPlan())
			.UpdatePreFinancingPlan(inputOrganizeAndFinance.UpdatePreFinancingPlan.CreateUpdatePreFinancingPlan());

		var fundingModeChanges = accompanyingFile.AccompanyingFilePreFinancingPlanNavigation.UpdatePreFinancingPlanFundingModes(
				inputOrganizeAndFinance.GetUpdatedFundingModes());

		var projectTypeChanges = accompanyingFile.AccompanyingFilePreWorkPlanNavigation
			.UpdatePreWorkPlanProjectTypes(inputOrganizeAndFinance.UpdatePreWorkPlan.ProjectTypes);

		var insuranceTypeChanges = accompanyingFile.AccompanyingFilePreWorkPlanNavigation
			.UpdatePreWorkPlanInsuranceTypes(inputOrganizeAndFinance.UpdatePreWorkPlan.InsuranceTypes);

		var workPackagesChanges = accompanyingFile.AccompanyingFilePreWorkPlanNavigation.UpdatePreWorkPlanWorkPackages(
			inputOrganizeAndFinance.GetUpdatedWorkPackages());

		return new(
			projectTypeChanges,
			insuranceTypeChanges,
			workPackagesChanges,
			fundingModeChanges);
	}

	public static SaveAndSubmitIdentificationMilestoneData SaveIdentificationMilestoneUpdater(
		SaveAccompanyingFileIdentificationMilestoneCommandInput inputIdentification, AccompanyingFile accompanyingFile)
	{
		accompanyingFile.UpdateHouseholdForIdentificationMilestone(
			inputIdentification.UpdatedHousehold.CreateUpdateHousehold(),
			inputIdentification.UpdatedHouseholdMainOccupant.CreateUpdateMainOccupant())
			.UpdateHousingForIdentificationMilestone(
				inputIdentification.UpdatedHousing.CreateUpdateHousing(),
				inputIdentification.UpdatedAddress.CreateUpdateAddress(),
				inputIdentification.UpdatedHousingInitialState.CreateUpdateHousingInitialState());

		var secondaryOccupantChanges =
			accompanyingFile.AccompanyingFileHouseholdNavigation.UpdateHouseholdSecondaryOccupants(
				inputIdentification.GetUpdatedSecondaryOccupants());

		var expenseChanges = accompanyingFile
			.AccompanyingFileHouseholdNavigation.UpdateHouseholdExpense(inputIdentification.GetUpdatedHouseholdExpenses());

		var resourceChanges = accompanyingFile
			.AccompanyingFileHouseholdNavigation.UpdateHouseholdResources(inputIdentification.GetUpdatedHouseholdResources());

		var householdDifficultiesChanges = accompanyingFile
			.AccompanyingFileHouseholdNavigation.UpdateHouseholdDifficulties(inputIdentification.UpdatedHouseholdDifficulties);

		var heatingEnergyChanges = accompanyingFile
			.AccompanyingFileHouseholdNavigation.UpdateHouseholdHeatingEnergy(inputIdentification.GetUpdatedHouseholdHeatingEnergy());

		return new(
			secondaryOccupantChanges,
			expenseChanges,
			resourceChanges,
			householdDifficultiesChanges,
			heatingEnergyChanges);
	}

	public static WorkMonitoring CreateWorkMonitoring(
		UpdatedProjectCost projectCost,
		UpdatedWorkSummary workSummary,
		UpdatedEvaluation evaluation)
	{
		return new WorkMonitoring
		{
			AccompanyingCost = projectCost.AccompanyingCost,
			WorkTotalCost = projectCost.WorkTotalCost,
			HouseholdSelfFinancing = projectCost.HouseholdAutoFinancing,
			IntermediateAirtightnessTestResult = workSummary.IntermediateAirtightnessTestResult,
			JustificationAndActionsPutInPlaceIfNoTest = workSummary.WaterproofingTreatmentActions,
			HasEffectiveComplianceWithWorkRecommendations = workSummary.HasEffectiveComplianceWithWorkRecommendations,
			HasWorksEnabledHouseholdToStayAtHome = workSummary.HasWorkEnablingHomeSupport,
			WellBeingRating = evaluation.WellBeing,
			EducationalFrameworkRating = evaluation.EducationnalFramework,
			FamilySatisfaction = evaluation.FamilySatisfaction,
			ReturnToEmployment = evaluation.IsBackToEmployment,
			HasHousingAdaptationWorks = workSummary.HasHousingAdaptationWorks,
			HasFinishingWorks = workSummary.HasFinishingWorks,
			HasSafetyWorks = workSummary.HasSafetyWorks,
			HasPreparationWorks = workSummary.HasPreparationWorks,
			HasEmergencyWorks = workSummary.HasEmergencyWorks,
			HasUnsanitaryExit = workSummary.HasUnsanitaryExit,
			TreatedAirTightness = workSummary.TreatedAirTightness,
			TreatedThermalBridges = workSummary.TreatedThermalBridges,
			HasHumidityManagement = workSummary.HasHumidityManagement
		};
	}

	public static UpdateHousing ImportToUpdateHousing(ParsedCsvData dataLine, Housing housing) =>
		new(
			dataLine.NumberOfRoom ?? housing.NumberOfRoom,
			dataLine.NumberOfDoor ?? housing.NumberOfDoor,
			dataLine.NumberOfWindow ?? housing.NumberOfWindow,
			dataLine.NumberOfPatioDoor ?? housing.NumberOfPatioDoor,
			dataLine.NumberOfRoofDoor ?? housing.NumberOfRoofDoor,
			dataLine.NumberOfBayWindow ?? housing.NumberOfBayWindow,
			dataLine.CeilingHeight ?? housing.CeilingHeight,
			dataLine.SunExposure ?? (SunExposure?)housing.SunExposure
			);

	public static UpdatedHousingInitialStateForOrganizeAndFinanceMilestone ImportToUpdateHousingInitialStateForOrganizeAndFinanceMilestone(
		ParsedCsvData dataLine, HousingInitialState housingInitialState) =>
		new(
			dataLine.HasPestOrMold ?? housingInitialState.HasPestOrMold,
			dataLine.HasFaultyElectricalSystem ?? housingInitialState.HasFaultyElectricalSystem,
			dataLine.HasVentilationSystem ?? housingInitialState.HasVentilationSystem,
			dataLine.HasHeatingSystem ?? housingInitialState.HasHeatingSystem,
			dataLine.HasHotWaterProduction ?? housingInitialState.HasHotWaterProduction,
			dataLine.HasOpenings ?? housingInitialState.HasOpenings,
			housingInitialState.HasInsulation,
			dataLine.HasHousingCover ?? housingInitialState.HasHousingCover,
			dataLine.DisordersObservedCommentary ?? housingInitialState.DisordersObservedCommentary,
			dataLine.Dpe ?? (DpeLabel?)housingInitialState.Dpe
		);

	public static UpdateHousingAfterWorkState ImportToUpdateHousingAfterworkState(ParsedCsvData dataLine, HousingAfterWorkState housingAfterWorkState) =>
		new(
			dataLine.EstimatedAnnualEnergyConsumptionAfterWork ?? housingAfterWorkState.EstimatedAnnualEnergyConsumptionAfterWork,
			dataLine.EstimatedAnnualGesEmissionsAfterWork ?? housingAfterWorkState.EstimatedAnnualGesemissionsAfterWork,
			dataLine.EstimatedDpeAfterWork ?? (DpeLabel?)housingAfterWorkState.EstimatedDpeafterWork,
			dataLine.EstimatedGesAfterWork ?? (GesLabel?)housingAfterWorkState.EstimatedGesafterWork,
			AccompanyingFileHelper.CalculateEnergeticClassJump(dataLine.Dpe, dataLine.EstimatedDpeAfterWork)
		);

	public static UpdatePreWorkPlan ImportToUpdatePreWorkPlan(ParsedCsvData dataLine,
		List<Guid> projectTypeIds, List<Guid> insuranceTypeIds, PreWorkPlan preWorkPlan) =>
		new(
			dataLine.RenovationType ?? (RenovationType?)preWorkPlan.RenovationType,
			dataLine.NextStepAndVigilancePoint ?? preWorkPlan.NextStepAndVigilancePoint,
			dataLine.HasNeedForTemporaryReHousing ?? preWorkPlan.HasNeedForTemporaryReHousing,
			dataLine.HasInterestInPossibleARAProcess ?? preWorkPlan.HasInterestInPossibleAraprocess,
			preWorkPlan.IsAraopeningStatementSent,
			dataLine.HasEmergencyWorks ?? preWorkPlan.HasEmergencyWorks,
			dataLine.HasEnergeticsRenovationWorks ?? preWorkPlan.HasEnergeticsRenovationWorks,
			dataLine.HasInducedWorks ?? preWorkPlan.HasInducedWorks,
			dataLine.HasSafetyAndHealthWorks ?? preWorkPlan.HasSafetyAndHealthWorks,
			dataLine.TreatedAirTightness ?? (PartlyStateTreatment?)preWorkPlan.TreatedAirTightness,
			dataLine.TreatedThermalBridge ?? (PartlyStateTreatment?)preWorkPlan.TreatedThermalBridge,
			dataLine.AreExistingHumidityAndVaporMigrationManagedAfterTreatment ?? (PartlyStateTreatment?)preWorkPlan.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			dataLine.IsRgeLabelUpToDate ?? preWorkPlan.IsRgeLabelUpToDate,
			preWorkPlan.IsHouseholdReadyToStartAraprocess,
			dataLine.AreHouseholdPhysicalCapacitiesTakenIntoAccount ?? preWorkPlan.AreHouseholdPhysicalCapacitiesTakenIntoAccount,
			dataLine.DoHouseholdCanMobilizeSocialCircleOnConstructionSite ?? preWorkPlan.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean,
			dataLine.WorkDetails ?? preWorkPlan.WorksDetails,
			dataLine.HouseholdAvailabilitiyToOrganizeARASite ?? preWorkPlan.HouseholdAvailabilitiyToOrganizeArasite,
			projectTypeIds,
			insuranceTypeIds
		);

    public static UpdatePreFinancingPlan ImportToUpdatePreFinancingPlan(ParsedCsvData dataLine, PreFinancingPlan preFinancingPlan) =>
        new(
            dataLine.MaPrimeRenovGuidedPath ?? preFinancingPlan.MaPrimeRenovGuidedPath,
            dataLine.MaPrimeRenovCoOwnerShip ?? preFinancingPlan.MaPrimeRenovCoOwnerShip,
            dataLine.MaPrimeLogementDecent ?? preFinancingPlan.MaPrimeLogementDecent,
            dataLine.MaPrimeAdapt ?? preFinancingPlan.MaPrimeAdapt,
            dataLine.BonusForExitingEnergeticSieve ?? preFinancingPlan.BonusForExitingEnergeticSieve,
            dataLine.RegionalAids ?? preFinancingPlan.RegionalAids,
            dataLine.DepartmentalAids ?? preFinancingPlan.DepartmentalAids,
            dataLine.PublicEstablishmentsForInterCommunalCooperationAids ?? preFinancingPlan.PublicEstablishmentsForInterCommunalCooperationAids,
            dataLine.MunicipalityAids ?? preFinancingPlan.MunicipalityAids,
            dataLine.SolicitedBankLoanType ?? preFinancingPlan.SolicitedBankLoanType,
            dataLine.ClassicBankLoan ?? preFinancingPlan.ClassicBankLoan,
            dataLine.MdphFinancing ?? preFinancingPlan.MdphFinancing,
            dataLine.CeeFinancing ?? preFinancingPlan.CeeFinancing,
            dataLine.HouseholdMaximumSavingAmountForRenovationProject ?? preFinancingPlan.HouseholdMaximumSavingAmountForRenovationProject,
            dataLine.CafMsaFinancing ?? preFinancingPlan.CafMsaFinancing,
            dataLine.PensionFund ?? preFinancingPlan.PensionFund,
            dataLine.UnderprivilegedHousingFoundation ?? preFinancingPlan.UnderprivilegedHousingFoundation,
            dataLine.LeroyMerlinFoundation ?? preFinancingPlan.LeroyMerlinFoundation,
            dataLine.WattForChangeFoundation ?? preFinancingPlan.WattForChangeFoundation,
            dataLine.SocialProtectionGroup ?? preFinancingPlan.SocialProtectionGroup,
            dataLine.StopEnergyExclusionFunds ?? preFinancingPlan.StopEnergyExclusionFunds,
            preFinancingPlan.FundingModes.Select(fm =>
                new FundingModeDto(fm.Id, fm.Label, fm.Value)).ToList(),
            dataLine.OtherFamilyMemberMaximumSupportAmountForRenovationProject ?? preFinancingPlan.OtherFamilyMemberMaximumSupportAmountForRenovationProject
        );

	public static UpdatedHousehold ImportToUpdateHousehold(ParsedCsvData dataLine, Household household) =>
		new(
			dataLine.HouseholdTypology ?? (HouseholdTypology?)household.HouseholdTypology,
			dataLine.IsFollowedByAnSocialWorker ?? household.IsFollowedByAnSocialWorker,
			dataLine.HasAnOccupantWithDisabilities ?? household.HasAnOccupantWithDisabilities,
			dataLine.HasAnOccupantWithLongTermIllness ?? household.HasAnOccupantWithLongTermIllness,
			dataLine.HasAnOccupantWithIndependenceLoss ?? household.HasAnOccupantWithIndependenceLoss,
			dataLine.HasAnOccupantUnderCuratorship ?? household.HasAnOccupantUnderCuratorship,
			dataLine.HasAnOccupantUnderGuardianship ?? household.HasAnOccupantUnderGuardianship,
			(float?)dataLine.ReferenceIncomeTax ?? (float?)household.ReferenceIncomeTax,
			household.AnahCategory,
			dataLine.SocialContext ?? household.SocialContext,
			dataLine.HouseholdProject ?? household.HouseholdProject,
			dataLine.HouseholdAvailabilityForVisits ?? household.HouseholdAvailabilityForVisits,
			dataLine.CommentsOnHouseholdDifficulties ?? household.CommentsOnHouseholdDifficulties,
			dataLine.HasOverdueInvoice ?? household.HasOverdueInvoice,
			household.EnergyEffortRate
		);

	public static UpdatedHouseholdMainOccupant ImportToUpdateHouseholdMainOccupant(
		ParsedCsvData dataLine,
		MainOccupant mainOccupant) =>
		new(
			mainOccupant.Id,
			AccompanyingFileHelper.GenerateTrigram(dataLine.FirstName ?? mainOccupant.FirstName, dataLine.LastName ?? mainOccupant.LastName),
			dataLine.Birthdate ?? mainOccupant.Birthdate,
			AccompanyingFileHelper.CalculateAge(dataLine.Birthdate ?? mainOccupant.Birthdate),
			dataLine.SocioProfessionalCategory ?? (SocioProfessionalCategory?)mainOccupant.SocioProfessionalCategory,
			dataLine.PhoneNumber ?? mainOccupant.PhoneNumber,
			dataLine.Email ?? mainOccupant.Email,
			dataLine.Profession ?? mainOccupant.Job,
			dataLine.SocialProtectionFund ?? (SocialProtectionFund?)mainOccupant.SocialProtectionFund,
			dataLine.CommentOnSocialProtectionFund ?? mainOccupant.CommentOnSocialProtectionFund,
			dataLine.PensionFundOccupant ?? (PensionFund?)mainOccupant.PensionFund,
			dataLine.CommentOnPensionFund ?? mainOccupant.CommentOnPensionFund,
			dataLine.AdditionnalFund ?? (AdditionalFund?)mainOccupant.AdditionnalFund,
			dataLine.CommentOnAdditionnalFund ?? mainOccupant.CommentOnAdditionnalFund,
			dataLine.FirstName ?? mainOccupant.FirstName,
			dataLine.LastName ?? mainOccupant.LastName
		);

	public static UpdatedHousing ImportToUpdatedHousing(ParsedCsvData dataLine, Housing housing) =>
		new(
			dataLine.GeographicAreaTypology ?? (GeographicalHousingAreaTypology?)housing.GeographicAreaTypology,
			dataLine.IsInABFArea ?? housing.IsInAbfarea,
			dataLine.ArchitecturalNorms ?? housing.ArchitecturalOrTownPlanningStandards,
			dataLine.OwnershipStatus ?? (OwnershipStatus?)housing.OwnershipStatus,
			dataLine.HousingType ?? (HousingType?)housing.HousingType,
			dataLine.ConstructionYear ?? (HousingYearConstruction?)housing.ConstructionYear,
			dataLine.LivingSpace ?? housing.LivingSpace,
			dataLine.NumberOfRoom ?? housing.NumberOfRoom,
			dataLine.NumberOfFloor ?? housing.NumberOfFloor,
			dataLine.YearOfAcquisitionOrEntry ?? housing.YearOfAcquisitionOrEntry,
			dataLine.CadastralReference ?? housing.CadastralReference,
			dataLine.HasPreviousWork ?? housing.HasPreviousWork,
			dataLine.CommentOnPreviousWork ?? housing.CommentOnPreviousWork
		);

	public static UpdatedAddress ImportToUpdateAddress(ParsedCsvData dataLine, Address address) =>
		new(
			address.Id,
			dataLine.Label ?? address.Label,
			dataLine.PostalCode ?? address.PostalCode,
			dataLine.City ?? address.City,
			dataLine.Department ?? address.Department,
			dataLine.Region ?? address.Region,
			dataLine.AdditionnalComment ?? address.AdditionnalComment
		);

	public static UpdatedHousingInitialStateForIdentificationMilestone ImportToUpdateInitialStateForIdentificationMilestone(
		ParsedCsvData dataLine, HousingInitialState housingInitialState) =>
		new(
			dataLine.DegradationIndex ?? (DegradationIndex?)housingInitialState.DegradationIndex,
			dataLine.UnsanitaryCoefficient ?? (UnsanitaryCoefficient?)housingInitialState.UnsanitaryCoefficient,
			dataLine.SummerThermalComfortLevel ?? (ComfortLevel?)housingInitialState.SummerThermalComfortLevel,
			dataLine.WinterThermalComfortLevel ?? (ComfortLevel?)housingInitialState.WinterThermalComfortLevel,
			dataLine.NoiseComfortLevel ?? (ComfortLevel?)housingInitialState.NoiseComfortLevel,
			dataLine.AnnualEnergyConsumption ?? housingInitialState.AnnualEnergyConsumption,
			dataLine.AnnualGesEmission ?? housingInitialState.AnnualGesemission,
			dataLine.EnergyDepravation ?? (EnergyDeprivation?)housingInitialState.EnergyDepravation,
			dataLine.Dpe ?? (DpeLabel?)housingInitialState.Dpe,
			dataLine.Ges ?? (GesLabel?)housingInitialState.Ges
		);

	public static UpdatedProjectCost ImportToUpdateProjectCost(ParsedCsvData dataLine, WorkMonitoring workMonitoring) =>
		new(
			dataLine.AccompanyingCost ?? workMonitoring.AccompanyingCost,
			dataLine.HouseholdSelfFinancing ?? workMonitoring.HouseholdSelfFinancing,
			dataLine.WorkTotalCost ?? workMonitoring.WorkTotalCost
		);

	public static UpdatedWorkSummary ImportToUpdateWorkSummary(ParsedCsvData dataLine, WorkMonitoring workMonitoring) =>
		new(
			dataLine.IntermediateAirtightnessTestResult ?? workMonitoring.IntermediateAirtightnessTestResult,
			dataLine.JustificationAndActionsPutInPlaceIfNoTest ?? workMonitoring.JustificationAndActionsPutInPlaceIfNoTest,
			dataLine.HasEffectiveComplianceWithWorkRecommendations ?? workMonitoring.HasEffectiveComplianceWithWorkRecommendations,
			null,
			dataLine.HasWorksEnabledHouseholdToStayAtHome ?? workMonitoring.HasWorksEnabledHouseholdToStayAtHome,
			dataLine.HasHousingAdaptationWorks ?? workMonitoring.HasHousingAdaptationWorks,
			dataLine.HasFinishingWorks ?? workMonitoring.HasFinishingWorks,
			dataLine.HasSafetyWorks ?? workMonitoring.HasSafetyWorks,
			dataLine.HasPreparationWorks ?? workMonitoring.HasPreparationWorks,
			dataLine.HasEmergencyWorks ?? workMonitoring.HasEmergencyWorks,
			dataLine.HasUnsanitaryExit ?? workMonitoring.HasUnsanitaryExit,
			(int?)dataLine.TreatedAirTightness ?? workMonitoring.TreatedAirTightness,
			(int?)dataLine.TreatedThermalBridge ?? workMonitoring.TreatedThermalBridges,
			dataLine.HasHumidityManagement ?? workMonitoring.HasHumidityManagement
		);

	public static UpdatedEvaluation ImportToUpdateEvaluation(ParsedCsvData dataLine, WorkMonitoring workMonitoring) =>
		new(
			dataLine.WellBeingRating ?? workMonitoring.WellBeingRating,
			dataLine.EducationalFrameworkRating ?? workMonitoring.EducationalFrameworkRating,
			dataLine.FamilySatisfaction ?? workMonitoring.FamilySatisfaction,
			dataLine.ReturnToEmployment ?? workMonitoring.ReturnToEmployment
		);
}