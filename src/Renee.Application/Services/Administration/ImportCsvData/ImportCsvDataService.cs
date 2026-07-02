using CsvHelper;
using CsvHelper.Configuration;
using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.User;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using System.ComponentModel;
using System.Globalization;
using System.Text;

namespace Renee.Application.Services.Administration.ImportCsvData;

public class ImportCsvDataService(
	IMediator mediator,
	IUserService userService) : IImportCsvDataService
{
	public async Task<CsvMappingResult> MapCsvData(List<ParsedCsvData> dataList)
	{
		List<ImportAccompanyingFileFromCsvCommandInput> mappedLinesToCreate = [];
		List<UpdateAccompanyingFileFromCsvFileInput> mappedLinesToUpdate = [];

		var listDataForCsvImportResult = await mediator.Send(new LoadListDataForCsvImportQuery());
		var externalReferences = dataList.Select(d => d.ExternalReference).ToList();
		var existingAccompanyingFilesResult = await mediator.Send(new GetDuplicatedAccompanyingFileByExternalReferenceQuery(externalReferences));

		if (!listDataForCsvImportResult.IsSuccess || !existingAccompanyingFilesResult.IsSuccess)
			return new CsvMappingResult(null, new List<ImportAccompanyingFileFromCsvCommandInput>(), new List<UpdateAccompanyingFileFromCsvFileInput>());

		var listDataForCsvImport = listDataForCsvImportResult.Value!;
		var existingAccompanyingFiles = existingAccompanyingFilesResult.Value!;
		var existingExternalReferences = existingAccompanyingFiles.Select(ef => ef.ExternalReference).ToList();
		string? operatorName = null;

		foreach (var dataLine in dataList)
		{
			var solidarBuilder = await userService.GetUserByEmail(dataLine.SolidarBuilder, dataLine.ExternalReference);

			operatorName = GetOperatorNameOrDefault(operatorName, solidarBuilder);

			var territoryId = listDataForCsvImport.Territories.FirstOrDefault(
				territory => territory.TerritoryName == dataLine.AccompanyingFileTerritory)?.TerritoryId;

			var projectTypesSplitted = dataLine.ProjectType?.Split(CsvDataLabel.ListElementsSeparator);
			var insuranceTypeSplitted = dataLine.InsuranceType?.Split(CsvDataLabel.ListElementsSeparator);
			var householdDifficultiesSplitted = dataLine.HouseholdDifficulties?.Split(CsvDataLabel.ListElementsSeparator);
			var householdResourcesSplitted = dataLine.HouseholdResources?.Split(CsvDataLabel.ListElementsSeparator);
			var expensesSplitted = dataLine.HouseholdExpenses?.Split(CsvDataLabel.ListElementsSeparator);
			var heatingEnergiesSplitted = dataLine.HeatingEnergy?.Split(CsvDataLabel.ListElementsSeparator);

			var projectTypeIds = AddMatchingIds(
				projectTypesSplitted,
				listDataForCsvImport.ProjectTypes,
				pt => pt.Label,
				pt => pt.Id);

			var insuranceTypeIds = AddMatchingIds(
				insuranceTypeSplitted,
				listDataForCsvImport.InsuranceTypes,
				pt => pt.Label,
				pt => pt.Id);

			var householdDifficultiesIds = AddMatchingIds(
				householdDifficultiesSplitted,
				listDataForCsvImport.DifficultiesFacedFamily,
				pt => pt.Name,
				pt => pt.Id);

			var householdResourcesDictionary = HandleHouseholdResources(householdResourcesSplitted, listDataForCsvImport);
			var expensesDictionary = HandleExpenses(expensesSplitted);
			var heatingEnergiesDictionary = HandleHeatingEnergies(heatingEnergiesSplitted, listDataForCsvImport);
			var expensesDictionaryToUpdate = new Dictionary<int, double>(expensesDictionary);

			AddOrUpdateExpense(expensesDictionary, ExpenseType.MonthlyEnergecticsExpenses, dataLine.MonthlyEnergeticsExpenses);

			if (!existingExternalReferences.Contains(dataLine.ExternalReference))
			{
				var importSupportTeam = CreateSupportTeamRecord(
				solidarBuilder?.Value != null ? solidarBuilder.Value.Id : Guid.Empty,
				dataLine);
				var importAddress = CreateAddressRecord(dataLine);
				var importInitialState = CreateHousingInitialStateRecord(dataLine);
				var importAfterWorkState = CreateHousingAfterWorkStateRecord(dataLine);
				var importHousing = CreateHousingRecord(
					dataLine,
					importAddress,
					importInitialState,
					importAfterWorkState);
				var mainOccupantToCreate = CreateOccupantRecord(dataLine);
				var importHousehold = CreateHouseholdRecord(
					dataLine,
					householdDifficultiesIds,
					householdResourcesDictionary,
					expensesDictionary,
					heatingEnergiesDictionary,
					mainOccupantToCreate);
				var importPreWorkPlan = CreatePreWorkPlanRecord(
					dataLine,
					insuranceTypeIds,
					projectTypeIds);
				var importPreFinancingPlan = CreatePreFinancingPlanRecord(dataLine);
				var importWorkMonitoring = CreateWorkMonitoringRecord(dataLine);

				var mappedLineToCreate = new ImportAccompanyingFileFromCsvCommandInput(
					dataLine.LineNumber,
					AccompanyingFileHelper.CreateReference(
						solidarBuilder?.Value != null ?
							$"{solidarBuilder?.Value?.FirstName?.Trim()} {solidarBuilder?.Value?.LastName?.Trim().ToUpper()}" : string.Empty,
						mainOccupantToCreate.Trigram,
						importAddress.PostalCode,
						dataLine.OpeningDate),
					dataLine.ExternalReference,
					dataLine.FirstEncounterDate,
					dataLine.StartOfAccompanyingDate,
					dataLine.OpeningDate,
					dataLine.CloseDate,
					dataLine.EndOfEncounterDate,
					dataLine.EndOfAccompanyingDate,
					dataLine.AccompanyingTimeDurationForIdentificationMilestone,
					dataLine.AccompanyingTimeDurationForOrganizeAndFinanceMilestone,
					dataLine.AccompanyingTimeDurationForRealizeAndFollowMilestone,
					dataLine.ZeroEnergyExclusionTerritoriesProgram,
					dataLine.AccompanyingType,
					territoryId,
					dataLine.IsDeleted,
					importSupportTeam,
					importHousehold,
					importHousing,
					importPreWorkPlan,
					importPreFinancingPlan,
					importWorkMonitoring);

				mappedLinesToCreate.Add(mappedLineToCreate);
			}
			else
			{
				var existingFile = existingAccompanyingFiles.FirstOrDefault(f => f.ExternalReference == dataLine.ExternalReference);
				if (existingFile == null)
					continue;
				var updatedCommandInput = new UpdateAccompanyingFileFromCsvFileInput(
					existingFile.Id,
					dataLine,
					projectTypeIds,
					insuranceTypeIds,
					householdDifficultiesIds?.Select(id => (Guid?)id).ToList(),
					expensesDictionaryToUpdate.Select(e => new UpdatedHouseholdExpenses(null, (ExpenseType)e.Key, e.Value)).ToList(),
					householdResourcesDictionary.Select(r => new UpdatedHouseholdResource(r.Key, r.Value)).ToList(),
					heatingEnergiesDictionary.Select(h => new UpdatedHouseholdHeatingEnergy(h.Key, h.Value)).ToList());

				mappedLinesToUpdate.Add(updatedCommandInput);
			}
		}

		return new CsvMappingResult(
			operatorName,
			mappedLinesToCreate,
			mappedLinesToUpdate);
	}

	public List<ParsedCsvData> ProcessCsvFile(Stream fileStream, out List<LineErrorReport> lineErrorsReporting)
	{
		lineErrorsReporting = [];
		CsvConfiguration config = new(CultureInfo.InvariantCulture)
		{
			Delimiter = ";",
			Encoding = new UTF8Encoding(false),
			HeaderValidated = null, // Ignore headers not found in mapping
			MissingFieldFound = null // Ignore fields not found in file
		};

		using var reader = new StreamReader(fileStream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
		using var csv = new CsvReader(reader, config);

		csv.Read();
		csv.ReadHeader();

		var expectedColumnCount = csv.HeaderRecord?.Length;
		var validatedLines = new List<ParsedCsvData>();

		csv.Context.RegisterClassMap<MappedCsvData>();
		while (csv.Read())
		{
			var recordFields = csv.Parser.Record;
			var actualColumnCount = csv.Parser.Count;
			var line = csv.Parser.RawRow;
			var lineErrors = LineErrorReport.Create(line, []);

			if (recordFields == null || recordFields.All(f => string.IsNullOrWhiteSpace(f)))
				continue;

			if (expectedColumnCount != actualColumnCount)
			{
				CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, string.Format(CsvDataLabel.Errors.IncorrectNumberOfFieldsOnLine, actualColumnCount, expectedColumnCount));
				continue;
			}

			try
			{
				var record = csv.GetRecord<CsvData>();

				var missingRequiredFieldsErrors = CsvHelperExtensions.ValidateRequiredFields(record);
				if (missingRequiredFieldsErrors.Count > 0)
				{
					CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, missingRequiredFieldsErrors);
					continue;
				}

				var parsedLine = CsvDataParser.Parse(record, out List<string> parsingErrors, line);
				if (parsingErrors.Count == 0)
				{
					validatedLines.Add(parsedLine);
				}
				else
				{
					CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, parsingErrors);
				}
			}
			catch (Exception)
			{
				CsvHelperExtensions.ReportErrors(lineErrorsReporting, lineErrors, CsvDataLabel.Errors.UnknownError);
			}
		}

		if (validatedLines.Count == 0)
			lineErrorsReporting.Add(LineErrorReport.Create(null, CsvDataLabel.Errors.NoDataToImport));

		return validatedLines;
	}


	private static string? GetOperatorNameOrDefault(string? operatorName, ReneeOperationResult<UserDto?> solidarBuilder)
	{
		if (string.IsNullOrEmpty(operatorName) && solidarBuilder != null)
		{
			return solidarBuilder.Value?.ReportingStructureDto?.Name;
		}

		return operatorName;
	}

	private static ImportSupportTeamFromCsv CreateSupportTeamRecord(
		Guid solidarBuilderId,
		ParsedCsvData dataLine) =>
			new(
				solidarBuilderId,
				(MarkerNature)dataLine.MarkerNature!,
				dataLine.CommentOnMarkerNature);

	private static ImportAddressFromCsv CreateAddressRecord(ParsedCsvData dataLine) =>
		new(
			dataLine.Label!,
			dataLine.PostalCode!,
			dataLine.City!,
			dataLine.Department!,
			dataLine.Region!,
			dataLine.AdditionnalComment ?? string.Empty);

	private static ImportHousingInitialStateFromCsv CreateHousingInitialStateRecord(ParsedCsvData dataLine) =>
		new(
			dataLine.DegradationIndex,
			dataLine.UnsanitaryCoefficient,
			dataLine.EnergyDepravation,
			dataLine.SummerThermalComfortLevel,
			dataLine.WinterThermalComfortLevel,
			dataLine.NoiseComfortLevel,
			dataLine.HasPestOrMold,
			dataLine.HasFaultyElectricalSystem,
			dataLine.HasVentilationSystem,
			dataLine.HasHeatingSystem,
			dataLine.HasHotWaterProduction,
			dataLine.RoofingState,
			dataLine.HasOpenings,
			dataLine.HasHousingCover,
			dataLine.DisordersObservedCommentary,
			dataLine.Dpe,
			dataLine.Ges,
			dataLine.AnnualEnergyConsumption,
			dataLine.AnnualGesEmission,
			dataLine.HeatingEnergy);

	private static ImportHousingAfterWorkStateFromCsv CreateHousingAfterWorkStateRecord(ParsedCsvData dataLine) =>
		new(
			dataLine.EstimatedDpeAfterWork,
			dataLine.EstimatedAnnualEnergyConsumptionAfterWork,
			dataLine.EstimatedAnnualGesEmissionsAfterWork,
			dataLine.EstimatedGesAfterWork,
			AccompanyingFileHelper.CalculateEnergeticClassJump(dataLine.Dpe, dataLine.EstimatedDpeAfterWork));

	private static ImportOccupantFromCsv CreateOccupantRecord(ParsedCsvData dataLine) =>
		new(
			AccompanyingFileHelper.GenerateTrigram(dataLine.FirstName, dataLine.LastName),
			dataLine.Birthdate,
			EnumHelper.GetEnumValueFromDescription<SocioProfessionalCategory>(
				dataLine.SocioProfessionalCategory.GetDescription()),
			dataLine.PhoneNumber,
			dataLine.Email,
			dataLine.Profession,
			dataLine.SocialProtectionFund,
			dataLine.CommentOnSocialProtectionFund,
			dataLine.PensionFundOccupant,
			dataLine.CommentOnPensionFund,
			dataLine.AdditionnalFund,
			dataLine.CommentOnAdditionnalFund,
			dataLine.LastName,
			dataLine.FirstName);

	private static ImportHouseholdFromCsv CreateHouseholdRecord(
		ParsedCsvData dataLine,
		List<Guid> householdDifficultiesIds,
		Dictionary<Guid, double> householdResourcesDictionary,
		Dictionary<int, double> expensesDictionary,
		Dictionary<Guid, double> heatingEnergiesDictionary,
		ImportOccupantFromCsv occupantToCreate) =>
		new(
			dataLine.ReferenceIncomeTax,
			dataLine.HouseholdTypology,
			dataLine.SocialContext,
			dataLine.HasOverdueInvoice,
			dataLine.HouseholdProject,
			householdDifficultiesIds,
			householdResourcesDictionary,
			expensesDictionary,
			heatingEnergiesDictionary,
			dataLine.IsFollowedByAnSocialWorker,
			dataLine.HasAnOccupantWithDisabilities,
			dataLine.HasAnOccupantWithLongTermIllness,
			dataLine.HasAnOccupantWithIndependenceLoss,
			dataLine.HasAnOccupantUnderCuratorship,
			dataLine.HasAnOccupantUnderGuardianship,
			dataLine.CommentsOnHouseholdDifficulties,
			dataLine.HouseholdAvailabilityForVisits,
			dataLine.NumberOfOccupants,
			occupantToCreate.CreateMainOccupant());

	private static ImportHousingFromCsv CreateHousingRecord(
		ParsedCsvData dataLine,
		ImportAddressFromCsv importAddressForCsvImport,
		ImportHousingInitialStateFromCsv importHousingInitialStateForCsvImport,
		ImportHousingAfterWorkStateFromCsv importHousingAfterWorkStateV3ForCsvImport) =>
		new(
			dataLine.ConstructionYear,
			dataLine.LivingSpace,
			dataLine.HousingType,
			dataLine.GeographicAreaTypology,
			dataLine.OwnershipStatus,
			dataLine.IsInABFArea,
			dataLine.CadastralReference,
			dataLine.ArchitecturalNorms,
			dataLine.YearOfAcquisitionOrEntry,
			dataLine.HasPreviousWork,
			dataLine.CommentOnPreviousWork,
			dataLine.SunExposure,
			dataLine.CeilingHeight,
			dataLine.NumberOfBayWindow,
			dataLine.NumberOfDoor,
			dataLine.NumberOfPatioDoor,
			dataLine.NumberOfRoom,
			dataLine.NumberOfRoofDoor,
			dataLine.NumberOfWindow,
			dataLine.NumberOfFloor,
			importAddressForCsvImport.CreateAddress(),
			importHousingInitialStateForCsvImport.CreateHousingInitialState(),
			importHousingAfterWorkStateV3ForCsvImport.CreateHousingAfterWorkState());

	private static ImportPreWorkPlanFromCsv CreatePreWorkPlanRecord(
		ParsedCsvData dataLine,
		List<Guid> insuranceTypes,
		List<Guid> projectTypes) =>
		new(
			insuranceTypes,
			dataLine.NextStepAndVigilancePoint,
			dataLine.RenovationType,
			dataLine.HasInterestInPossibleARAProcess,
			dataLine.HasNeedForTemporaryReHousing,
			dataLine.HasEmergencyWorks,
			dataLine.HasEnergeticsRenovationWorks,
			dataLine.HasInducedWorks,
			dataLine.HasSafetyAndHealthWorks,
			dataLine.TreatedAirTightness,
			dataLine.TreatedThermalBridge,
			dataLine.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			dataLine.IsHouseholdReadyToStartARAProcess,
			dataLine.AreHouseholdPhysicalCapacitiesTakenIntoAccount,
			dataLine.DoHouseholdCanMobilizeSocialCircleOnConstructionSite,
			dataLine.WorkDetails,
			dataLine.HouseholdAvailabilitiyToOrganizeARASite,
			dataLine.IsRgeLabelUpToDate,
			dataLine.OtherQualification,
			projectTypes);

	private static ImportPreFinancingPlanFromCsv CreatePreFinancingPlanRecord(
		ParsedCsvData dataLine) =>
		new(
			dataLine.MaPrimeRenovGuidedPath,
			dataLine.MaPrimeRenovCoOwnerShip,
			dataLine.MaPrimeLogementDecent,
			dataLine.MaPrimeAdapt,
			dataLine.BonusForExitingEnergeticSieve,
			dataLine.RegionalAids,
			dataLine.DepartmentalAids,
			dataLine.PublicEstablishmentsForInterCommunalCooperationAids,
			dataLine.MunicipalityAids,
			dataLine.SolicitedBankLoanType,
			dataLine.ClassicBankLoan,
			dataLine.MdphFinancing,
			dataLine.CeeFinancing,
			dataLine.CafMsaFinancing,
			dataLine.PensionFund,
			dataLine.UnderprivilegedHousingFoundation,
			dataLine.LeroyMerlinFoundation,
			dataLine.WattForChangeFoundation,
			dataLine.SocialProtectionGroup,
			dataLine.StopEnergyExclusionFunds,
			dataLine.HouseholdMaximumSavingAmountForRenovationProject,
			dataLine.OtherFamilyMemberMaximumSupportAmountForRenovationProject,
			[]);

	private static ImportWorkMonitoringFromCsv CreateWorkMonitoringRecord(ParsedCsvData dataLine) =>
		new(
			dataLine.AccompanyingCost,
			dataLine.HouseholdSelfFinancing,
			dataLine.IntermediateAirtightnessTestResult,
			dataLine.JustificationAndActionsPutInPlaceIfNoTest,
			dataLine.HasEffectiveComplianceWithWorkRecommendations,
			dataLine.HasWorksEnabledHouseholdToStayAtHome,
			dataLine.WellBeingRating,
			dataLine.EducationalFrameworkRating,
			dataLine.FamilySatisfaction,
			dataLine.ReturnToEmployment,
			dataLine.HasHousingAdaptationWorks,
			dataLine.HasFinishingWorks,
			dataLine.HasSafetyWorks,
			dataLine.HasPreparationWorks,
			dataLine.HasEmergencyWorks,
			dataLine.HasUnsanitaryExit,
			dataLine.TreatedAirTightnessWorks,
			dataLine.TreatedThermalBridgesWorks,
			dataLine.HasHumidityManagement,
			dataLine.WorkTotalCost);

	private static List<Guid> AddMatchingIds<T>(
		IEnumerable<string>? inputList,
		IEnumerable<T> referenceList,
		Func<T, string?> labelSelector,
		Func<T, Guid> idSelector) where T : class
	{
		if (inputList == null || inputList.All(string.IsNullOrWhiteSpace)) return [];

		var result = new List<Guid>();

		foreach (var input in inputList)
		{
			var match = referenceList.FirstOrDefault(item => labelSelector(item) == input);
			if (match != null)
			{
				result.Add(idSelector(match));
			}
		}

		return result;
	}


	private static int? GetExpenseTypeIdFromDescription(string label)
	{
		foreach (var field in typeof(ExpenseType).GetFields())
		{
			if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute &&
			attribute.Description.Contains(label, StringComparison.OrdinalIgnoreCase))
			{
				var value = field.GetValue(null);
				if (value != null)
					return (int)value;
			}
		}
		return null;
	}

	private static Dictionary<Guid, double> HandleHouseholdResources(
		string[]? householdResourcesSplitted,
		LoadListDataForCsvImportQueryObjectResult listDataForCsvImport)
	{
		var householdResourcesDictionary = new Dictionary<Guid, double>();

		if (householdResourcesSplitted != null && !householdResourcesSplitted.All(hr => string.IsNullOrWhiteSpace(hr)))
			foreach (var householdResource in householdResourcesSplitted)
			{
				var typologie = householdResource.Split(CsvDataLabel.LabelValueSeparator);
				var matchedTypologie = listDataForCsvImport.HouseholdResourcesTypology.FirstOrDefault(hr => hr.Name != null && hr.Name.Contains(typologie[0]));

				if (matchedTypologie != null && typologie.Length == 2 && double.TryParse(typologie[1], out var parsedValue))
					householdResourcesDictionary[matchedTypologie.Id] = parsedValue;
			}

		return householdResourcesDictionary;
	}

	private static Dictionary<int, double> HandleExpenses(string[]? expensesSplitted)
	{
		var expensesDictionary = new Dictionary<int, double>();

		if (expensesSplitted is null || expensesSplitted.All(string.IsNullOrWhiteSpace))
			return expensesDictionary;

		foreach (var householdExpense in expensesSplitted)
		{
			var expense = householdExpense.Split(CsvDataLabel.LabelValueSeparator);

			if (expense.Length != 2 || !double.TryParse(expense[1], out var parsedValue)) continue;

			var expenseType = GetExpenseTypeIdFromDescription(expense[0]);

			if (expenseType != null)
				expensesDictionary[(int)expenseType] = parsedValue;
		}

		return expensesDictionary;
	}

	private static void AddOrUpdateExpense(
		Dictionary<int, double> expensesDictionary,
		ExpenseType type,
		double? value)
	{
		if (value is double val)
		{
			expensesDictionary[(int)type] = val;
		}
	}

	private static Dictionary<Guid, double> HandleHeatingEnergies(
		string[]? heatingEnergiesSplitted,
		LoadListDataForCsvImportQueryObjectResult listDataForCsvImport)
	{
		var heatingEnergiesDictionary = new Dictionary<Guid, double>();

		if (heatingEnergiesSplitted is null || heatingEnergiesSplitted.All(string.IsNullOrWhiteSpace))
			return heatingEnergiesDictionary;

		foreach (var energy in heatingEnergiesSplitted)
		{
			var heatingEnergy = energy.Split(CsvDataLabel.LabelValueSeparator);
			var matchedHeatingEnergy = listDataForCsvImport.HouseholdHeatingEnergyLabel.FirstOrDefault(he => he.Name.Contains(heatingEnergy[0]));

			if (heatingEnergy.Length != 2 || !double.TryParse(heatingEnergy[1], out var parsedValue) || matchedHeatingEnergy == null) continue;

			heatingEnergiesDictionary[matchedHeatingEnergy.Id] = parsedValue;
		}

		return heatingEnergiesDictionary;
	}
}