using Renee.Application.Helpers;
using Renee.Domain;
using Renee.Domain.Enums;
using System.Linq.Expressions;

namespace Renee.Application.Services.Administration.ImportCsvData;

public static class CsvDataParser
{
	private sealed class FieldParser
	{
		private readonly List<string> _errors = [];

		public List<string> GetErrors() => _errors;

		private void AddError(string? input, string fieldName)
		{
			if (!string.IsNullOrWhiteSpace(input))
				_errors.Add(string.Format(CsvDataLabel.Errors.FormatError, fieldName, input));
		}

		public T? ParseValue<T>(string? input, string fieldName) where T : struct
		{
			if (typeof(T) == typeof(bool))
				return (T?)(object?)ParseBool(input, fieldName);

			if (ImportFileDataHelper.TryParseValue<T>(input, out var val))
				return val;

			AddError(input, fieldName);
			return null;
		}

		public bool? ParseBool(string? input, string fieldName)
		{
			if (ImportFileDataHelper.TryParseValue<bool?>(input, out var val))
				return val;

			AddError(input, fieldName);
			return null;
		}

		public DateTime? ParseDate(string? input, string fieldName)
			=> ParseValue<DateTime>(input, fieldName); 

		public int? ParseInt(string? input, string fieldName)
			=> ParseValue<int>(input, fieldName);

		public double? ParseDouble(string? input, string fieldName)
			=> ParseValue<double>(input, fieldName);

		public T? ParseEnum<T>(string? input, string fieldName) where T : struct
			=> ParseValue<T>(input, fieldName);

		public static string CsvName<TProp>(Expression<Func<CsvData, TProp>> expr) => CsvHelperExtensions.CsvName(expr);

		public HousingYearConstruction? GetHousingYearConstructionFromYear(string? year)
		{
			var yearToInt = ParseInt(year, FieldParser.CsvName(x => x.ConstructionYear));

			if (yearToInt is null)
				return null;

			return yearToInt switch
			{
				<= 1947 => HousingYearConstruction.HouseConstructionPeriod1,
				<= 1974 => HousingYearConstruction.HouseConstructionPeriod2,
				<= 1977 => HousingYearConstruction.HouseConstructionPeriod3,
				<= 1982 => HousingYearConstruction.HouseConstructionPeriod4,
				<= 1988 => HousingYearConstruction.HouseConstructionPeriod5,
				<= 2000 => HousingYearConstruction.HouseConstructionPeriod6,
				<= 2005 => HousingYearConstruction.HouseConstructionPeriod7,
				<= 2012 => HousingYearConstruction.HouseConstructionPeriod8,
				<= 2020 => HousingYearConstruction.HouseConstructionPeriod9,
				_ => HousingYearConstruction.HouseConstructionPeriod10
			};
		}

	}

