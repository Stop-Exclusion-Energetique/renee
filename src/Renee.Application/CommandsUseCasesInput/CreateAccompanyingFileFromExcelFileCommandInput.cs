using MediatR;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Helpers;
using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateAccompanyingFileFromExcelFileCommandInput(
	ImportAccompanyingFileFromFileInputCommandInput? Input,
	Guid UserId) : IRequest<ImportExcelDataCommandResult>;

public record ImportAccompanyingFileFromFileInputCommandInput(
	string Reference,
	DateTime? FirstContactDate,
	DateTime? StartSupportDate,
	DateTime? EndContactDate,
	DateTime? EndSupportDate,
	MarkerNature? MarkerNature,
	string? CommentOnMarkerNature,
	int? ContactWithFamilyForIdentificationMilestone,
	int? ContactWithFamilyForOrganizeAndFinanceMilestone,
	int? ContactWithFamilyForRealizeAndFollowMilestone,
	bool? ZeroEnergyExclusionTerritoriesProgram,
	AccompanyingType? AccompanyingType,
	Guid? TerritoryId,
	ImportSupportTeam SupportTeam,
	ImportHousehold Household,
	ImportHousing Housing,
	ImportPreWorkPlan PreWorkPlan,
	ImportPreFinancingPlan PreFinancingPlan,
	ImportWorkMonitoring WorkMonitoring,
	ImportInvoice Invoice);

public record RequiredFieldsFromDataImportPage(
	string? LastName,
	string? FirstName,
	bool? ZeroEnergyExclusionTerritoriesProgram,
	AccompanyingType? AccompanyingType,
	Guid? ReferentTargetCoordinatorId,
	Guid? ReferentEtId,
	Guid? TerritoryId,
	Guid? ReferentDiffuseCoordinatorId,
	Guid? ReferentSolidarBuilderId);

public record OccupantToCreate(
	string Trigram,
	DateTime Birthdate,
	SocioProfessionalCategory? SocioProfessionalCategory,
	string? LastName,
	string? FirstName)
{
	public MainOccupant CreateMainOccupant() =>
		new()
		{
			Trigram = Trigram,
			Birthdate = Birthdate,
			SocioProfessionalCategory = (int?)SocioProfessionalCategory,
			Age = AccompanyingFileHelper.CalculateAge(Birthdate),
			FirstName = FirstName,
			LastName = LastName
		};

	public SecondaryOccupant CreateSecondaryOccupant() =>
		new() { Trigram = Trigram, Birthdate = Birthdate, Age = AccompanyingFileHelper.CalculateAge(Birthdate) };
}

public record ImportSupportTeam(
	Guid? ReferentDiffuseCoordinatorId,
	Guid? ReferentTargetCoordinatorId,
	Guid? TerritorialBuilderId,
	Guid SolidarBuilderId,
    MarkerNature MarkerNature,
	string? OtherMarkerNature)
{
	public SupportTeam CreateSupportTeam() =>
		new()
		{
			DiffuseCoordinator = ReferentDiffuseCoordinatorId,
			TargetCoordinator = ReferentTargetCoordinatorId,
			SolidarBuilder = SolidarBuilderId,
			TerritorialBuilder = TerritorialBuilderId,
			MarkerNature = (int)MarkerNature,
			CommentOnMarkerNature = OtherMarkerNature
		};
}

public record ImportAddress(
	string Label,
	string PostalCode,
	string City,
	string Department,
	string Region,
	string AdditionnalAddress)
{
	public Address CreateAddress() =>
		new()
		{
			City = City,
			AdditionnalComment = AdditionnalAddress,
			Department = Department,
			Label = Label,
			PostalCode = PostalCode,
			Region = Region
		};
}

public record ImportHousingInitialState(
	DegradationIndex? DegradationIndex,
	UnsanitaryCoefficient? UnsanitaryCoefficient,
	double? AnnualEnergyConsumption,
	DpeLabel? DpeLabel,
	EnergyDeprivation? EnergyDeprivation)
{
	public HousingInitialState CreateHousingInitialState() =>
		new()
		{
			DegradationIndex = (int?)DegradationIndex,
			UnsanitaryCoefficient = (int?)UnsanitaryCoefficient,
			AnnualEnergyConsumption = AnnualEnergyConsumption,
			Dpe = (int?)DpeLabel,
			EnergyDepravation = (int?)EnergyDeprivation
		};
}

