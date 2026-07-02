using MediatR;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput(
	Guid AccompanyingFileId,
	AccompanyingTimeDuration? AccompanyingTimeDuration,
	UpdateHousing UpdateHousing,
	UpdatedHousingInitialStateForOrganizeAndFinanceMilestone UpdatedHousingInitialState,
	UpdateHousingAfterWorkState UpdatedHousingAfterWorkState,
	UpdatePreWorkPlan UpdatePreWorkPlan,
	UpdatePreFinancingPlan UpdatePreFinancingPlan,
	List<UpdateWorkPackage> UpdatedWorkPackages,
	List<UpdateFundingModes> UpdatedFundingModes,
	Guid ConnectedUserId,
	string? AnahFolderNumber,
	DateTime? AnahFolderFilingDate) : IRequest<ReneeOperationResult<bool>>
{
	public List<WorkPackage> GetUpdatedWorkPackages() =>
		UpdatedWorkPackages.Select(uwp => uwp.CreateUpdatedWorkPackage()).ToList();

	public List<FundingMode> GetUpdatedFundingModes() =>
		UpdatedFundingModes.Select(ufm => ufm.CreateUpdateFundingMode()).ToList();
}

public record UpdateHousing(
	int? RoomCounter,
	int? DoorCounter,
	int? WindowCounter,
	int? PatioDoorCounter,
	int? RoofWindowCounter,
	int? BayWindowCounter,
	double? CeilingHeight,
	SunExposure? SunExposure)
{
	public Housing CreateUpdateHousing() =>
		new()
		{
			NumberOfRoom = RoomCounter,
			NumberOfDoor = DoorCounter,
			NumberOfWindow = WindowCounter,
			NumberOfPatioDoor = PatioDoorCounter,
			NumberOfRoofDoor = RoofWindowCounter,
			NumberOfBayWindow = BayWindowCounter,
			CeilingHeight = CeilingHeight,
			SunExposure = (int?)SunExposure
		};
}

public record UpdatedHousingInitialStateForOrganizeAndFinanceMilestone(
	bool? HasPestOrMold,
	bool? HasFaultyElectricalSystem,
	bool? HasVentilationSystem,
	bool? HasHeatingSystem,
	bool? HasHotWaterProduction,
	bool? HasOpenings,
	bool? HasInsulation,
	bool? HasHousingCover,
	string? DisordersObservedCommentary,
	DpeLabel? BeforeWorkDpe)
{
	public HousingInitialState CreateUpdateHousingInitialState() =>
		new()
		{
			HasPestOrMold = HasPestOrMold,
			HasFaultyElectricalSystem = HasFaultyElectricalSystem,
			HasVentilationSystem = HasVentilationSystem,
			HasHeatingSystem = HasHeatingSystem,
			HasHotWaterProduction = HasHotWaterProduction,
			HasOpenings = HasOpenings,
			HasInsulation = HasInsulation,
			HasHousingCover = HasHousingCover,
			DisordersObservedCommentary = DisordersObservedCommentary,
			Dpe = (int?)BeforeWorkDpe
		};
}

public record UpdateHousingAfterWorkState(
	double? EstimatedAnnualEnergyConsumptionAfterWork,
	double? EstimatedAnnualGesemissionsAfterWork,
	DpeLabel? EstimatedDpeafterWork,
	GesLabel? EstimatedGesafterWork,
	int? EstimatedDpeclassJump)
{
	public HousingAfterWorkState CreateUpdateHousingAfterWorkState() =>
		new()
		{
			EstimatedAnnualEnergyConsumptionAfterWork = EstimatedAnnualEnergyConsumptionAfterWork,
			EstimatedAnnualGesemissionsAfterWork = EstimatedAnnualGesemissionsAfterWork,
			EstimatedDpeafterWork = (int?)EstimatedDpeafterWork,
			EstimatedGesafterWork = (int?)EstimatedGesafterWork,
			EstimatedDpeclassJump = EstimatedDpeclassJump
		};
}

