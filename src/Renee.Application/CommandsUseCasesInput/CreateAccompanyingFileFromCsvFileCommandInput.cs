using MediatR;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Helpers;
using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateAccompanyingFileFromCsvFileCommandInput(
    List<ImportAccompanyingFileFromCsvCommandInput> Input,
    Guid UserId,
    Guid ImportRunId) : IRequest<ImportCsvDataCommandResult>;

public record ImportAccompanyingFileFromCsvCommandInput(
    int LineNumber,
    string Reference,
    string? ExternalReference,
    DateTime? FirstContactDate,
    DateTime? StartSupportDate,
    DateTime? FileOpeningDate,
    DateTime? FileClosingDate,
    DateTime? EndContactDate,
    DateTime? EndSupportDate,
    AccompanyingTimeDuration? AccompanyingTimeDurationForIdentificationMilestone,
	AccompanyingTimeDuration? AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
	AccompanyingTimeDuration? AccompanyingTimeDurationForRealizeAndFollowMilestone,
    bool? ZeroEnergyExclusionTerritoriesProgram,
    AccompanyingType? AccompanyingType,
    Guid? TerritoryId,
    bool? IsDeleted,
    ImportSupportTeamFromCsv SupportTeam,
    ImportHouseholdFromCsv Household,
    ImportHousingFromCsv Housing,
    ImportPreWorkPlanFromCsv PreWorkPlan,
    ImportPreFinancingPlanFromCsv PreFinancingPlan,
    ImportWorkMonitoringFromCsv WorkMonitoring);

public record ImportOccupantFromCsv(
    string Trigram,
    DateTime? Birthdate,
    SocioProfessionalCategory? SocioProfessionalCategory,
    string? PhoneNumber,
    string? Email,
    string? Profession,
    SocialProtectionFund? SocialProtectionFund,
    string? SocialFundComment,
    PensionFund? RetirementFund,
    string? RetirementFundComment,
    AdditionalFund? ComplementaryFund,
    string? ComplementaryFundComment,
    string? LastName,
    string? FirstName)
{
    public MainOccupant CreateMainOccupant() =>
        new()
        {
            Trigram = Trigram,
            Birthdate = Birthdate,
            SocioProfessionalCategory = (int?)SocioProfessionalCategory,
            PhoneNumber = PhoneNumber,
            Email = Email,
            Job = Profession,
            SocialProtectionFund = (int?)SocialProtectionFund,
            CommentOnSocialProtectionFund = SocialFundComment,
            PensionFund = (int?)RetirementFund,
            CommentOnPensionFund = RetirementFundComment,
            AdditionnalFund = (int?)ComplementaryFund,
            CommentOnAdditionnalFund = ComplementaryFundComment,
            Age = AccompanyingFileHelper.CalculateAge(Birthdate),
            FirstName = FirstName,
            LastName = LastName
        };
}

public record ImportSupportTeamFromCsv(
    Guid SolidarBuilderId,
    MarkerNature MarkerNature,
    string? OtherMarkerNature)
{
    public SupportTeam CreateSupportTeam() =>
        new()
        {
            SolidarBuilder = SolidarBuilderId,
            MarkerNature = (int)MarkerNature,
            CommentOnMarkerNature = OtherMarkerNature
        };
}