public record ImportHousingInitialStateV3(
	DegradationIndex? DegradationIndex,
	UnsanitaryCoefficient? UnsanitaryCoefficient,
	double? AnnualEnergyConsumption,
	DpeLabel? DpeLabel,
	GesLabel? GesLabel,
	EnergyDeprivation? EnergyDeprivation)
{
	public HousingInitialState CreateHousingInitialState() =>
		new()
		{
			DegradationIndex = (int?)DegradationIndex,
			UnsanitaryCoefficient = (int?)UnsanitaryCoefficient,
			AnnualEnergyConsumption = AnnualEnergyConsumption,
			Dpe = (int?)DpeLabel,
			Ges = (int?)GesLabel,
			EnergyDepravation = (int?)EnergyDeprivation,
			
		};
}

public record ImportHousingAfterWorkState(
	DpeLabel? EstimatedDpeLabelAfterWork,
	double? EstimatedAnnualEnergyConsumptionAfterWork,
	int? EstimatedDpeClassJump)
{
	public HousingAfterWorkState CreateHousingAfterWorkState() =>
		new()
		{
			EstimatedDpeafterWork = (int?)EstimatedDpeLabelAfterWork,
			EstimatedAnnualEnergyConsumptionAfterWork = EstimatedAnnualEnergyConsumptionAfterWork,
			EstimatedDpeclassJump = EstimatedDpeClassJump
		};
}

public record ImportHousingAfterWorkStateV3(
	DpeLabel? EstimatedDpeLabelAfterWork,
	GesLabel? EstimatedGesLabelAfterWork,
	double? EstimatedAnnualEnergyConsumptionAfterWork,
	int? EstimatedDpeClassJump)
{
	public HousingAfterWorkState CreateHousingAfterWorkState() =>
		new()
		{
			EstimatedDpeafterWork = (int?)EstimatedDpeLabelAfterWork,
			EstimatedGesafterWork = (int?) EstimatedGesLabelAfterWork,
			EstimatedAnnualEnergyConsumptionAfterWork = EstimatedAnnualEnergyConsumptionAfterWork,
			EstimatedDpeclassJump = EstimatedDpeClassJump
		};
}

public record ImportHousing(
	int? ConstructionYear,
	int? LivingSpace,
	HousingType? HousingType,
	GeographicalHousingAreaTypology? GeographicalHousingAreaTypology,
	OwnershipStatus? OwnershipStatus,
	Address Address,
	HousingInitialState HousingInitialState,
	HousingAfterWorkState HousingAfterWorkState)
{
	public Housing CreateHousing() =>
		new()
		{
			ConstructionYear = ConstructionYear,
			LivingSpace = LivingSpace,
			HousingType = (int?)HousingType,
			OwnershipStatus = (int?)OwnershipStatus,
			HousingInitialStateNavigation = HousingInitialState,
			HousingAfterWorkStateNavigation = HousingAfterWorkState,
			HousingAddressNavigation = Address,
			GeographicAreaTypology = (int?)GeographicalHousingAreaTypology
		};
}

public record ImportHousehold(
	double? ReferenceIncomeTax,
	HouseholdTypology? HouseholdTypology,
	string? SocialContext,
	bool? HasOverdueInvoice,
	string? AnahCategory,
	MainOccupant MainOccupant,
	List<SecondaryOccupant> SecondaryOccupants)
{
	public Household CreateHousehold() =>
		new()
		{
			ReferenceIncomeTax = ReferenceIncomeTax,
			HouseholdTypology = (int?)HouseholdTypology,
			SocialContext = SocialContext,
			HasOverdueInvoice = HasOverdueInvoice,
			AnahCategory = AnahCategory,
			MainOccupantNavigation = MainOccupant,
			SecondaryOccupants = SecondaryOccupants,
		};
}