public record UpdatePreWorkPlan(
	RenovationType? RenovationType,
	string? NextStepAndVigilancePoints,
	bool? NeedTemporaryRehousingSolution,
	bool? InterestInPossibleSupportedSelfRehabilitationAra,
	bool? AraOpeningStatementSent,
	bool? IsEmergencyWorks,
	bool? IsEnergeticsRenovationWorks,
	bool? IsInducedWorks,
	bool? IsSafetyAndHealthWorks,
	PartlyStateTreatment? TreatedAirTightness,
	PartlyStateTreatment? TreatedThermalBridge,
	PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
	bool? IsRgeLabelUpToDate,
	bool? IsFamilyReadyForSupportedSelfRehabilitationApproach,
	bool? FamilyPhysicalcCapabilitiesHaveBeenTakenIntoAccount,
	bool? FamilyCanMobilizeSocialCircleOnConstructionSite,
	string? WorksDetails,
	string? FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite,
	List<Guid> ProjectTypes,
	List<Guid> InsuranceTypes)
{
	public PreWorkPlan CreateUpdatePreWorkPlan() =>
		new()
		{
			RenovationType = (int?)RenovationType,
			NextStepAndVigilancePoint = NextStepAndVigilancePoints,
			HasNeedForTemporaryReHousing = NeedTemporaryRehousingSolution,
			HasInterestInPossibleAraprocess = InterestInPossibleSupportedSelfRehabilitationAra,
			IsAraopeningStatementSent = AraOpeningStatementSent,
			HasEmergencyWorks = IsEmergencyWorks,
			HasEnergeticsRenovationWorks = IsEnergeticsRenovationWorks,
			HasInducedWorks = IsInducedWorks,
			HasSafetyAndHealthWorks = IsSafetyAndHealthWorks,
			TreatedAirTightness = (int?)TreatedAirTightness,
			TreatedThermalBridge = (int?)TreatedThermalBridge,
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
				(int?)AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			IsHouseholdReadyToStartAraprocess = IsFamilyReadyForSupportedSelfRehabilitationApproach,
			AreHouseholdPhysicalCapacitiesTakenIntoAccount = FamilyPhysicalcCapabilitiesHaveBeenTakenIntoAccount,
			DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean =
				FamilyCanMobilizeSocialCircleOnConstructionSite,
			WorksDetails = WorksDetails,
			HouseholdAvailabilitiyToOrganizeArasite =
				FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite,
			IsRgeLabelUpToDate = IsRgeLabelUpToDate
		};
}

public record UpdatePreFinancingPlan(
	double? GuidedPathwayBonus,
	double? CoOwnershipBonus,
	double? DecentHousingBonus,
	double? AdaptationBonus,
	double? ExitEnergySieveBonus,
	double? Region,
	double? Department,
	double? PublicEstablishmentsIntercommunalCooperation,
	double? Municipality,
	string? BankLoanType,
	double? ClassicBankLoan,
	double? DepartmentalHouseForDisabledPersons,
	double? EnergySavingCertificates,
	double? HouseholdMaximumSavingAmountForRenovationProject,
	double? FamilyAllowanceFund,
	double? PensionFund,
	double? UnderprivilegedHousingFoundation,
	double? LeroyMerlinFoundation,
	double? WattForChangeFoundation,
	double? SocialProtectionGroup,
	double? StopEnergyExclusionFunds,
	List<FundingModeDto> FundingModes,
	double? OtherFamilyMemberMaximumSupportAmountForRenovationProject)
{
	public PreFinancingPlan CreateUpdatePreFinancingPlan() =>
		new()
		{
			MaPrimeRenovGuidedPath = GuidedPathwayBonus,
			MaPrimeRenovCoOwnerShip = CoOwnershipBonus,
			MaPrimeLogementDecent = DecentHousingBonus,
			MaPrimeAdapt = AdaptationBonus,
			BonusForExitingEnergeticSieve = ExitEnergySieveBonus,
			RegionalAids = Region,
			DepartmentalAids = Department,
			PublicEstablishmentsForInterCommunalCooperationAids = PublicEstablishmentsIntercommunalCooperation,
			MunicipalityAids = Municipality,
			SolicitedBankLoanType = BankLoanType,
			ClassicBankLoan = ClassicBankLoan,
			MdphFinancing = DepartmentalHouseForDisabledPersons,
			CeeFinancing = EnergySavingCertificates,
			OtherFamilyMemberMaximumSupportAmountForRenovationProject =
				OtherFamilyMemberMaximumSupportAmountForRenovationProject,
			PensionFund = PensionFund,
			CafMsaFinancing = FamilyAllowanceFund,
			UnderprivilegedHousingFoundation = UnderprivilegedHousingFoundation,
			LeroyMerlinFoundation = LeroyMerlinFoundation,
			WattForChangeFoundation = WattForChangeFoundation,
			SocialProtectionGroup = SocialProtectionGroup,
			StopEnergyExclusionFunds = StopEnergyExclusionFunds,
			FundingModes =
				FundingModes.Select(fm => new FundingMode { Id = fm.Id, Label = fm.Label, Value = fm.Value })
					.ToList(),
			HouseholdMaximumSavingAmountForRenovationProject = HouseholdMaximumSavingAmountForRenovationProject
		};
}

public record UpdateFundingModes(Guid? Id, string Label, double Value)
{
	public FundingMode CreateUpdateFundingMode() => new() { Id = Id ?? Guid.Empty, Label = Label, Value = Value };
}

public record UpdateWorkPackage(
	Guid? Id,
	string? EnergeticsEffectAfterWorks,
	List<UpdateWorkTypeCost> UpdateWorkTypeCosts)
{
	public WorkPackage CreateUpdatedWorkPackage()
	{
		return new WorkPackage
		{
			Id = Id ?? Guid.Empty,
			EnergeticsEffectAfterWorks = EnergeticsEffectAfterWorks,
			WorkPackageWorkTypeCosts = UpdateWorkTypeCosts.Select(wtc => wtc.UpdatedWorkTypeCost()).ToList()
		};
	}
}

public record UpdateWorkTypeCost(Guid Id, double? Cost, string? Description)
{
	public WorkPackageWorkTypeCost UpdatedWorkTypeCost() =>
		new() { WorkType = Id, Cost = Cost ?? 0, Description = Description };
}