public record ImportAddressFromCsv(
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

public record ImportHousingInitialStateFromCsv(
    DegradationIndex? DegradationIndex,
    UnsanitaryCoefficient? UnsanitaryCoefficient,
    EnergyDeprivation? EnergyDeprivation,
    ComfortLevel? SummerComfortLevel,
    ComfortLevel? WinterComfortLevel,
    ComfortLevel? SoundComfortLevel,
    bool? PestsOrMold,
    bool? ElectricalSystem,
    bool? VentilationSystem,
    bool? HeatingSystem,
    bool? HotWaterProduction,
    bool? RoofCondition,
    bool? OpeningsCondition,
    bool? CoveringsCondition,
    string? ProblemsBeforeWorksComment,
    DpeLabel? EnergyLabelBefore,
    GesLabel? GesLabelBefore,
    double? AnnualEnergyConsumptionBefore,
    double? AnnualGesBefore,
    string? HeatingEnergyBefore)
{
    public HousingInitialState CreateHousingInitialState() =>
        new()
        {
            DegradationIndex = (int?)DegradationIndex,
            UnsanitaryCoefficient = (int?)UnsanitaryCoefficient,
            EnergyDepravation = (int?)EnergyDeprivation,
            SummerThermalComfortLevel = (int?)SummerComfortLevel,
            WinterThermalComfortLevel = (int?)WinterComfortLevel,
            NoiseComfortLevel = (int?)SoundComfortLevel,
            HasPestOrMold = PestsOrMold,
            HasFaultyElectricalSystem = ElectricalSystem,
            HasVentilationSystem = VentilationSystem,
            HasHeatingSystem = HeatingSystem,
            HasHotWaterProduction = HotWaterProduction,
            HasInsulation = RoofCondition,
            HasOpenings = OpeningsCondition,
            HasHousingCover = CoveringsCondition,
            DisordersObservedCommentary = ProblemsBeforeWorksComment,
            Dpe = (int?)EnergyLabelBefore,
            Ges = (int?)GesLabelBefore,
            AnnualEnergyConsumption = AnnualEnergyConsumptionBefore,
            AnnualGesemission = AnnualGesBefore,
            HeatingEnergy = HeatingEnergyBefore
        };
}


public record ImportHousingAfterWorkStateFromCsv(
    DpeLabel? EstimatedDpeLabelAfterWork,
    double? EstimatedAnnualEnergyConsumptionAfterWork,
    double? EstimatedAnnualGESEmissionsAfterWork,
    GesLabel? EstimatedGesLabelAfterWork,
    int? EstimatedDpeClassJump)
{
    public HousingAfterWorkState CreateHousingAfterWorkState() =>
        new()
        {
            EstimatedDpeafterWork = (int?)EstimatedDpeLabelAfterWork,
            EstimatedAnnualEnergyConsumptionAfterWork = EstimatedAnnualEnergyConsumptionAfterWork,
            EstimatedAnnualGesemissionsAfterWork = EstimatedAnnualGESEmissionsAfterWork,
            EstimatedGesafterWork = (int?)EstimatedGesLabelAfterWork,
            EstimatedDpeclassJump = EstimatedDpeClassJump
		};
}

public record ImportHousingFromCsv(
    HousingYearConstruction? ConstructionYear,
    double? LivingSpace,
    HousingType? HousingType,
    GeographicalHousingAreaTypology? GeographicalHousingAreaTypology,
    OwnershipStatus? OwnershipStatus,
	bool? IsABFZone,
	string? CadastralReference,
	string? ArchitecturalOrTownPlanningStandards,
	int? YearOfAcquisitionOrEntry,
	bool? HasPreviousWorks,
	string? CommentOnPreviousWork,
	SunExposure? SunExposure,
	double? CeilingHeight,
	int? NumberOfBayWindows,
	int? NumberOfDoors,
	int? NumberOfDoorsWindows,
	int? NumberOfRooms,
	int? NumberOfSkylights,
	int? NumberOfWindows,
    int? NumberOfFloor,
	Address Address,
    HousingInitialState HousingInitialState,
    HousingAfterWorkState HousingAfterWorkState)
{
    public Housing CreateHousing() =>
        new()
        {
            ConstructionYear = (int?)ConstructionYear,
            LivingSpace = LivingSpace,
            HousingType = (int?)HousingType,
            GeographicAreaTypology = (int?)GeographicalHousingAreaTypology,
            OwnershipStatus = (int?)OwnershipStatus,
            IsInAbfarea = IsABFZone,
            CadastralReference = CadastralReference,
            ArchitecturalOrTownPlanningStandards = ArchitecturalOrTownPlanningStandards,
            YearOfAcquisitionOrEntry = YearOfAcquisitionOrEntry,
            HasPreviousWork = HasPreviousWorks,
            CommentOnPreviousWork = CommentOnPreviousWork,
            HousingAddressNavigation = Address,
            HousingInitialStateNavigation = HousingInitialState,
            HousingAfterWorkStateNavigation = HousingAfterWorkState,
            SunExposure = (int?)SunExposure,
            CeilingHeight = CeilingHeight,
            NumberOfBayWindow = NumberOfBayWindows,
            NumberOfDoor = NumberOfDoors,
            NumberOfPatioDoor = NumberOfDoorsWindows,
            NumberOfRoofDoor = NumberOfSkylights,
            NumberOfRoom = NumberOfRooms,
            NumberOfWindow = NumberOfWindows,
            NumberOfFloor = NumberOfFloor
        };
}

public record ImportHouseholdFromCsv(
    double? ReferenceIncomeTax,
    HouseholdTypology? HouseholdTypology,
    string? SocialContext,
    bool? HasOverdueInvoice,
    string? FamilyProject,
    List<Guid> HouseholdDifficulties,
    Dictionary<Guid, double> HouseholdResources,
    Dictionary<int, double> HouseholdExpenses,
    Dictionary<Guid, double> HeatingEnergies,
    bool? IsFollowedByAnSocialWorker,
    bool? HasAnOccupantWithDisabilities,
    bool? HasAnOccupantWithLongTermIllness,
    bool? HasAnOccupantWithIndependenceLoss,
    bool? HasAnOccupantUnderCuratorship,
    bool? HasAnOccupantUnderGuardianship,
    string? CommentsOnHouseholdDifficulties,
    string? HouseholdAvailabilityForVisits,
    int? NumberOfOccupants,
    MainOccupant MainOccupant)
{
    public Household CreateHousehold() =>
        new()
        {
            ReferenceIncomeTax = ReferenceIncomeTax,
            HouseholdTypology = (int?)HouseholdTypology,
            SocialContext = SocialContext,
            HasOverdueInvoice = HasOverdueInvoice,
            MainOccupantNavigation = MainOccupant,
            HouseholdProject = FamilyProject,
            HouseholdDifficulties = [.. HouseholdDifficulties.Select(h => new HouseholdDifficulty { Difficulty = h })],
            HouseholdResources = [.. HouseholdResources.Select(keyValuePair => new HouseholdResource
            {
                HouseholdResources = keyValuePair.Key,
                Value = keyValuePair.Value
            })],
            HouseholdExpenses = [.. HouseholdExpenses.Select(keyValuePair => new HouseholdExpense
            {
                Type = keyValuePair.Key,
                Value = keyValuePair.Value
            })],
            HouseholdHeatingEnergies = [.. HeatingEnergies.Select(keyValuePair => new HouseholdHeatingEnergy
            {
                HouseholdHeatingEnergyLabel = keyValuePair.Key,
                Value = keyValuePair.Value
            })],
            IsFollowedByAnSocialWorker = IsFollowedByAnSocialWorker,
            HasAnOccupantWithDisabilities = HasAnOccupantWithDisabilities,
            HasAnOccupantWithLongTermIllness = HasAnOccupantWithLongTermIllness,
            HasAnOccupantWithIndependenceLoss = HasAnOccupantWithIndependenceLoss,
            HasAnOccupantUnderCuratorship = HasAnOccupantUnderCuratorship,
            HasAnOccupantUnderGuardianship = HasAnOccupantUnderGuardianship,
            CommentsOnHouseholdDifficulties = CommentsOnHouseholdDifficulties,
            HouseholdAvailabilityForVisits = HouseholdAvailabilityForVisits,
            SecondaryOccupants = CreateSecondaryOccupants(NumberOfOccupants)
        };
    public static List<SecondaryOccupant> CreateSecondaryOccupants(int? nbOccupant)
    {
        var result = new List<SecondaryOccupant>();
        if (!nbOccupant.HasValue || nbOccupant.Value <= 0)
            return result;
        for (int i = 0; i < nbOccupant.Value - 1; i++)
        {
            result.Add(new SecondaryOccupant() { Trigram = string.Empty });
        }
        return result;
    }
}

public record ImportPreWorkPlanFromCsv(
    List<Guid> InsurancesTypes,
    string? NextStepAndVigilancePoints,
    RenovationType? RenovationType,
    bool? HasInterestInPossibleARAProcess,
    bool? HasNeedForTemporaryReHousing,
    bool? HasEmergencyWorks,
    bool? HasEnergeticsRenovationWorks,
    bool? HasInducedWorks,
    bool? HasSafetyAndHealthWorks,
    PartlyStateTreatment? TreatedAirTightness,
    PartlyStateTreatment? TreatedThermalBridge,
    PartlyStateTreatment? AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
    bool? IsHouseholdReadyToStartARAProcess,
    bool? AreHouseholdPhysicalCapacitiesTakenIntoAccount,
    bool? DoHouseholdCanMobilizeSocialCircleOnConstructionSite,
    string? WorksDetails,
    string? HouseholdAvailabilitiyToOrganizeARASite,
    bool? IsRgeLabelUpToDate,
    string? OtherQualification,
	List<Guid> ProjectTypes)
{
    public PreWorkPlan CreatePreWorkPlan() =>
        new()
        {
            PreWorkPlanInsuranceTypes = InsurancesTypes.Select(i => new PreWorkPlanInsuranceType { InsuranceType = i }).ToList(),
            NextStepAndVigilancePoint = NextStepAndVigilancePoints,
            RenovationType = (int?)RenovationType,
            HasInterestInPossibleAraprocess = HasInterestInPossibleARAProcess,
            HasEmergencyWorks = HasEmergencyWorks,
            HasEnergeticsRenovationWorks = HasEnergeticsRenovationWorks,
            HasInducedWorks = HasInducedWorks,
            HasSafetyAndHealthWorks = HasSafetyAndHealthWorks,
            TreatedAirTightness = (int?)TreatedAirTightness,
            TreatedThermalBridge = (int?)TreatedThermalBridge,
            AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
                (int?)AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
            IsHouseholdReadyToStartAraprocess = IsHouseholdReadyToStartARAProcess,
            AreHouseholdPhysicalCapacitiesTakenIntoAccount = AreHouseholdPhysicalCapacitiesTakenIntoAccount,
            DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean = DoHouseholdCanMobilizeSocialCircleOnConstructionSite,
            WorksDetails = WorksDetails,
            HouseholdAvailabilitiyToOrganizeArasite =  HouseholdAvailabilitiyToOrganizeARASite,
            IsRgeLabelUpToDate = IsRgeLabelUpToDate,
            OtherQualification = OtherQualification,
            PreWorkPlanProjectTypes = ProjectTypes.Select(p => new PreWorkPlanProjectType { ProjectType = p }).ToList(),
            HasNeedForTemporaryReHousing = HasNeedForTemporaryReHousing
        };
}

public record ImportPreFinancingPlanFromCsv(
    double? MaPrimeRenovGuidedPath,
    double? MaPrimeRenovCoOwnerShip,
    double? MaPrimeLogementDecent,
    double? MaPrimeAdapt,
    double? BonusForExitingEnergeticSieve,
    double? RegionAids,
    double? DepartmentAids,
    double? PublicEstablishmentsIntercommunalCooperation,
    double? MunicipalityAids,
    string? SolicitedBankLoanType,
    double? ClassicBankLoan,
    double? MdphFinancing,
    double? CeeFinancing,
    double? CafMsaFinancing,
    double? PensionFunds,
    double? UnderprivilegedHousingFoundation,
    double? LeroyMerlinFoundation,
    double? WattForChangeFoundation,
    double? SocialProtectionGroup,
    double? StopEnergyExclusionFunds,
	double? HouseholdMaximumSavingAmountForRenovationProject,
    double? MaximumAmountSupportFamilyMembersRenovationProject,
    List<FundingMode> FundingModes)
{
    public PreFinancingPlan CreatePreFinancingPlan() =>
        new()
        {
            MaPrimeRenovGuidedPath = MaPrimeRenovGuidedPath,
            MaPrimeRenovCoOwnerShip =  MaPrimeRenovCoOwnerShip,
            MaPrimeLogementDecent = MaPrimeLogementDecent,
            MaPrimeAdapt =  MaPrimeAdapt,
            BonusForExitingEnergeticSieve = BonusForExitingEnergeticSieve,
            RegionalAids = RegionAids,
            DepartmentalAids = DepartmentAids,
            PublicEstablishmentsForInterCommunalCooperationAids = PublicEstablishmentsIntercommunalCooperation,
            MunicipalityAids = MunicipalityAids,
            SolicitedBankLoanType = SolicitedBankLoanType,
            ClassicBankLoan = ClassicBankLoan,
            MdphFinancing = MdphFinancing,
            CeeFinancing = CeeFinancing,
            CafMsaFinancing = CafMsaFinancing,
            PensionFund = PensionFunds,
            UnderprivilegedHousingFoundation = UnderprivilegedHousingFoundation,
            LeroyMerlinFoundation = LeroyMerlinFoundation,
            WattForChangeFoundation = WattForChangeFoundation,
            SocialProtectionGroup = SocialProtectionGroup,
            StopEnergyExclusionFunds = StopEnergyExclusionFunds,
			HouseholdMaximumSavingAmountForRenovationProject = HouseholdMaximumSavingAmountForRenovationProject,
            OtherFamilyMemberMaximumSupportAmountForRenovationProject =
                MaximumAmountSupportFamilyMembersRenovationProject,
            FundingModes = FundingModes
        };
}

public record ImportWorkMonitoringFromCsv(
    double? AccompanyingCost,
    double? HouseholdAutoFinancing,
    string? IntermediateAirtightnessTestResult,
    string? JustificationAndActionsPutInPlaceIfNoTest,
    bool? HasEffectiveComplianceWithWorkRecommendations,
    bool? HasWorkEnablingHomeSupport,
    int? WellBeingRating,
    int? EducationalFrameworkRating,
    int? FamilySatisfactionWithSupport,
    bool? ReturnToEmployment,
    bool? HasHousingAdaptationWorks,
    bool? HasFinishingWorks,
    bool? HasSafetyWorks,
    bool? HasPreparationWorks,
    bool? HasEmergencyWorks,
    bool? HasUnsanitaryExit,
    PartlyStateTreatment? TreatedAirTightness,
    PartlyStateTreatment? TreatedThermalBridges,
    bool? HasHumidityManagement,
    double? WorkTotalCost)
{
    public WorkMonitoring CreateWorkMonitoring() =>
        new()
        {
            AccompanyingCost = AccompanyingCost,
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
            HasHumidityManagement = HasHumidityManagement,
            WorkTotalCost = WorkTotalCost
        };
}

public record ImportSiteSupervisionFromCsv()
{
    public static SiteSupervision CreateSiteSupervision() =>
        new();
}