public record ImportPreWorkPlan(
	List<Guid> ProjectTypes,
	RenovationType? RenovationType,
	string? NextStepAndVigilancePoints,
	bool? IsEmergencyWorks,
	PartlyStateTreatment? TreatedAirTightness,
	PartlyStateTreatment? TreatedThermalBridge,
	PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
	List<WorkPackage> WorkPackages)
{
	public PreWorkPlan CreatePreWorkPlan() =>
		new()
		{
			PreWorkPlanProjectTypes = ProjectTypes.Select(p => new PreWorkPlanProjectType { ProjectType = p }).ToList(),
			RenovationType = (int?)RenovationType,
			NextStepAndVigilancePoint = NextStepAndVigilancePoints,
			HasEmergencyWorks = IsEmergencyWorks,
			TreatedAirTightness = (int?)TreatedAirTightness,
			TreatedThermalBridge = (int?)TreatedThermalBridge,
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
				(int?)AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			WorkPackages = WorkPackages
		};
}

public record ImportPreFinancingPlan(
	double? RegionAids,
	double? DepartmentAids,
	double? PublicEstablishmentsIntercommunalCooperation,
	double? MunicipalityAids,
	double? PrivateActors,
	double? PensionFunds,
	double? HouseholdMaximumSavingAmountForRenovationProject,
	double? MaximumAmountSupportFamilyMembersRenovationProject,
	double? EstimatedRemainingAmount)
{
	public PreFinancingPlan CreatePreFinancingPlan() =>
		new()
		{
			RegionalAids = RegionAids,
			DepartmentalAids = DepartmentAids,
			PublicEstablishmentsForInterCommunalCooperationAids = PublicEstablishmentsIntercommunalCooperation,
			MunicipalityAids = MunicipalityAids,
			PensionFund = PensionFunds,
			HouseholdMaximumSavingAmountForRenovationProject = HouseholdMaximumSavingAmountForRenovationProject,
			OtherFamilyMemberMaximumSupportAmountForRenovationProject =
				MaximumAmountSupportFamilyMembersRenovationProject,
			EstimatedRemainingAmount = EstimatedRemainingAmount,
		};
}

public record ImportWorkMonitoring(
	double? HouseholdAutoFinancing,
	string? IntermediateAirtightnessTestResult,
	string? JustificationAndActionsPutInPlaceIfNoTest,
	bool? HasEffectiveComplianceWithWorkRecommendations,
	bool? HasWorkEnablingHomeSupport,
	int? WellBeingRating,
	int? EducationalFrameworkRating,
	int? FamilySatisfactionWithSupport,
	bool? ReturnToEmployment,
	bool? HasHousingAdaptationWorks = null,
bool? HasFinishingWorks = null,
bool? HasSafetyWorks = null,
bool? HasPreparationWorks = null,
bool? HasEmergencyWorks = null,
bool? HasUnsanitaryExit = null,
PartlyStateTreatment? TreatedAirTightness = null,
PartlyStateTreatment? TreatedThermalBridges = null,
bool? HasHumidityManagement = null)
{
	public WorkMonitoring CreateWorkMonitoring() =>
		new()
		{
			HouseholdSelfFinancing = HouseholdAutoFinancing,
			IntermediateAirtightnessTestResult = IntermediateAirtightnessTestResult,
			JustificationAndActionsPutInPlaceIfNoTest = JustificationAndActionsPutInPlaceIfNoTest,
			HasEffectiveComplianceWithWorkRecommendations = HasEffectiveComplianceWithWorkRecommendations,
			HasWorksEnabledHouseholdToStayAtHome = HasWorkEnablingHomeSupport,
			WellBeingRating = WellBeingRating,
			EducationalFrameworkRating = EducationalFrameworkRating,
			FamilySatisfaction = FamilySatisfactionWithSupport,
			ReturnToEmployment = ReturnToEmployment,
			HasHousingAdaptationWorks = HasHousingAdaptationWorks,
			HasFinishingWorks = HasFinishingWorks,
			HasSafetyWorks = HasSafetyWorks,
			HasPreparationWorks = HasPreparationWorks,
			HasEmergencyWorks = HasEmergencyWorks,
			HasUnsanitaryExit = HasUnsanitaryExit,
			TreatedAirTightness = (int?)TreatedAirTightness,
			TreatedThermalBridges = (int?)TreatedThermalBridges,
			HasHumidityManagement = HasHumidityManagement
		};
}

public record ImportInvoice(
	double? InvoiceCost,
	double? LaborCost)
{
	public Invoice CreateInvoice() =>
		new()
		{
			InvoiceCost = InvoiceCost,
			LaborCost = LaborCost
		};
}