	public static ParsedCsvData Parse(CsvData source, out List<string> errors, int lineNumber)
	{
		var parser = new FieldParser();

		static string? ReturnStringValueOrNull(string? value) => string.IsNullOrEmpty(value) ? null : value.Trim();

		var result = new ParsedCsvData
			{
				LineNumber = lineNumber,
				//Map Enum properties 
				AccompanyingType = parser.ParseEnum<AccompanyingType>(source.AccompanyingType, FieldParser.CsvName(x => x.AccompanyingType)),
				HouseholdTypology = parser.ParseEnum<HouseholdTypology>(source.HouseholdTypology, FieldParser.CsvName(x => x.HouseholdTypology)),
				SocioProfessionalCategory = parser.ParseEnum<SocioProfessionalCategory>(source.SocioProfessionalCategory, FieldParser.CsvName(x => x.SocioProfessionalCategory)),
				SocialProtectionFund = parser.ParseEnum<SocialProtectionFund>(source.SocialProtectionFund, FieldParser.CsvName(x => x.SocialProtectionFund)),
				PensionFundOccupant = parser.ParseEnum<PensionFund>(source.PensionFundOccupant, FieldParser.CsvName(x => x.PensionFundOccupant)),
				AdditionnalFund = parser.ParseEnum<AdditionalFund>(source.AdditionnalFund, FieldParser.CsvName(x => x.AdditionnalFund)),
				GeographicAreaTypology = parser.ParseEnum<GeographicalHousingAreaTypology>(source.GeographicAreaTypology, FieldParser.CsvName(x => x.GeographicAreaTypology)),
				OwnershipStatus = parser.ParseEnum<OwnershipStatus>(source.OwnershipStatus, FieldParser.CsvName(x => x.OwnershipStatus)),
				HousingType = parser.ParseEnum<HousingType>(source.HousingType, FieldParser.CsvName(x => x.HousingType)),
				SunExposure = parser.ParseEnum<SunExposure>(source.SunExposure, FieldParser.CsvName(x => x.SunExposure)),
				DegradationIndex = parser.ParseEnum<DegradationIndex>(source.DegradationIndex, FieldParser.CsvName(x => x.DegradationIndex)),
				UnsanitaryCoefficient = parser.ParseEnum<UnsanitaryCoefficient>(source.UnsanitaryCoefficient, FieldParser.CsvName(x => x.UnsanitaryCoefficient)),
				EnergyDepravation = parser.ParseEnum<EnergyDeprivation>(source.EnergyDepravation, FieldParser.CsvName(x => x.EnergyDepravation)),
				SummerThermalComfortLevel = parser.ParseEnum<ComfortLevel>(source.SummerThermalComfortLevel, FieldParser.CsvName(x => x.SummerThermalComfortLevel)),
				WinterThermalComfortLevel = parser.ParseEnum<ComfortLevel>(source.WinterThermalComfortLevel, FieldParser.CsvName(x => x.WinterThermalComfortLevel)),
				NoiseComfortLevel = parser.ParseEnum<ComfortLevel>(source.NoiseComfortLevel, FieldParser.CsvName(x => x.NoiseComfortLevel)),
				Dpe = parser.ParseEnum<DpeLabel>(source.Dpe, FieldParser.CsvName(x => x.Dpe)),
				Ges = parser.ParseEnum<GesLabel>(source.Ges, FieldParser.CsvName(x => x.Ges)),
				EstimatedDpeAfterWork = parser.ParseEnum<DpeLabel>(source.EstimatedDpeAfterWork, FieldParser.CsvName(x => x.EstimatedDpeAfterWork)),
				EstimatedGesAfterWork = parser.ParseEnum<GesLabel>(source.EstimatedGesAfterWork, FieldParser.CsvName(x => x.EstimatedGesAfterWork)),
				RenovationType = parser.ParseEnum<RenovationType>(source.RenovationType, FieldParser.CsvName(x => x.RenovationType)),
				TreatedAirTightness = parser.ParseEnum<PartlyStateTreatment>(source.TreatedAirTightness, FieldParser.CsvName(x => x.TreatedAirTightness)),
				TreatedThermalBridge = parser.ParseEnum<PartlyStateTreatment>(source.TreatedThermalBridge, FieldParser.CsvName(x => x.TreatedThermalBridge)),
				AreExistingHumidityAndVaporMigrationManagedAfterTreatment = parser.ParseEnum<PartlyStateTreatment>(source.AreExistingHumidityAndVaporMigrationManagedAfterTreatment, FieldParser.CsvName(x => x.AreExistingHumidityAndVaporMigrationManagedAfterTreatment)),
				TreatedAirTightnessWorks = parser.ParseEnum<PartlyStateTreatment>(source.TreatedAirTightnessWorks, FieldParser.CsvName(x => x.TreatedAirTightnessWorks)),
				TreatedThermalBridgesWorks = parser.ParseEnum<PartlyStateTreatment>(source.TreatedThermalBridgesWorks, FieldParser.CsvName(x => x.TreatedThermalBridgesWorks)),
				MarkerNature = parser.ParseEnum<MarkerNature>(source.MarkerNature, FieldParser.CsvName(x => x.MarkerNature)),
				ConstructionYear = parser.GetHousingYearConstructionFromYear(source.ConstructionYear),
				AccompanyingTimeDurationForIdentificationMilestone = parser.ParseEnum<AccompanyingTimeDuration>(source.AccompanyingTimeDurationForIdentificationMilestone, FieldParser.CsvName(x => x.AccompanyingTimeDurationForIdentificationMilestone)),
				AccompanyingTimeDurationForOrganizeAndFinanceMilestone = !string.IsNullOrEmpty(source.AccompanyingTimeDurationForOrganizeAndFinanceMilestone) ? parser.ParseEnum<AccompanyingTimeDuration>(source.AccompanyingTimeDurationForOrganizeAndFinanceMilestone, FieldParser.CsvName(x => x.AccompanyingTimeDurationForOrganizeAndFinanceMilestone)) : null,
				AccompanyingTimeDurationForRealizeAndFollowMilestone = !string.IsNullOrEmpty(source.AccompanyingTimeDurationForRealizeAndFollowMilestone) ? parser.ParseEnum<AccompanyingTimeDuration>(source.AccompanyingTimeDurationForRealizeAndFollowMilestone, FieldParser.CsvName(x => x.AccompanyingTimeDurationForRealizeAndFollowMilestone)) : null,

				//Map string properties
				ExternalReference = ReturnStringValueOrNull(source.Reference) ,
				AccompanyingFileTerritory = ReturnStringValueOrNull(source.AccompanyingFileTerritory),
				SocialContext = ReturnStringValueOrNull(source.SocialContext),
				HouseholdProject = ReturnStringValueOrNull(source.HouseholdProject),
				CommentsOnHouseholdDifficulties = ReturnStringValueOrNull(source.CommentsOnHouseholdDifficulties),
				HouseholdDifficulties = ReturnStringValueOrNull(source.HouseholdDifficulties),
				HouseholdExpenses = ReturnStringValueOrNull(source.HouseholdExpenses),
				HouseholdResources = ReturnStringValueOrNull(source.HouseholdResources),
				PhoneNumber = ReturnStringValueOrNull(source.PhoneNumber),
				Email = ReturnStringValueOrNull(source.Email),
				Profession = ReturnStringValueOrNull(source.Job),
				CommentOnSocialProtectionFund = ReturnStringValueOrNull(source.CommentOnSocialProtectionFund),
				CommentOnPensionFund = ReturnStringValueOrNull(source.CommentOnPensionFund),
				CommentOnAdditionnalFund = ReturnStringValueOrNull(source.CommentOnAdditionnalFund),
				FirstName = ReturnStringValueOrNull(source.FirstName),
				LastName = ReturnStringValueOrNull(source.LastName),
				ArchitecturalNorms = ReturnStringValueOrNull(source.ArchitecturalOrTownPlanningStandards),
				CadastralReference = ReturnStringValueOrNull(source.CadastralReference),
				CommentOnPreviousWork = ReturnStringValueOrNull(source.CommentOnPreviousWork),
				Label = ReturnStringValueOrNull(source.Label),
				PostalCode = ReturnStringValueOrNull(source.PostalCode),
				City = ReturnStringValueOrNull(source.City),
				Department = ReturnStringValueOrNull(source.Department),
				Region = ReturnStringValueOrNull(source.Region),
				AdditionnalComment = ReturnStringValueOrNull(source.AdditionnalComment),
				DisordersObservedCommentary = ReturnStringValueOrNull(source.DisordersObservedCommentary),
				HeatingEnergy = ReturnStringValueOrNull(source.HeatingEnergy),
				NextStepAndVigilancePoint = ReturnStringValueOrNull(source.NextStepAndVigilancePoint),
				WorkDetails = ReturnStringValueOrNull(source.WorksDetails),
				HouseholdAvailabilitiyToOrganizeARASite = ReturnStringValueOrNull(source.HouseholdAvailabilitiyToOrganizeARASite),
				OtherQualification = ReturnStringValueOrNull(source.OtherQualification),
				ProjectType = ReturnStringValueOrNull(source.ProjectType),
				InsuranceType = ReturnStringValueOrNull(source.InsuranceType),
				SolicitedBankLoanType = ReturnStringValueOrNull(source.SolicitedBankLoanType),
				IntermediateAirtightnessTestResult = ReturnStringValueOrNull(source.IntermediateAirtightnessTestResult),
				JustificationAndActionsPutInPlaceIfNoTest = ReturnStringValueOrNull(source.JustificationAndActionsPutInPlaceIfNoTest),
				SolidarBuilder = ReturnStringValueOrNull(source.SolidarBuilder),
				CommentOnMarkerNature = ReturnStringValueOrNull(source.CommentOnMarkerNature),
				HouseholdAvailabilityForVisits = ReturnStringValueOrNull(source.HouseholdAvailabilityForVisits),

				//Map DateTime properties
				FirstEncounterDate = parser.ParseDate(source.FirstEncounterDate, FieldParser.CsvName(x => x.FirstEncounterDate)),
				StartOfAccompanyingDate = parser.ParseDate(source.StartOfAccompanyingDate, FieldParser.CsvName(x => x.StartOfAccompanyingDate)),
				OpeningDate = parser.ParseDate(source.OpeningDate, FieldParser.CsvName(x => x.OpeningDate)),
				CloseDate = parser.ParseDate(source.CloseDate, FieldParser.CsvName(x => x.CloseDate)),
				EndOfAccompanyingDate = parser.ParseDate(source.EndOfAccompanyingDate, FieldParser.CsvName(x => x.EndOfAccompanyingDate)),
				EndOfEncounterDate = parser.ParseDate(source.EndOfEncounterDate, FieldParser.CsvName(x => x.EndOfEncounterDate)),
				Birthdate = parser.ParseDate(source.Birthdate, FieldParser.CsvName(x => x.Birthdate)),

				//Map int and double properties
				NumberOfRoom = parser.ParseInt(source.NumberOfRoom, FieldParser.CsvName(x => x.NumberOfRoom)),
				NumberOfFloor = parser.ParseInt(source.NumberOfFloor, FieldParser.CsvName(x => x.NumberOfFloor)),
				YearOfAcquisitionOrEntry = parser.ParseInt(source.YearOfAcquisitionOrEntry, FieldParser.CsvName(x => x.YearOfAcquisitionOrEntry)),
				NumberOfDoor = parser.ParseInt(source.NumberOfDoor, FieldParser.CsvName(x => x.NumberOfDoor)),
				NumberOfWindow = parser.ParseInt(source.NumberOfWindow, FieldParser.CsvName(x => x.NumberOfWindow)),
				NumberOfPatioDoor = parser.ParseInt(source.NumberOfPatioDoor, FieldParser.CsvName(x => x.NumberOfPatioDoor)),
				NumberOfRoofDoor = parser.ParseInt(source.NumberOfRoofDoor, FieldParser.CsvName(x => x.NumberOfRoofDoor)),
				NumberOfBayWindow = parser.ParseInt(source.NumberOfBayWindow, FieldParser.CsvName(x => x.NumberOfBayWindow)),
				WellBeingRating = parser.ParseInt(source.WellBeingRating, FieldParser.CsvName(x => x.WellBeingRating)),
				EducationalFrameworkRating = parser.ParseInt(source.EducationalFrameworkRating, FieldParser.CsvName(x => x.EducationalFrameworkRating)),
				FamilySatisfaction = parser.ParseInt(source.FamilySatisfaction, FieldParser.CsvName(x => x.FamilySatisfaction)),
				LivingSpace = parser.ParseDouble(source.LivingSpace, FieldParser.CsvName(x => x.LivingSpace)),
				CeilingHeight = parser.ParseDouble(source.CeilingHeight, FieldParser.CsvName(x => x.CeilingHeight)),
				ReferenceIncomeTax = parser.ParseDouble(source.ReferenceIncomeTax, FieldParser.CsvName(x => x.ReferenceIncomeTax)),
				AnnualEnergyConsumption = parser.ParseDouble(source.AnnualEnergyConsumption, FieldParser.CsvName(x => x.AnnualEnergyConsumption)),
				AnnualGesEmission = parser.ParseDouble(source.AnnualGesEmission, FieldParser.CsvName(x => x.AnnualGesEmission)),
				EstimatedAnnualEnergyConsumptionAfterWork = parser.ParseDouble(source.EstimatedAnnualEnergyConsumptionAfterWork, FieldParser.CsvName(x => x.EstimatedAnnualEnergyConsumptionAfterWork)),
				EstimatedAnnualGesEmissionsAfterWork = parser.ParseDouble(source.EstimatedAnnualGesEmissionsAfterWork, FieldParser.CsvName(x => x.EstimatedAnnualGesEmissionsAfterWork)),
				MaPrimeRenovGuidedPath = parser.ParseDouble(source.MaPrimeRenovGuidedPath, FieldParser.CsvName(x => x.MaPrimeRenovGuidedPath)),
				MaPrimeRenovCoOwnerShip = parser.ParseDouble(source.MaPrimeRenovCoOwnerShip, FieldParser.CsvName(x => x.MaPrimeRenovCoOwnerShip)),
				MaPrimeLogementDecent = parser.ParseDouble(source.MaPrimeLogementDecent, FieldParser.CsvName(x => x.MaPrimeLogementDecent)),
				MaPrimeAdapt = parser.ParseDouble(source.MaPrimeAdapt, FieldParser.CsvName(x => x.MaPrimeAdapt)),
				BonusForExitingEnergeticSieve = parser.ParseDouble(source.BonusForExitingEnergeticSieve, FieldParser.CsvName(x => x.BonusForExitingEnergeticSieve)),
				RegionalAids = parser.ParseDouble(source.RegionalAids, FieldParser.CsvName(x => x.RegionalAids)),
				DepartmentalAids = parser.ParseDouble(source.DepartmentalAids, FieldParser.CsvName(x => x.DepartmentalAids)),
				PublicEstablishmentsForInterCommunalCooperationAids = parser.ParseDouble(source.PublicEstablishmentsForInterCommunalCooperationAids, FieldParser.CsvName(x => x.PublicEstablishmentsForInterCommunalCooperationAids)),
				MunicipalityAids = parser.ParseDouble(source.MunicipalityAids, FieldParser.CsvName(x => x.MunicipalityAids)),
				ClassicBankLoan = parser.ParseDouble(source.ClassicBankLoan, FieldParser.CsvName(x => x.ClassicBankLoan)),
				MdphFinancing = parser.ParseDouble(source.MdphFinancing, FieldParser.CsvName(x => x.MdphFinancing)),
				CeeFinancing = parser.ParseDouble(source.CeeFinancing, FieldParser.CsvName(x => x.CeeFinancing)),
				CafMsaFinancing = parser.ParseDouble(source.CafMsaFinancing, FieldParser.CsvName(x => x.CafMsaFinancing)),
				PensionFund = parser.ParseDouble(source.PensionFund, FieldParser.CsvName(x => x.PensionFund)),
				UnderprivilegedHousingFoundation = parser.ParseDouble(source.UnderprivilegedHousingFoundation, FieldParser.CsvName(x => x.UnderprivilegedHousingFoundation)),
				LeroyMerlinFoundation = parser.ParseDouble(source.LeroyMerlinFoundation, FieldParser.CsvName(x => x.LeroyMerlinFoundation)),
				WattForChangeFoundation = parser.ParseDouble(source.WattForChangeFoundation, FieldParser.CsvName(x => x.WattForChangeFoundation)),
				SocialProtectionGroup = parser.ParseDouble(source.SocialProtectionGroup, FieldParser.CsvName(x => x.SocialProtectionGroup)),
				HouseholdMaximumSavingAmountForRenovationProject = parser.ParseDouble(source.HouseholdMaximumSavingAmountForRenovationProject, FieldParser.CsvName(x => x.HouseholdMaximumSavingAmountForRenovationProject)),
				OtherFamilyMemberMaximumSupportAmountForRenovationProject = parser.ParseDouble(source.OtherFamilyMemberMaximumSupportAmountForRenovationProject, FieldParser.CsvName(x => x.OtherFamilyMemberMaximumSupportAmountForRenovationProject)),
				AccompanyingCost = parser.ParseDouble(source.AccompanyingCost, FieldParser.CsvName(x => x.AccompanyingCost)),
				HouseholdSelfFinancing = parser.ParseDouble(source.HouseholdSelfFinancing, FieldParser.CsvName(x => x.HouseholdSelfFinancing)),
				WorkTotalCost = parser.ParseDouble(source.WorkTotalCost, FieldParser.CsvName(x => x.WorkTotalCost)),
				NumberOfOccupants = parser.ParseInt(source.NumberOfOccupants, FieldParser.CsvName(x => x.NumberOfOccupants)),
				MonthlyEnergeticsExpenses = parser.ParseDouble(source.MonthlyEnergeticsExpenses, FieldParser.CsvName(x => x.MonthlyEnergeticsExpenses)),
				StopEnergyExclusionFunds = parser.ParseDouble(source.StopEnergyExclusionFunds, FieldParser.CsvName(x => x.StopEnergyExclusionFunds)),

				//Map bool properties
				ZeroEnergyExclusionTerritoriesProgram = parser.ParseBool(source.ZeroEnergyExclusionTerritoriesProgram, FieldParser.CsvName(x => x.ZeroEnergyExclusionTerritoriesProgram)),
				IsFollowedByAnSocialWorker = parser.ParseBool(source.IsFollowedByAnSocialWorker, FieldParser.CsvName(x => x.IsFollowedByAnSocialWorker)),
				HasAnOccupantWithDisabilities = parser.ParseBool(source.HasAnOccupantWithDisabilities, FieldParser.CsvName(x => x.HasAnOccupantWithDisabilities)),
				HasAnOccupantWithLongTermIllness = parser.ParseBool(source.HasAnOccupantWithLongTermIllness, FieldParser.CsvName(x => x.HasAnOccupantWithLongTermIllness)),
				HasAnOccupantWithIndependenceLoss = parser.ParseBool(source.HasAnOccupantWithIndependenceLoss, FieldParser.CsvName(x => x.HasAnOccupantWithIndependenceLoss)),
				HasAnOccupantUnderGuardianship = parser.ParseBool(source.HasAnOccupantUnderGuardianship, FieldParser.CsvName(x => x.HasAnOccupantUnderGuardianship)),
				HasAnOccupantUnderCuratorship = parser.ParseBool(source.HasAnOccupantUnderCuratorship, FieldParser.CsvName(x => x.HasAnOccupantUnderCuratorship)),
				HasOverdueInvoice = parser.ParseBool(source.HasOverdueInvoice, FieldParser.CsvName(x => x.HasOverdueInvoice)),
				IsInABFArea = parser.ParseBool(source.IsInABFArea, FieldParser.CsvName(x => x.IsInABFArea)),
				HasPreviousWork = parser.ParseBool(source.HasPreviousWork, FieldParser.CsvName(x => x.HasPreviousWork)),
				HasPestOrMold = parser.ParseBool(source.HasPestOrMold, FieldParser.CsvName(x => x.HasPestOrMold)),
				HasFaultyElectricalSystem = parser.ParseBool(source.HasFaultyElectricalSystem, FieldParser.CsvName(x => x.HasFaultyElectricalSystem)),
				HasVentilationSystem = parser.ParseBool(source.HasVentilationSystem, FieldParser.CsvName(x => x.HasVentilationSystem)),
				HasHeatingSystem = parser.ParseBool(source.HasHeatingSystem, FieldParser.CsvName(x => x.HasHeatingSystem)),
				HasHotWaterProduction = parser.ParseBool(source.HasHotWaterProduction, FieldParser.CsvName(x => x.HasHotWaterProduction)),
				RoofingState = parser.ParseBool(source.RoofingState, FieldParser.CsvName(x => x.RoofingState)),
				HasOpenings = parser.ParseBool(source.HasOpenings, FieldParser.CsvName(x => x.HasOpenings)),
				HasHousingCover = parser.ParseBool(source.HasHousingCover, FieldParser.CsvName(x => x.HasHousingCover)),
				HasInterestInPossibleARAProcess = parser.ParseBool(source.HasInterestInPossibleARAProcess, FieldParser.CsvName(x => x.HasInterestInPossibleARAProcess)),
				HasNeedForTemporaryReHousing = parser.ParseBool(source.HasNeedForTemporaryReHousing, FieldParser.CsvName(x => x.HasNeedForTemporaryReHousing)),
				HasEmergencyWorks = parser.ParseBool(source.HasEmergencyWorks, FieldParser.CsvName(x => x.HasEmergencyWorks)),
				HasEnergeticsRenovationWorks = parser.ParseBool(source.HasEnergeticsRenovationWorks, FieldParser.CsvName(x => x.HasEnergeticsRenovationWorks)),
				HasInducedWorks = parser.ParseBool(source.HasInducedWorks, FieldParser.CsvName(x => x.HasInducedWorks)),
				HasSafetyAndHealthWorks = parser.ParseBool(source.HasSafetyAndHealthWorks, FieldParser.CsvName(x => x.HasSafetyAndHealthWorks)),
				IsHouseholdReadyToStartARAProcess = parser.ParseBool(source.IsHouseholdReadyToStartARAProcess, FieldParser.CsvName(x => x.IsHouseholdReadyToStartARAProcess)),
				AreHouseholdPhysicalCapacitiesTakenIntoAccount = parser.ParseBool(source.AreHouseholdPhysicalCapacitiesTakenIntoAccount, FieldParser.CsvName(x => x.AreHouseholdPhysicalCapacitiesTakenIntoAccount)),
				DoHouseholdCanMobilizeSocialCircleOnConstructionSite = parser.ParseBool(source.DoHouseholdCanMobilizeSocialCircleOnConstructionSite, FieldParser.CsvName(x => x.DoHouseholdCanMobilizeSocialCircleOnConstructionSite)),
				IsRgeLabelUpToDate = parser.ParseBool(source.IsRgeLabelUpToDate, FieldParser.CsvName(x => x.IsRgeLabelUpToDate)),
				HasWorksEnabledHouseholdToStayAtHome = parser.ParseBool(source.HasWorksEnabledHouseholdToStayAtHome, FieldParser.CsvName(x => x.HasWorksEnabledHouseholdToStayAtHome)),
				ReturnToEmployment = parser.ParseBool(source.ReturnToEmployment, FieldParser.CsvName(x => x.ReturnToEmployment)),
				HasHousingAdaptationWorks = parser.ParseBool(source.HasHousingAdaptationWorks, FieldParser.CsvName(x => x.HasHousingAdaptationWorks)),
				HasFinishingWorks = parser.ParseBool(source.HasFinishingWorks, FieldParser.CsvName(x => x.HasFinishingWorks)),
				HasSafetyWorks = parser.ParseBool(source.HasSafetyWorks, FieldParser.CsvName(x => x.HasSafetyWorks)),
				HasPreparationWorks = parser.ParseBool(source.HasPreparationWorks, FieldParser.CsvName(x => x.HasPreparationWorks)),
				HasEmergencyWorksMonitoring = parser.ParseBool(source.HasEmergencyWorksMonitoring, FieldParser.CsvName(x => x.HasEmergencyWorksMonitoring)),
				HasUnsanitaryExit = parser.ParseBool(source.HasUnsanitaryExit, FieldParser.CsvName(x => x.HasUnsanitaryExit)),
				HasHumidityManagement = parser.ParseBool(source.HasHumidityManagement, FieldParser.CsvName(x => x.HasHumidityManagement)),
				HasEffectiveComplianceWithWorkRecommendations = parser.ParseBool(source.HasEffectiveComplianceWithWorkRecommendations, FieldParser.CsvName(x => x.HasEffectiveComplianceWithWorkRecommendations)),
				IsDeleted = parser.ParseBool(source.IsDeleted, FieldParser.CsvName(x => x.IsDeleted))
			};

			errors = parser.GetErrors();
			return result;
		}
}