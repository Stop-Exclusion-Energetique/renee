using System.ComponentModel;
using Renee.Domain;

namespace Renee.Application.Services.Administration.ImportCsvData;

public class CsvData
{
    [Description(CsvDataLabel.CsvColumnsNames.Reference)]
    public string? Reference { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.FirstEncounterDate)]
    public string? FirstEncounterDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.StartOfAccompanyingDate)]
    public string? StartOfAccompanyingDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.OpeningDate)]
    public string? OpeningDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CloseDate)]
    public string? CloseDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EndOfAccompanyingDate)]
    public string? EndOfAccompanyingDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EndOfEncounterDate)]
    public string? EndOfEncounterDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ZeroEnergyExclusionTerritoriesProgram)]
    public string? ZeroEnergyExclusionTerritoriesProgram { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AccompanyingType)]
    public string? AccompanyingType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AccompanyingFileTerritory)]
    public string? AccompanyingFileTerritory { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdTypology)]
    public string? HouseholdTypology { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IsFollowedByAnSocialWorker)]
    public string? IsFollowedByAnSocialWorker { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasAnOccupantWithDisabilities)]
    public string? HasAnOccupantWithDisabilities { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasAnOccupantWithLongTermIllness)]
    public string? HasAnOccupantWithLongTermIllness { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasAnOccupantWithIndependenceLoss)]
    public string? HasAnOccupantWithIndependenceLoss { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasAnOccupantUnderCuratorship)]
    public string? HasAnOccupantUnderCuratorship { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasAnOccupantUnderGuardianship)]
    public string? HasAnOccupantUnderGuardianship { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ReferenceIncomeTax)]
    public string? ReferenceIncomeTax { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SocialContext)]
    public string? SocialContext { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdProject)]
    public string? HouseholdProject { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdAvailabilityForVisits)]
    public string? HouseholdAvailabilityForVisits { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentsOnHouseholdDifficulties)]
    public string? CommentsOnHouseholdDifficulties { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasOverdueInvoice)]
    public string? HasOverdueInvoice { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdDifficulties)]
    public string? HouseholdDifficulties { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdExpenses)]
    public string? HouseholdExpenses { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdResources)]
    public string? HouseholdResources { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Birthdate)]
    public string? Birthdate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SocioProfessionalCategory)]
    public string? SocioProfessionalCategory { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.PhoneNumber)]
    public string? PhoneNumber { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Email)]
    public string? Email { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Job)]
    public string? Job { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SocialProtectionFund)]
    public string? SocialProtectionFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentOnSocialProtectionFund)]
    public string? CommentOnSocialProtectionFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.PensionFundOccupant)]
    public string? PensionFundOccupant { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentOnPensionFund)]
    public string? CommentOnPensionFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AdditionnalFund)]
    public string? AdditionnalFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentOnAdditionnalFund)]
    public string? CommentOnAdditionnalFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.FirstName)]
    public string? FirstName { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.LastName)]
    public string? LastName { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.GeographicAreaTypology)]
    public string? GeographicAreaTypology { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IsInABFArea)]
    public string? IsInABFArea { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ArchitecturalOrTownPlanningStandards)]
    public string? ArchitecturalOrTownPlanningStandards { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.OwnershipStatus)]
    public string? OwnershipStatus { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HousingType)]
    public string? HousingType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ConstructionYear)]
    public string? ConstructionYear { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.LivingSpace)]
    public string? LivingSpace { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfRoom)]
    public string? NumberOfRoom { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfFloor)]
    public string? NumberOfFloor { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.YearOfAcquisitionOrEntry)]
    public string? YearOfAcquisitionOrEntry { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CadastralReference)]
    public string? CadastralReference { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SunExposure)]
    public string? SunExposure { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfDoor)]
    public string? NumberOfDoor { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfWindow)]
    public string? NumberOfWindow { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfPatioDoor)]
    public string? NumberOfPatioDoor { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfRoofDoor)]
    public string? NumberOfRoofDoor { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfBayWindow)]
    public string? NumberOfBayWindow { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CeilingHeight)]
    public string? CeilingHeight { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasPreviousWork)]
    public string? HasPreviousWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentOnPreviousWork)]
    public string? CommentOnPreviousWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Label)]
    public string? Label { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.PostalCode)]
    public string? PostalCode { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.City)]
    public string? City { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Department)]
    public string? Department { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Region)]
    public string? Region { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AdditionnalComment)]
    public string? AdditionnalComment { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.DegradationIndex)]
    public string? DegradationIndex { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.UnsanitaryCoefficient)]
    public string? UnsanitaryCoefficient { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EnergyDepravation)]
    public string? EnergyDepravation { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SummerThermalComfortLevel)]
    public string? SummerThermalComfortLevel { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.WinterThermalComfortLevel)]
    public string? WinterThermalComfortLevel { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NoiseComfortLevel)]
    public string? NoiseComfortLevel { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasPestOrMold)]
    public string? HasPestOrMold { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasFaultyElectricalSystem)]
    public string? HasFaultyElectricalSystem { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasVentilationSystem)]
    public string? HasVentilationSystem { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasHeatingSystem)]
    public string? HasHeatingSystem { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasHotWaterProduction)]
    public string? HasHotWaterProduction { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.RoofingState)]
    public string? RoofingState { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasOpenings)]
    public string? HasOpenings { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasHousingCover)]
    public string? HasHousingCover { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.DisordersObservedCommentary)]
    public string? DisordersObservedCommentary { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Dpe)]
    public string? Dpe { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.Ges)]
    public string? Ges { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AnnualEnergyConsumption)]
    public string? AnnualEnergyConsumption { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AnnualGesEmission)]
    public string? AnnualGesEmission { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HeatingEnergy)]
    public string? HeatingEnergy { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EstimatedDpeAfterWork)]
    public string? EstimatedDpeAfterWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EstimatedAnnualEnergyConsumptionAfterWork)]
    public string? EstimatedAnnualEnergyConsumptionAfterWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EstimatedAnnualGesEmissionsAfterWork)]
    public string? EstimatedAnnualGesEmissionsAfterWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EstimatedGesAfterWork)]
    public string? EstimatedGesAfterWork { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.RenovationType)]
    public string? RenovationType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NextStepAndVigilancePoint)]
    public string? NextStepAndVigilancePoint { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasInterestInPossibleARAProcess)]
    public string? HasInterestInPossibleARAProcess { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasNeedForTemporaryReHousing)]
    public string? HasNeedForTemporaryReHousing { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasEmergencyWorks)]
    public string? HasEmergencyWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasEnergeticsRennovationWorks)]
    public string? HasEnergeticsRenovationWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasInducedWorks)]
    public string? HasInducedWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasSafetyAndHealthWorks)]
    public string? HasSafetyAndHealthWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.TreatedAirTightness)]
    public string? TreatedAirTightness { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.TreatedThermalBridge)]
    public string? TreatedThermalBridge { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AreExistingHumidityAndVaporMigrationManagedAfterTreatment)]
    public string? AreExistingHumidityAndVaporMigrationManagedAfterTreatment { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IsHouseholdReadyToStartARAProcess)]
    public string? IsHouseholdReadyToStartARAProcess { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AreHouseholdPhysicalCapacitiesTakenIntoAccount)]
    public string? AreHouseholdPhysicalCapacitiesTakenIntoAccount { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.DoHouseholdCanMobilizeSocialCircleOnConstructionSite)]
    public string? DoHouseholdCanMobilizeSocialCircleOnConstructionSite { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.WorksDetails)]
    public string? WorksDetails { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdAvailabilitiyToOrganizeARASite)]
    public string? HouseholdAvailabilitiyToOrganizeARASite { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IsRgeLabelUpToDate)]
    public string? IsRgeLabelUpToDate { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.OtherQualification)]
    public string? OtherQualification { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ProjectType)]
    public string? ProjectType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.InsuranceType)]
    public string? InsuranceType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MaPrimeRenovGuidedPath)]
    public string? MaPrimeRenovGuidedPath { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MaPrimeRenovCoOwnerShip)]
    public string? MaPrimeRenovCoOwnerShip { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MaPrimeLogementDecent)]
    public string? MaPrimeLogementDecent { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MaPrimeAdapt)]
    public string? MaPrimeAdapt { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.BonusForExitingEnergeticSieve)]
    public string? BonusForExitingEnergeticSieve { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.RegionalAids)]
    public string? RegionalAids { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.DepartmentalAids)]
    public string? DepartmentalAids { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.PublicEstablishmentsForInterCommunalCooperationAids)]
    public string? PublicEstablishmentsForInterCommunalCooperationAids { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MunicipalityAids)]
    public string? MunicipalityAids { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SolicitedBankLoanType)]
    public string? SolicitedBankLoanType { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NewBorrowingCapacity)]
    public string? NewBorrowingCapacity { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ClassicBankLoan)]
    public string? ClassicBankLoan { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EcoPtz)]
    public string? EcoPTZ { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MdphFinancing)]
    public string? MdphFinancing { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CeeFinancing)]
    public string? CeeFinancing { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CafMsaFinancing)]
    public string? CafMsaFinancing { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.PensionFund)]
    public string? PensionFund { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.UnderprivilegedHousingFoundation)]
    public string? UnderprivilegedHousingFoundation { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.LeroyMerlinFoundation)]
    public string? LeroyMerlinFoundation { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.WattForChangeFoundation)]
    public string? WattForChangeFoundation { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SocialProtectionGroup)]
    public string? SocialProtectionGroup { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdMaximumSavingAmountForRenovationProject)]
    public string? HouseholdMaximumSavingAmountForRenovationProject { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.OtherFamilyMemberMaximumSupportAmountForRenovationProject)]
    public string? OtherFamilyMemberMaximumSupportAmountForRenovationProject { get; set; }

	[Description(CsvDataLabel.CsvColumnsNames.FundingModeLabel)]
	public string? FundingModeLabel { get; set; }

	[Description(CsvDataLabel.CsvColumnsNames.AccompanyingCost)]
    public string? AccompanyingCost { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HouseholdSelfFinancing)]
    public string? HouseholdSelfFinancing { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IntermediateAirtightnessTestResult)]
    public string? IntermediateAirtightnessTestResult { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.JustificationAndActionsPutInPlaceIfNoTest)]
    public string? JustificationAndActionsPutInPlaceIfNoTest { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EffectiveComplianceWithWorkRecommendations)]
    public string? HasEffectiveComplianceWithWorkRecommendations { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasWorksEnabledHouseholdToStayAtHome)]
    public string? HasWorksEnabledHouseholdToStayAtHome { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.WellBeingRating)]
    public string? WellBeingRating { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.EducationalFrameworkRating)]
    public string? EducationalFrameworkRating { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.FamilySatisfaction)]
    public string? FamilySatisfaction { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.ReturnToEmployment)]
    public string? ReturnToEmployment { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasHousingAdaptationWorks)]
    public string? HasHousingAdaptationWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasFinishingWorks)]
    public string? HasFinishingWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasSafetyWorks)]
    public string? HasSafetyWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasPreparationWorks)]
    public string? HasPreparationWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasEmergencyWorksMonitoring)]
    public string? HasEmergencyWorksMonitoring { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasUnsanitaryExit)]
    public string? HasUnsanitaryExit { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.TreatedAirTightnessWorks)]
    public string? TreatedAirTightnessWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.TreatedThermalBridgesWorks)]
    public string? TreatedThermalBridgesWorks { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.HasHumidityManagement)]
    public string? HasHumidityManagement { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.WorkTotalCost)]
    public string? WorkTotalCost { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.SolidarBuilder)]
    public string? SolidarBuilder { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MarkerNature)]
    public string? MarkerNature { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.CommentOnMarkerNature)]
    public string? CommentOnMarkerNature { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.IsDeleted)]
    public string? IsDeleted { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.AccompanyingTimeDurationForIdentificationMilestone)]
    public string? AccompanyingTimeDurationForIdentificationMilestone { get; set; }

	[Description(CsvDataLabel.CsvColumnsNames.AccompanyingTimeDurationForOrganizeAndFinanceMilestone)]
	public string? AccompanyingTimeDurationForOrganizeAndFinanceMilestone { get; set; }

	[Description(CsvDataLabel.CsvColumnsNames.AccompanyingTimeDurationForRealizeAndFollowMilestone)]
	public string? AccompanyingTimeDurationForRealizeAndFollowMilestone { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.NumberOfOccupants)]
    public string? NumberOfOccupants { get; set; }

    [Description(CsvDataLabel.CsvColumnsNames.MonthlyEnergeticsExpenses)]
    public string? MonthlyEnergeticsExpenses { get; set; }

	[Description(CsvDataLabel.CsvColumnsNames.StopEnergyExclusionFunds)]
	public string? StopEnergyExclusionFunds {  get; set; }

}