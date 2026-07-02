using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ExcelExport;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetCompleteAccompanyingFileForExcelExportQueryHandler(
    IAccompanyingFileRepository accompanyingFileRepository,
    IUserRepository userRepository,
    ITelemetryService telemetryService) : QueryHandler<GetCompleteAccompanyingFileForExcelExportQuery, ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>>
{
    public override async Task<ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>> HandleQuery(GetCompleteAccompanyingFileForExcelExportQuery request)
    {
        try
        {
            var user = await userRepository.GetUserById(request.UserId);

            if (user is null)
                return ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>.Failure(Labels.Errors.UserNotFound);

            var accompanyingFiles = request.UserRole switch
            {
                string userRole when userRole == Constants.SolidarBuilderRole || userRole == Constants.TerritorialBuilderRole => await accompanyingFileRepository.GetSolidarOrTerritorialBuilderAccompanyingFileForExcelExport(request.UserId),
                string userRole when userRole == Constants.DiffuseCoordinatorRole || userRole == Constants.TargetedCoordinatorRole => await accompanyingFileRepository.GetAllAccompanyingFileForExcelExport(),
                string userRole when userRole == Constants.StructuralReferentRole =>
                    (user.ReportingStructureNavigation?.NationalStructureId != null) ? await accompanyingFileRepository.GetStructuralReferentAccompanyingFileForExcelExport((Guid)user.ReportingStructureNavigation.NationalStructureId) : null,
                _ => null
            };

            if (accompanyingFiles is null)
                return ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFiles);

            var excelData = new List<AccompanyingFileExcelExportDto>();

            foreach (var accompanyingFile in accompanyingFiles)
            {
                var excelExportDto = new AccompanyingFileExcelExportDto();

                AdaptAccompanyingForExcelExport(accompanyingFile, excelExportDto);
                AdaptSupportTeamSection(accompanyingFile, excelExportDto);
                AdaptHouseholdSection(accompanyingFile, excelExportDto);
                AdaptHousingSection(accompanyingFile, excelExportDto);
                AdaptPreWorkPlanSection(accompanyingFile, excelExportDto);
                AdaptPreFinancingSection(accompanyingFile, excelExportDto);
                AdaptWorkMonitoringSection(accompanyingFile, excelExportDto);
                AdaptSiteSupervisionSection(accompanyingFile, excelExportDto);

                excelData.Add(excelExportDto);
            }

            return ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>.Success(new GetCompleteAccompanyingFileForExcelExportQueryObjectResult(excelData));
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);

            return ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>.Failure(ExcelDataLabel.Errors.ErrorWhileGeneratingExcelFile);
        }
    }

    private static void AdaptAccompanyingForExcelExport(Domain.Entity.AccompanyingFile accompanyingFile, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.AccompanyingFileReference = accompanyingFile.AccompanyingFileReference;
        accompanyingExport.AccompanyingFileMilestone = EnumHelper.GetDescription((AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone);
        accompanyingExport.AccompanyingFileStatus = EnumHelper.GetDescription((AccompanyingFileStatus)accompanyingFile.AccompanyingFileStatus);
        accompanyingExport.AbortReasonLabel = accompanyingFile.AbortReasonLabel?.Label == Labels.Other
            ? accompanyingFile.OtherAbortReason
            : accompanyingFile.AbortReasonLabel?.Label;
        accompanyingExport.FirstEncounterDate = accompanyingFile.FirstEncounterDate;
        accompanyingExport.StartOfAccompanyingDate = accompanyingFile.StartOfAccompanyingDate;
        accompanyingExport.NumberOfEncounterWithFamillyForIdentificationMilestone = accompanyingFile.NumberOfEncounterWithFamilyForIdentificationMilestone;
        accompanyingExport.OpeningDate = accompanyingFile.OpeningDate;
        accompanyingExport.CloseDate = accompanyingFile.CloseDate;
        accompanyingExport.NumberOfEncounterWithFamillyForOrganizeAndFinanceMilestone = accompanyingFile.NumberOfEncounterWithFamilyForOrganizeAndFinanceMilestone;
        accompanyingExport.NumberOfEncounterWithFamillyForRealizeAndFollowMilestone = accompanyingFile.NumberOfEncounterWithFamilyForRealizeAndFollowMilestone;
        accompanyingExport.EndOfAccompanyingDate = accompanyingFile.EndOfEncounterDate;
        accompanyingExport.EndOfEncounterDate = accompanyingFile.EndOfEncounterDate;
        accompanyingExport.ZeroEnergyExclusionTerritoriesProgram = (bool)accompanyingFile.ZeroEnergyExclusionTerritoriesProgram! ? "Oui" : "Non";
        accompanyingExport.AccompanyingType = accompanyingFile.AccompanyingType is not null ?
                    EnumHelper.GetDescription((AccompanyingType)accompanyingFile.AccompanyingType) :
                    string.Empty;
        accompanyingExport.AccompanyingFileTerritory = (accompanyingFile.AccompanyingType is not null && accompanyingFile.AccompanyingType == 0 && accompanyingFile.AccompanyingFileTerritoryNavigation is not null) ?
                    accompanyingFile.AccompanyingFileTerritoryNavigation.Label :
                    string.Empty;
        accompanyingExport.DeliveryTime = accompanyingFile.DeliveryTime;
        accompanyingExport.IdentifySynthesisValidationDate = accompanyingFile.IdentifySynthesisValidationDate;
        accompanyingExport.OrganizeAndFinanceSynthesisValidationDate = accompanyingFile.OrganizeAndFinanceSynthesisValidationDate;
        accompanyingExport.RealizeAndFollowSynthesisValidationDate = accompanyingFile.RealizeAndFollowSynthesisValidationDate;
        accompanyingExport.ShouldAccompanyingFileBeSubmittedToAnah = StringHelper.BoolToString(accompanyingFile.ShouldAccompanyingFileBeSubmittedToAnah);
        accompanyingExport.AnahFolderNumber = accompanyingFile.AnahFolderNumber;
        accompanyingExport.AnahFolderFilingDate = accompanyingFile.AnahFolderFilingDate;
        accompanyingExport.AccompanyingTimeDurationForIdentificationMilestone = EnumHelper.GetDescription((AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForIdentificationMilestone);
        accompanyingExport.AccompanyingTimeDurationForOrganizeAndFinanceMilestone = EnumHelper.GetDescription((AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForOrganizeAndFinanceMilestone);
        accompanyingExport.AccompanyingTimeDurationForRealizeAndFollowMilestone = EnumHelper.GetDescription((AccompanyingTimeDuration?)accompanyingFile.AccompanyingTimeDurationForRealizeAndFollowMilestone);
        accompanyingExport.AbortTypeLabel = GetAbortTypeLabel(accompanyingFile);
        accompanyingExport.AbortRequestDetails = accompanyingFile.SolidarBuilderAbortRequestDetails;
        accompanyingExport.HasAbortAttachment = StringHelper.BoolToString(accompanyingFile.HasAbortAttachment);
        accompanyingExport.IsBillingRequested = StringHelper.BoolToString(accompanyingFile.IsAbortBillingRequested);
        accompanyingExport.ValidatorName = $"{accompanyingFile.AbortDecidedBy?.FirstName} {accompanyingFile.AbortDecidedBy?.LastName}";
        accompanyingExport.ValidatorRole = accompanyingFile.AbortDecidedBy?.Role.LongName;
        accompanyingExport.ValidatorAbortComment = accompanyingFile.ValidatorAbortComment;
        accompanyingExport.DecisionDate = accompanyingFile.AbortDecidedAt;
        accompanyingExport.AnahGrantDate = accompanyingFile.AnahGrantDate;
    }

    private static void AdaptAccompanyingFileHouseholdForExcelExport(Domain.Entity.Household household, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.HouseholdTypology = household.HouseholdTypology is not null ?
                    EnumHelper.GetDescription((HouseholdTypology)household.HouseholdTypology) :
                    string.Empty;
        accompanyingExport.IsFollowedByAnSocialWorker = StringHelper.BoolToString(household.IsFollowedByAnSocialWorker);
        accompanyingExport.HasAnOccupantWithDisabilities = StringHelper.BoolToString(household.HasAnOccupantWithDisabilities);
        accompanyingExport.HasAnOccupantWithLongTermIllness = StringHelper.BoolToString(household.HasAnOccupantWithLongTermIllness);
        accompanyingExport.HasAnOccupantWithIndependenceLoss = StringHelper.BoolToString(household.HasAnOccupantWithIndependenceLoss);
        accompanyingExport.HasAnOccupantUnderCuratorship = StringHelper.BoolToString(household.HasAnOccupantUnderCuratorship);
        accompanyingExport.HasAnOccupantUnderGardianship = StringHelper.BoolToString(household.HasAnOccupantUnderGuardianship);
        accompanyingExport.ReferenceIncomeTax = household.ReferenceIncomeTax;
        accompanyingExport.AnahCategory = household.AnahCategory;
        accompanyingExport.SocialContext = household.SocialContext;
        accompanyingExport.HouseholdProject = household.HouseholdProject;
        accompanyingExport.HouseholdAvailabilityForVisits = household.HouseholdAvailabilityForVisits;
        accompanyingExport.CommentsOnHouseholdDifficulties = household.CommentsOnHouseholdDifficulties;
        accompanyingExport.HasOverdueInvoice = StringHelper.BoolToString(household.HasOverdueInvoice);
        accompanyingExport.EnergyEffortRate = household.EnergyEffortRate;

        var difficulties = household.HouseholdDifficulties.Select(d => d.DifficultyNavigation.Labels).ToList();
        accompanyingExport.HouseholdDifficulties = string.Join("; ", difficulties);

        var expenses = household.HouseholdExpenses.Select(ex => $"{EnumHelper.GetDescription((ExpenseType)ex.Type)}: {ex.Value}").ToList();
        accompanyingExport.HouseholdExpenses = string.Join("; ", expenses);

        var ressources = household.HouseholdResources.Select(r => $"{r.HouseholdResourcesNavigation.Labels} : {r.Value}€ ").ToList();
        accompanyingExport.HouseholdResources = string.Join("; ", ressources);
    }

    private static void AdaptMainOccupantForExcelExport(Domain.Entity.MainOccupant mainOccupant, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.Trigram = mainOccupant.Trigram;
        accompanyingExport.Birthdate = mainOccupant.Birthdate;
        accompanyingExport.Age = mainOccupant.Age;
        accompanyingExport.SocioProfessionalCategory = mainOccupant.SocioProfessionalCategory is not null ?
                    EnumHelper.GetDescription((SocioProfessionalCategory)mainOccupant.SocioProfessionalCategory) :
                    string.Empty;
        accompanyingExport.PhoneNumber = mainOccupant.PhoneNumber;
        accompanyingExport.Email = mainOccupant.Email;
        accompanyingExport.Job = mainOccupant.Job;
        accompanyingExport.SocialProtectionFund = mainOccupant.SocialProtectionFund is not null ?
                    EnumHelper.GetDescription((SocialProtectionFund)mainOccupant.SocialProtectionFund) :
                    string.Empty;
        accompanyingExport.CommentOnSocialProtectionFund = mainOccupant.CommentOnSocialProtectionFund;
        accompanyingExport.PensionFundName = mainOccupant.PensionFund is not null ?
                    EnumHelper.GetDescription((PensionFund)mainOccupant.PensionFund) :
                    string.Empty;
        accompanyingExport.CommentOnPensionFund = mainOccupant.CommentOnPensionFund;
        accompanyingExport.AdditionnalFund = mainOccupant.AdditionnalFund is not null ?
                    EnumHelper.GetDescription((AdditionalFund)mainOccupant.AdditionnalFund) :
                    string.Empty;
        accompanyingExport.CommentOnAdditionnalFund = mainOccupant.CommentOnAdditionnalFund;
        accompanyingExport.FirstName = mainOccupant.FirstName;
        accompanyingExport.LastName = mainOccupant.LastName;
    }

    private static void AdaptHousingForExcelExport(Domain.Entity.Housing housing, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.GeographicAreaTypology = housing.GeographicAreaTypology is not null ?
                    EnumHelper.GetDescription((GeographicalHousingAreaTypology)housing.GeographicAreaTypology) :
                    string.Empty;
        accompanyingExport.IsInAbfarea = StringHelper.BoolToString(housing.IsInAbfarea);
        accompanyingExport.ArchitecturalOrTownPlanningStandards = housing.ArchitecturalOrTownPlanningStandards;
        accompanyingExport.OwnershipStatus = housing.OwnershipStatus is not null ?
                    EnumHelper.GetDescription((OwnershipStatus)housing.OwnershipStatus) :
                    string.Empty;
        accompanyingExport.HousingType = housing.HousingType is not null ?
                    EnumHelper.GetDescription((HousingType)housing.HousingType) :
                    string.Empty;
        accompanyingExport.ConstructionYear = housing.ConstructionYear is not null ?
            EnumHelper.GetDescription((HousingYearConstruction)housing.ConstructionYear) :
            string.Empty;
        accompanyingExport.LivingSpace = housing.LivingSpace;
        accompanyingExport.NumberOfRoom = housing.NumberOfRoom;
        accompanyingExport.NumberOfFloor = housing.NumberOfFloor;
        accompanyingExport.YearOfAcquisitionOrEntry = housing.YearOfAcquisitionOrEntry;
        accompanyingExport.CadastralReference = housing.CadastralReference;
        accompanyingExport.SunExposure = housing.SunExposure is not null ?
                    EnumHelper.GetDescription((SunExposure)housing.SunExposure) :
                    string.Empty;
        accompanyingExport.NumberOfDoor = housing.NumberOfDoor;
        accompanyingExport.NumberOfWindow = housing.NumberOfWindow;
        accompanyingExport.NumberOfPatioDoor = housing.NumberOfPatioDoor;
        accompanyingExport.NumberOfRoofDoor = housing.NumberOfRoofDoor;
        accompanyingExport.NumberOfBayWindow = housing.NumberOfBayWindow;
        accompanyingExport.CeilingHeight = housing.CeilingHeight;
        accompanyingExport.HasPreviousWork = StringHelper.BoolToString(housing.HasPreviousWork);
        accompanyingExport.CommentOnPreviousWork = housing.CommentOnPreviousWork;
    }

    private static void AdaptAdressForExcelExport(Domain.Entity.Address address, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.Label = address.Label;
        accompanyingExport.PostalCode = address.PostalCode;
        accompanyingExport.City = address.City;
        accompanyingExport.Department = address.Department;
        accompanyingExport.Region = address.Region;
        accompanyingExport.AdditionalComment = address.AdditionnalComment;
    }

    private static void AdaptInitialHouseStateForExcelExport(Domain.Entity.HousingInitialState housingInitialState, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.DegradationIndex = housingInitialState.DegradationIndex is not null ?
                    EnumHelper.GetDescription((DegradationIndex)housingInitialState.DegradationIndex) :
                    string.Empty;
        accompanyingExport.UnsanitaryCoefficient = housingInitialState.UnsanitaryCoefficient is not null ?
                    EnumHelper.GetDescription((UnsanitaryCoefficient)housingInitialState.UnsanitaryCoefficient) :
                    string.Empty;
        accompanyingExport.EnergyDepravation = housingInitialState.EnergyDepravation is not null ?
                    EnumHelper.GetDescription((EnergyDeprivation)housingInitialState.EnergyDepravation) :
                    string.Empty;
        accompanyingExport.SummerThermalComfortLevel = housingInitialState.SummerThermalComfortLevel is not null ?
                    EnumHelper.GetDescription((ComfortLevel)housingInitialState.SummerThermalComfortLevel) :
                    string.Empty;
        accompanyingExport.WinterThermalComfortLevel = housingInitialState.WinterThermalComfortLevel is not null ?
                    EnumHelper.GetDescription((ComfortLevel)housingInitialState.WinterThermalComfortLevel) :
                    string.Empty;
        accompanyingExport.NoiseComfortLevel = housingInitialState.NoiseComfortLevel is not null ?
                    EnumHelper.GetDescription((ComfortLevel)housingInitialState.NoiseComfortLevel) :
                    string.Empty;
        accompanyingExport.HasPestOrMold = StringHelper.BoolToString(housingInitialState.HasPestOrMold);
        accompanyingExport.HasFaultyElectricalSystem = StringHelper.BoolToString(housingInitialState.HasFaultyElectricalSystem);
        accompanyingExport.HasVentilationSystem = StringHelper.BoolToString(housingInitialState.HasVentilationSystem);
        accompanyingExport.HasHeatingSystem = StringHelper.BoolToString(housingInitialState.HasHeatingSystem);
        accompanyingExport.HasHotWaterProduction = StringHelper.BoolToString(housingInitialState.HasHotWaterProduction);
        accompanyingExport.HasHousingCover = StringHelper.BoolToString(housingInitialState.HasHousingCover);
        accompanyingExport.HasOpenings = StringHelper.BoolToString(housingInitialState.HasOpenings);
        accompanyingExport.HasInsulation = StringHelper.BoolToString(housingInitialState.HasInsulation);
        accompanyingExport.DisordersObservedCommentary = housingInitialState.DisordersObservedCommentary;
        accompanyingExport.Dpe = housingInitialState.Dpe is not null ?
                    EnumHelper.GetDescription((DpeLabel)housingInitialState.Dpe) :
                    string.Empty;
        accompanyingExport.Ges = housingInitialState.Ges is not null ?
                    EnumHelper.GetDescription((GesLabel)housingInitialState.Ges) :
                    string.Empty;
        accompanyingExport.AnnualEnergyConsumption = housingInitialState.AnnualEnergyConsumption;
        accompanyingExport.AnnualGesemission = housingInitialState.AnnualGesemission;
        accompanyingExport.HeatingEnergy = housingInitialState.HeatingEnergy;
    }

    private static void AdaptAfterWorkHouseStateForExcelExport(Domain.Entity.HousingAfterWorkState housingAfterWorkState, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.EstimatedDpeclassJump = housingAfterWorkState.EstimatedDpeclassJump is not null ?
                    EnumHelper.GetDescription((EstimatedJumpClass)housingAfterWorkState.EstimatedDpeclassJump) :
                    string.Empty;
        accompanyingExport.EstimatedDpeafterWork = housingAfterWorkState.EstimatedDpeafterWork is not null ?
                    EnumHelper.GetDescription((DpeLabel)housingAfterWorkState.EstimatedDpeafterWork) :
                    string.Empty;
        accompanyingExport.EstimatedAnnualEnergyConsumptionAfterWork = housingAfterWorkState.EstimatedAnnualEnergyConsumptionAfterWork;
        accompanyingExport.EstimatedAnnualGesemissionsAfterWork = housingAfterWorkState.EstimatedAnnualGesemissionsAfterWork;
        accompanyingExport.EstimatedGesafterWork = housingAfterWorkState.EstimatedGesafterWork is not null ?
                    EnumHelper.GetDescription((GesLabel)housingAfterWorkState.EstimatedGesafterWork) :
                    string.Empty;
        accompanyingExport.FinalDpe = housingAfterWorkState.FinalDpe is not null ?
                    EnumHelper.GetDescription((DpeLabel)housingAfterWorkState.FinalDpe) :
                    string.Empty;
        accompanyingExport.FinalDpeClassJump = housingAfterWorkState.FinalDpeClassJump is not null ?
                    EnumHelper.GetDescription((EstimatedJumpClass)housingAfterWorkState.FinalDpeClassJump) :
                    string.Empty;
    }

    private static void AdaptWorkMonitoringForExcelExport(Domain.Entity.WorkMonitoring workMonitoring, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.AccompanyingCost = workMonitoring.AccompanyingCost;
        accompanyingExport.HouseholdSelfFinancing = workMonitoring.HouseholdSelfFinancing;
        accompanyingExport.IntermediateAirtightnessTestResult = workMonitoring.IntermediateAirtightnessTestResult;
        accompanyingExport.JustificationAndActionsPutInPlaceIfNoTest = workMonitoring.JustificationAndActionsPutInPlaceIfNoTest;
        accompanyingExport.HasEffectiveComplianceWithWorkRecommendations = StringHelper.BoolToString(workMonitoring.HasEffectiveComplianceWithWorkRecommendations);
        accompanyingExport.HasWorksEnabledHouseholdToStayAtHome = StringHelper.BoolToString(workMonitoring.HasWorksEnabledHouseholdToStayAtHome);
        accompanyingExport.WellBeingRating = workMonitoring.WellBeingRating;
        accompanyingExport.EducationnalFrameworkRating = workMonitoring.EducationalFrameworkRating;
        accompanyingExport.FamilySatisfaction = workMonitoring.FamilySatisfaction;
        accompanyingExport.ReturnToEmployment = StringHelper.BoolToString(workMonitoring.ReturnToEmployment);
        accompanyingExport.HasHousingAdaptationWorks = StringHelper.BoolToString(workMonitoring.HasHousingAdaptationWorks);
        accompanyingExport.HasFinishingWorks = StringHelper.BoolToString(workMonitoring.HasFinishingWorks);
        accompanyingExport.HasSafetyWorks = StringHelper.BoolToString(workMonitoring.HasSafetyWorks);
        accompanyingExport.HasPreparationWorks = StringHelper.BoolToString(workMonitoring.HasPreparationWorks);
        accompanyingExport.HasEmergencyWorks = StringHelper.BoolToString(workMonitoring.HasEmergencyWorks);
        accompanyingExport.HasUnsanitaryExit = StringHelper.BoolToString(workMonitoring.HasUnsanitaryExit);
        accompanyingExport.TreatedAirTightness = workMonitoring.TreatedAirTightness is not null ?
                            EnumHelper.GetDescription((PartlyStateTreatment)workMonitoring.TreatedAirTightness) :
                            string.Empty;
        accompanyingExport.TreatedThermalBridges = workMonitoring.TreatedThermalBridges is not null ?
                            EnumHelper.GetDescription((PartlyStateTreatment)workMonitoring.TreatedThermalBridges) :
                            string.Empty;
        accompanyingExport.HasHumidityManagement = StringHelper.BoolToString(workMonitoring.HasHumidityManagement);
        accompanyingExport.WorkTotalCost = workMonitoring.WorkTotalCost;
        accompanyingExport.ShouldChangeFinalEstimatedDpe = StringHelper.BoolToString(workMonitoring.ShouldChangeFinalEstimatedDpe);
    }

    private static void AdaptPrefinancingPlanForExcelExport(Domain.Entity.PreFinancingPlan preFinancingPlan, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.MaPrimeRenoveGuidedPath = preFinancingPlan.MaPrimeRenovGuidedPath;
        accompanyingExport.MaPrimeRenoveCoOwnerShip = preFinancingPlan.MaPrimeRenovCoOwnerShip;
        accompanyingExport.MaPrimeLogementDecent = preFinancingPlan.MaPrimeLogementDecent;
        accompanyingExport.MaPrimeAdapt = preFinancingPlan.MaPrimeAdapt;
        accompanyingExport.BonusForExitingEnergeticSieve = preFinancingPlan.BonusForExitingEnergeticSieve;
        accompanyingExport.RegionalAids = preFinancingPlan.RegionalAids;
        accompanyingExport.DepartmentalAids = preFinancingPlan.DepartmentalAids;
        accompanyingExport.PublicEstablishmentsForInterCommunalCooperationAids = preFinancingPlan.PublicEstablishmentsForInterCommunalCooperationAids;
        accompanyingExport.MunicipalityAids = preFinancingPlan.MunicipalityAids;
        accompanyingExport.SolicitedBankLoanType = preFinancingPlan.SolicitedBankLoanType;
        accompanyingExport.ClassicBankLoan = preFinancingPlan.ClassicBankLoan;
        accompanyingExport.MdphFinancing = preFinancingPlan.MdphFinancing;
        accompanyingExport.CeeFinancing = preFinancingPlan.CeeFinancing;
        accompanyingExport.CafMsaFinancing = preFinancingPlan.CafMsaFinancing;
        accompanyingExport.PensionFund = preFinancingPlan.PensionFund;
        accompanyingExport.AbePierreFundation = preFinancingPlan.UnderprivilegedHousingFoundation;
        accompanyingExport.LeroyMerlinFundation = preFinancingPlan.LeroyMerlinFoundation;
        accompanyingExport.WattForChangeFundation = preFinancingPlan.WattForChangeFoundation;
        accompanyingExport.SocialProtectionGroup = preFinancingPlan.SocialProtectionGroup;
        accompanyingExport.HouseholdMaximumSavingAmountForRenovationProject = preFinancingPlan.HouseholdMaximumSavingAmountForRenovationProject;
        accompanyingExport.OtherFamilyMemberMaximumSupportAmountForRenovationProject = preFinancingPlan.OtherFamilyMemberMaximumSupportAmountForRenovationProject;

        var fundingMode = preFinancingPlan.FundingModes.Select(fm => $"{fm.Label}: {fm.Value}€").ToList();
        accompanyingExport.FundingMode = string.Join("; ", fundingMode);
    }

    private static void AdaptPreWorkPlanForExcelNavigation(Domain.Entity.PreWorkPlan preWorkPlan, AccompanyingFileExcelExportDto accompanyingExport)
    {
        accompanyingExport.RenovationType = preWorkPlan.RenovationType is not null ?
                    EnumHelper.GetDescription((RenovationType)preWorkPlan.RenovationType) :
                    string.Empty;
        accompanyingExport.NextStepAndVigilancePoint = preWorkPlan.NextStepAndVigilancePoint;
        accompanyingExport.HasInterestInPossibleAraprocess = StringHelper.BoolToString(preWorkPlan.HasInterestInPossibleAraprocess);
        accompanyingExport.HasNeedForTemporaryReHousing = StringHelper.BoolToString(preWorkPlan.HasNeedForTemporaryReHousing);
        accompanyingExport.HasEmergencyWorksPreWorkPlan = StringHelper.BoolToString(preWorkPlan.HasEmergencyWorks);
        accompanyingExport.HasEnergeticsRenovationWorks = StringHelper.BoolToString(preWorkPlan.HasEnergeticsRenovationWorks);
        accompanyingExport.HasInducedWorks = StringHelper.BoolToString(preWorkPlan.HasInducedWorks);
        accompanyingExport.HasSafetyAndHealthWorks = StringHelper.BoolToString(preWorkPlan.HasSafetyAndHealthWorks);
        accompanyingExport.TreatedAirTightnessPreworkPlan = preWorkPlan.TreatedAirTightness is not null ?
                    EnumHelper.GetDescription((PartlyStateTreatment)preWorkPlan.TreatedAirTightness) :
                    string.Empty;
        accompanyingExport.TreatedThermalBridge = preWorkPlan.TreatedThermalBridge is not null ?
                    EnumHelper.GetDescription((PartlyStateTreatment)preWorkPlan.TreatedThermalBridge) :
                    string.Empty;
        accompanyingExport.AreExistingHumidityAndVaporMigrationManagedAfterTreatment = preWorkPlan.AreExistingHumidityAndVaporMigrationManagedAfterTreatment is not null ?
                    EnumHelper.GetDescription((PartlyStateTreatment)preWorkPlan.AreExistingHumidityAndVaporMigrationManagedAfterTreatment) :
                    string.Empty;
        accompanyingExport.IsHouseholdReadyToStartAraprocess = StringHelper.BoolToString(preWorkPlan.IsHouseholdReadyToStartAraprocess);
        accompanyingExport.AreHouseholdPhysicalCapacitiesTakenIntoAccount = StringHelper.BoolToString(preWorkPlan.AreHouseholdPhysicalCapacitiesTakenIntoAccount);
        accompanyingExport.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean = StringHelper.BoolToString(preWorkPlan.DoHouseholdCanMobilizeSocialCircleOnConstructionSiteBoolean);
        accompanyingExport.WorksDetails = preWorkPlan.WorksDetails;
        accompanyingExport.HouseholdAvailabilitiyToOrganizeArasite = preWorkPlan.HouseholdAvailabilitiyToOrganizeArasite;
        accompanyingExport.IsRgeLabelUpToDate = StringHelper.BoolToString(preWorkPlan.IsRgeLabelUpToDate);
        accompanyingExport.OtherQualification = preWorkPlan.OtherQualification;

        var projectTypesList = preWorkPlan.PreWorkPlanProjectTypes.Select(p => p.ProjectTypeNavigation.Label).ToList();
        accompanyingExport.ProjectType = string.Join(";", projectTypesList);

        var insuranceTypes = preWorkPlan.PreWorkPlanInsuranceTypes.Select(p => p.InsuranceTypeNavigation.Label).ToList();
        accompanyingExport.InsuranceType = string.Join(";", insuranceTypes);
    }

    private static void AdaptHouseholdSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        var household = file.AccompanyingFileHouseholdNavigation;
        if (household is null) return;

        AdaptAccompanyingFileHouseholdForExcelExport(household, dto);

        if (household.MainOccupantNavigation is not null)
            AdaptMainOccupantForExcelExport(household.MainOccupantNavigation, dto);
    }

    private static void AdaptHousingSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        var housing = file.AccompanyingFileHousingNavigation;
        if (housing is null) return;

        AdaptHousingForExcelExport(housing, dto);

        if (housing.HousingAddressNavigation is not null)
            AdaptAdressForExcelExport(housing.HousingAddressNavigation, dto);

        if (housing.HousingInitialStateNavigation is not null)
            AdaptInitialHouseStateForExcelExport(housing.HousingInitialStateNavigation, dto);

        if (housing.HousingAfterWorkStateNavigation is not null)
            AdaptAfterWorkHouseStateForExcelExport(housing.HousingAfterWorkStateNavigation, dto);
    }

    private static void AdaptPreFinancingSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        if (file.AccompanyingFilePreFinancingPlanNavigation is not null)
            AdaptPrefinancingPlanForExcelExport(file.AccompanyingFilePreFinancingPlanNavigation, dto);
    }

    private static void AdaptWorkMonitoringSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        if (file.AccompanyingFileWorkMonitoringNavigation is not null)
            AdaptWorkMonitoringForExcelExport(file.AccompanyingFileWorkMonitoringNavigation, dto);
    }

    private static void AdaptPreWorkPlanSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        var preWorkPlan = file.AccompanyingFilePreWorkPlanNavigation;
        if (preWorkPlan is null) return;

        AdaptPreWorkPlanForExcelNavigation(preWorkPlan, dto);
    }

    private static void AdaptSupportTeamSection(Domain.Entity.AccompanyingFile file, AccompanyingFileExcelExportDto dto)
    {
        dto.ReportingStructure = file.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation?.Name;
        dto.SolidarBuilder = file.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.Email ?? string.Empty;
        dto.SecondSolidarBuilder = file.AccompanyingFileSupportTeamNavigation.SecondSolidarBuilderNavigation?.Email;
        dto.ThirdSolidarBuilder = file.AccompanyingFileSupportTeamNavigation.ThirdSolidarBuilderNavigation?.Email;
        dto.TerritorialBuilder = file.AccompanyingFileSupportTeamNavigation.TerritorialBuilderNavigation?.Email;
        dto.SecondTerritorialBuilder = file.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilderNavigation?.Email;
        dto.Coordinator = (AccompanyingType?)file.AccompanyingType switch
        {
            AccompanyingType.Diffuse => file.AccompanyingFileSupportTeamNavigation.DiffuseCoordinatorNavigation?.Email,
            AccompanyingType.Targeted => file.AccompanyingFileSupportTeamNavigation.TargetCoordinatorNavigation?.Email,
            _ => null
        };
    }

    private static string? GetAbortTypeLabel(Domain.Entity.AccompanyingFile accompanyingFile)
    {
        if (accompanyingFile.AccompanyingFileStatus != (int)AccompanyingFileStatus.Aborted)
            return null;

        if (accompanyingFile.IsAbortBillingRequested == true)
            return string.Format(Labels.AbortWithBillingRequestLabel, (int?)accompanyingFile.AccompanyingFileMilestone + 1);

        if (accompanyingFile.IsAbortBillingRequested == false)
            return Labels.AbortWithoutBillingRequestLabel;

        return null;
    }

    private static void AdaptSiteSupervisionSection(Domain.Entity.AccompanyingFile accompanyingFile, AccompanyingFileExcelExportDto dto)
    {
        dto.PreSiteSupervisionMeetingDate = accompanyingFile.SiteSupervision?.PreSiteSupervisionMeetingDate;
        dto.OverallStartDate = accompanyingFile.SiteSupervision?.OverallStartDate;
        dto.ActualOverallEndDate = accompanyingFile.SiteSupervision?.ActualOverallEndDate;
    }
}