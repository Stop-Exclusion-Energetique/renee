using System.Globalization;
using System.Reflection;
using System.Text;
using ExcelDataReader;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.Application.Services.Administration.ImportExcelData;

public class ImportExcelDataService(IProjectTypeService projectTypeService) : IImportExcelDataService
{
	private readonly Dictionary<string, AccompanyingFileStage> _headerStageMapping = new()
	{
		{ 
			ExcelDataLabel.IdentificationMilestone.MilestoneValidation, 
			AccompanyingFileStage.Identify },
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.MilestoneValidation,
			AccompanyingFileStage.OrganizingAndFinancing
		},
		{
			ExcelDataLabel.RealizeAndFollowMilestone.MilestoneValidation,
			AccompanyingFileStage.RealisationAndFollowing
		}
	};

	private readonly Dictionary<string, string> _identifyMilestoneHeaderMapping = new()
	{
		{
			ExcelDataLabel.IdentificationMilestone.CommentOnMarkerNature,
			nameof(IdentificationMappedData.CommentOnMarkerNature)
		},
		{
			ExcelDataLabel.IdentificationMilestone.StreetNumberName,
			nameof(IdentificationMappedData.StreetNumberName)
		},
		{
			ExcelDataLabel.IdentificationMilestone.AdditionalAddress,
			nameof(IdentificationMappedData.AdditionnalAddress)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.PostalCode, 
			nameof(IdentificationMappedData.PostalCode) 
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.Municipality, 
			nameof(IdentificationMappedData.Municipality) 
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.Department,
			nameof(IdentificationMappedData.Department)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.Region,
			nameof(IdentificationMappedData.Region) 
		},
		{
			ExcelDataLabel.IdentificationMilestone.IncomeTaxReference,
			nameof(IdentificationMappedData.IncomeTaxReference)
		},
		{
			ExcelDataLabel.IdentificationMilestone.EnergyDeprivation,
			nameof(IdentificationMappedData.EnergyDeprivation)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.HousingType,
			nameof(IdentificationMappedData.HousingType) 
		},
		{
			ExcelDataLabel.IdentificationMilestone.ConstructionYear,
			nameof(IdentificationMappedData.ConstructionYear)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.LivingSpace, 
			nameof(IdentificationMappedData.LivingSpace) 
		},
		{
			ExcelDataLabel.IdentificationMilestone.AnnualEnergyConsumptionBeforeWork,
			nameof(IdentificationMappedData.AnnualEnergyConsumptionBeforeWork)
		},
		{
			ExcelDataLabel.IdentificationMilestone.HasOverdueInvoice,
			nameof(IdentificationMappedData.HasOverdueInvoice)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.SocialContext,
			nameof(IdentificationMappedData.SocialContext) 
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.StartingDpe,
			nameof(IdentificationMappedData.StartingDpe) 
		},
		{
			ExcelDataLabel.IdentificationMilestone.GeographicalHousingAreaTypology,
			nameof(IdentificationMappedData.GeographicalHousingAreaTypology)
		},
		{
			ExcelDataLabel.IdentificationMilestone.HouseholdTypology,
			nameof(IdentificationMappedData.HouseholdTypology)
		},
		{ 
			ExcelDataLabel.IdentificationMilestone.MarkerNature,
			nameof(IdentificationMappedData.MarkerNature) 
		},
		{
			ExcelDataLabel.IdentificationMilestone.OwnershipStatus,
			nameof(IdentificationMappedData.OwnershipStatus)
		}
	};

	private readonly Dictionary<string, string> _organizeAndFinanceMilestoneHeaderMapping = new()
	{
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.IsEmergencyWorks,
			nameof(OrganizeAndFinanceMappedData.IsEmergencyWorks)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.NextStepAndVigilancePoints,
			nameof(OrganizeAndFinanceMappedData.NextStepAndVigilancePoints)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.TreatedAirTightness,
			nameof(OrganizeAndFinanceMappedData.TreatedAirTightness)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.TreatedThermalBridge,
			nameof(OrganizeAndFinanceMappedData.TreatedThermalBridge)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			nameof(OrganizeAndFinanceMappedData.AreExistingHumidityAndVaporMigrationManagedAfterTreatment)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.EstimatedEnergyConsumptionAfterWork,
			nameof(OrganizeAndFinanceMappedData.EstimatedEnergyConsumptionAfterWork)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.EstimatedDpeLabelAfterWork,
			nameof(OrganizeAndFinanceMappedData.EstimatedDpeLabelAfterWork)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.EstimatedDpeJumpClass,
			nameof(OrganizeAndFinanceMappedData.EstimatedDpeJumpClass)
		},
		{ ExcelDataLabel.OrganizeAndFinanceMilestone.RegionAids, nameof(OrganizeAndFinanceMappedData.RegionAids) },
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.DepartmentAids,
			nameof(OrganizeAndFinanceMappedData.DepartmentAids)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.MunicipalityAids,
			nameof(OrganizeAndFinanceMappedData.MunicipalityAids)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.PrivateActors, nameof(OrganizeAndFinanceMappedData.PrivateActors)
		},
		{ ExcelDataLabel.OrganizeAndFinanceMilestone.PensionFunds, nameof(OrganizeAndFinanceMappedData.PensionFunds) },
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.HouseholdMaximumSavingAmountForRenovationProject,
			nameof(OrganizeAndFinanceMappedData.HouseholdMaximumSavingAmountForRenovationProject)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.MaximumAmountSupportFamilyMembersRenovationProject,
			nameof(OrganizeAndFinanceMappedData.MaximumAmountSupportFamilyMembersRenovationProject)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.EstimatedRemainingAmount,
			nameof(OrganizeAndFinanceMappedData.EstimatedRemainingAmount)
		},
		{
			ExcelDataLabel.OrganizeAndFinanceMilestone.RenovationType,
			nameof(OrganizeAndFinanceMappedData.RenovationType)
		}
	};

	public async Task<ImportAccompanyingFileFromFileInputCommandInput?> MapExcelData(
		List<ExcelData> data,
		RequiredFieldsFromDataImportPage requiredFieldsFromDataImportPage)
	{
		var identifyMappedData = MapIdentificationData(data);
		var organizeAndFinanceMappedData = await MapOrganizeAndFinanceData(data);
		var realizeAndFollowMappedData = MapRealizeAndFollowData(data); 

		if (requiredFieldsFromDataImportPage.ReferentSolidarBuilderId == null)
			return null;

		var importSupportTeam = new ImportSupportTeam(
			requiredFieldsFromDataImportPage.ReferentDiffuseCoordinatorId,
			requiredFieldsFromDataImportPage.ReferentTargetCoordinatorId,
			requiredFieldsFromDataImportPage.ReferentEtId,
			(Guid)requiredFieldsFromDataImportPage.ReferentSolidarBuilderId,
			identifyMappedData.MarkerNature,
			identifyMappedData.CommentOnMarkerNature);

		var importAddress = new ImportAddress(
			identifyMappedData.StreetNumberName,
			identifyMappedData.PostalCode,
			identifyMappedData.Municipality,
			identifyMappedData.Department,
			identifyMappedData.Region,
			identifyMappedData.AdditionnalAddress ?? string.Empty);

		var importInitialState = new ImportHousingInitialState(
			identifyMappedData.DegradationIndex,
			identifyMappedData.UnsanitaryCoefficient,
			identifyMappedData.AnnualEnergyConsumptionBeforeWork,
			identifyMappedData.StartingDpe,
			identifyMappedData.EnergyDeprivation);

		var importAfterWorkState = new ImportHousingAfterWorkState(
			organizeAndFinanceMappedData.EstimatedDpeLabelAfterWork,
			organizeAndFinanceMappedData.EstimatedEnergyConsumptionAfterWork,
			organizeAndFinanceMappedData.EstimatedDpeJumpClass);

		var mainOccupantToCreate = new OccupantToCreate(
			identifyMappedData.Occupants[0].Trigram,
			DateTime.ParseExact(identifyMappedData.Occupants[0].DateOfBirth, Labels.DateFormatUtc, CultureInfo.InvariantCulture)
				.Date,
			EnumHelper.GetEnumValueFromDescription<SocioProfessionalCategory>(
				identifyMappedData.Occupants[0].SocioprofessionalCategory),
			requiredFieldsFromDataImportPage.LastName,
			requiredFieldsFromDataImportPage.FirstName);

		var secondaryOccupantToCreate = identifyMappedData.Occupants.Where(so => so != identifyMappedData.Occupants[0]).Select(
			o => new OccupantToCreate(
				o.Trigram,
				DateTime.ParseExact(o.DateOfBirth, Labels.DateFormatUtc, CultureInfo.InvariantCulture).Date,
				null,
				null,
				null)).ToList();

		var importHousehold = new ImportHousehold(
			identifyMappedData.IncomeTaxReference,
			identifyMappedData.HouseholdTypology,
			identifyMappedData.SocialContext,
			identifyMappedData.HasOverdueInvoice,
			identifyMappedData.AnahCategory,
			mainOccupantToCreate.CreateMainOccupant(),
			secondaryOccupantToCreate.Select(so => so.CreateSecondaryOccupant()).ToList());

		var importHousing = new ImportHousing(
			identifyMappedData.ConstructionYear,
			identifyMappedData.LivingSpace,
			identifyMappedData.HousingType,
			identifyMappedData.GeographicalHousingAreaTypology,
			identifyMappedData.OwnershipStatus,
			importAddress.CreateAddress(),
			importInitialState.CreateHousingInitialState(),
			importAfterWorkState.CreateHousingAfterWorkState());

		var importPreWorkPlan = new ImportPreWorkPlan(
			organizeAndFinanceMappedData.ProjectTypes ?? [],
			organizeAndFinanceMappedData.RenovationType,
			organizeAndFinanceMappedData.NextStepAndVigilancePoints,
			organizeAndFinanceMappedData.IsEmergencyWorks,
			organizeAndFinanceMappedData.TreatedAirTightness,
			organizeAndFinanceMappedData.TreatedThermalBridge,
			organizeAndFinanceMappedData.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			[]);

		var importPreFinancingPlan = new ImportPreFinancingPlan(
			organizeAndFinanceMappedData.RegionAids,
			organizeAndFinanceMappedData.DepartmentAids,
			organizeAndFinanceMappedData.PublicEstablishmentsIntercommunalCooperation,
			organizeAndFinanceMappedData.MunicipalityAids,
			organizeAndFinanceMappedData.PrivateActors,
			organizeAndFinanceMappedData.PensionFunds,
			organizeAndFinanceMappedData.HouseholdMaximumSavingAmountForRenovationProject,
			organizeAndFinanceMappedData.MaximumAmountSupportFamilyMembersRenovationProject,
			organizeAndFinanceMappedData.EstimatedRemainingAmount);

		var importWorkMonitoring = new ImportWorkMonitoring(
			realizeAndFollowMappedData.HouseholdAutoFinancing,
			realizeAndFollowMappedData.IntermediateAirtightnessTestResult,
			realizeAndFollowMappedData.WaterproofingTreatmentActions,
			realizeAndFollowMappedData.HasEffectiveComplianceWithWorkRecommendations,
			realizeAndFollowMappedData.HasWorkEnablingHomeSupport,
			realizeAndFollowMappedData.WellBeingRating,
			realizeAndFollowMappedData.EducationalFrameworkRating,
			realizeAndFollowMappedData.FamilySatisfactionWithSupport,
			realizeAndFollowMappedData.IsBackToEmployment
			);

		var importInvoice = new ImportInvoice(
			realizeAndFollowMappedData.TotalCost,
			realizeAndFollowMappedData.BilledWorkForce);

		return new ImportAccompanyingFileFromFileInputCommandInput(
			identifyMappedData.Reference,
			identifyMappedData.FirstContactDate,
			identifyMappedData.StartSupportDate,
			realizeAndFollowMappedData.EndOfEncounterDate,
			realizeAndFollowMappedData.EndOfAccompanyingDate,
			identifyMappedData.MarkerNature,
			identifyMappedData.CommentOnMarkerNature,
			identifyMappedData.ContactWithFamilyForIdentificationMilestones,
			organizeAndFinanceMappedData.ContactWithFamilyForOrganizeAndFinanceMilestone,
			realizeAndFollowMappedData.ContactWithFamilyForRealizeAndFollowMilestone,
			requiredFieldsFromDataImportPage.ZeroEnergyExclusionTerritoriesProgram,
			requiredFieldsFromDataImportPage.AccompanyingType,
			requiredFieldsFromDataImportPage.TerritoryId,
			importSupportTeam,
			importHousehold,
			importHousing,
			importPreWorkPlan,
			importPreFinancingPlan,
			importWorkMonitoring,
			importInvoice);
	}

	public List<ExcelData> ProcessExcelFile(Stream fileStream)
	{
		var dataList = new List<ExcelData>();

		Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

		using var reader = ExcelReaderFactory.CreateReader(fileStream);
		var result = reader.AsDataSet(
			new ExcelDataSetConfiguration
			{
				ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
			});

		// Read the first excel sheet
		var dataTable = result.Tables[0];

		// Get stage in order to stop the file reading at the end of the current stage
		var stageEnum = GetStage(dataTable.Rows[5][3].ToString() ?? string.Empty);

		// Index begins at 2 to avoid useless file headers
		for (var i = 2; i < dataTable.Rows.Count; i++)
		{
			var header = dataTable.Rows[i][2]?.ToString(); // Column C
			var value = dataTable.Rows[i][3] is DateTime dateTimeValue // Column D
				? dateTimeValue.ToString(Labels.DateFormatUtc, CultureInfo.InvariantCulture)
				: dataTable.Rows[i][3].ToString();

			if (string.IsNullOrWhiteSpace(header)) continue;

			// This value corresponds to another header (EnergeticEfficiency)
			if (header == Labels.PreWorkPlanProjectType)
				header = ExcelDataLabel.OrganizeAndFinanceMilestone.EnergeticEfficiency;

			if (!string.IsNullOrWhiteSpace(value) && (header.Contains(Labels.DegradationIndex) ||
				header.Contains(Labels.UnsanitaryCoefficient)))
			{
				var extractedValueWithoutMiddleSpaces = String.Join("", value.Split(" "));
				value = extractedValueWithoutMiddleSpaces;
			}

			dataList.Add(new ExcelData(header.Trim(), value?.Trim() ?? null));

			// Stop file reading at the end of the current stage
			if (_headerStageMapping.TryGetValue(header.Replace('\n', ' '), out var mappedStage) &&
				stageEnum == mappedStage)
				break;
		}

		return dataList;
	}

	public ImportExcelDataCommandResult ValidateData(List<ExcelData> data, bool isV3)
	{
		var requiredFields = new List<ExcelData>
		{
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Reference) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.Reference, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Milestone) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.Milestone, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.StreetNumberName) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.StreetNumberName, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.PostalCode) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.PostalCode, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Municipality) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.Municipality, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Department) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.Department, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Region) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.Region, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.MarkerNature) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.MarkerNature, null),
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.FirstContactDate) ??
			new ExcelData(ExcelDataLabel.IdentificationMilestone.FirstContactDate, null)
		};

		string? mainOccupantTrigram, mainOccupantDateOfBirth, mainOccupantSocioProfessionalCategory;
		if (isV3)
		{
			// Get main occupant data
			mainOccupantTrigram =
				data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Trigram)?.Value;
			mainOccupantDateOfBirth =
				data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.MainOccupantDateOfBirth)?.Value;
			mainOccupantSocioProfessionalCategory = data.Find(
					x => x.Header.Trim() ==
					     ExcelDataLabel.IdentificationMilestone.MainOccupantSocialProfessionalCategory)
				?.Value;
		}
		else
		{
			// Get main occupant data
			mainOccupantTrigram =
				data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.MainOccupant)?.Value;
			mainOccupantDateOfBirth =
				data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.MainOccupantDateOfBirth)?.Value;
			mainOccupantSocioProfessionalCategory = data.Find(
					x => x.Header.Trim() ==
					     ExcelDataLabel.IdentificationMilestone.MainOccupantSocialProfessionalCategory)
				?.Value;
		}

		var requiredFieldsAreValidate = ValidateRequiredFields(
			requiredFields,
			mainOccupantTrigram,
			mainOccupantDateOfBirth,
			mainOccupantSocioProfessionalCategory);
		if (!requiredFieldsAreValidate.IsSuccess) return requiredFieldsAreValidate;

		return ImportExcelDataCommandResult.Success();
	}

	private static AccompanyingFileStage GetStage(string description)
	{
		return description switch
		{
			not null when description.Contains("Jalon 1") => AccompanyingFileStage.Identify,
			not null when description.Contains("Jalon 2") => AccompanyingFileStage.OrganizingAndFinancing,
			not null when description.Contains("Jalon 3") => AccompanyingFileStage.RealisationAndFollowing,
			_ => throw new ArgumentException(ExcelDataLabel.Errors.UnknownMilestone)
		};
	}

	private IdentificationMappedData MapIdentificationData(List<ExcelData> data)
	{
		var contactWithFamilyDuringIdentifyStage =
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.ContactWithFamily)?.Value!;
		var reference = data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Reference)?.Value!;
		var firstContactDate =
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.FirstContactDate)?.Value!;
		var startSupportDate =
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.StartSupportDate)?.Value!;
		var streetNumberName =
			data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.StreetNumberName)?.Value!;
		var postalCode = data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.PostalCode)?.Value!;
		var municipality = data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Municipality)?.Value!;
		var department = data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Department)?.Value!;
		var region = data.Find(x => x.Header == ExcelDataLabel.IdentificationMilestone.Region)?.Value!;

		var degradationIndex =
			data.Find(x => x.Header.Contains(ExcelDataLabel.IdentificationMilestone.DegradationIndex))?.Value;
		var unsanitaryCoefficient = 
			data.Find(x => x.Header.Contains(ExcelDataLabel.IdentificationMilestone.UnsanitaryCoefficient))?.Value;

		var mappedData = new IdentificationMappedData(
			reference,
			DateTime.ParseExact(
				firstContactDate,
				Labels.DateFormatUtc,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None),
			DateTime.ParseExact(
				startSupportDate,
				Labels.DateFormatUtc,
				CultureInfo.InvariantCulture,
				DateTimeStyles.None),
			streetNumberName,
			postalCode,
			municipality,
			department,
			region)
		{
			ContactWithFamilyForIdentificationMilestones =
				!string.IsNullOrEmpty(contactWithFamilyDuringIdentifyStage)
					? int.Parse(contactWithFamilyDuringIdentifyStage)
					: null,
			DegradationIndex = EnumHelper.GetEnumValueFromDescription<DegradationIndex>(degradationIndex),
			UnsanitaryCoefficient = EnumHelper.GetEnumValueFromDescription<UnsanitaryCoefficient>(unsanitaryCoefficient)
		};

		for (var i = 0; i < data.Count; i++)
			if (data[i].Header.Contains("Occupant"))
			{
				// When secondary occupant is null
				if (string.IsNullOrEmpty(data[i].Value)) continue;

				var trigram = data[i].Value!;
				var dateOfBirth = data[i + 2].Value!;
				var socioProfessionalCategory = data[i].Header.Equals(Labels.MainOccupant) ? data[i + 3].Value! : null;

				mappedData.Occupants.Add(new ImportOccupantExcelData(trigram, dateOfBirth, socioProfessionalCategory));
			}
			else if (_identifyMilestoneHeaderMapping.TryGetValue(data[i].Header, out var propertyName))
			{
				ParseValidatedData(data[i], propertyName, mappedData);
			}

		return mappedData;
	}

	private async Task<OrganizeAndFinanceMappedData> MapOrganizeAndFinanceData(List<ExcelData> data)
	{
		var contactWithFamilyDuringOrganizeAndFinanceStage = data
			.LastOrDefault(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.ContactWithFamily)?.Value;

		var projectTypesLabels = new List<ExcelData?>
		{
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.EnergeticEfficiency),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.FightAgainstSubstandardHousing),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.UnsanitaryExit),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.SecurityWork),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.PreparationWork),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.FinishingWork),
			data.Find(x => x.Header == ExcelDataLabel.OrganizeAndFinanceMilestone.HousingAdaptationWork)
		};
		var projectTypesResult = await projectTypeService.GetAllProjectTypes();
		var projectTypeIds = MatchProjectTypes(projectTypesLabels, projectTypesResult.IsSuccess ? projectTypesResult.Value : []);

		var publicEstablishmentsIntercommunalCooperation = data.Find(
			x => x.Header.Contains("Etablissements publics de coopération intercommunale"));

		var mappedData = new OrganizeAndFinanceMappedData
		{
			ProjectTypes = projectTypeIds,
			ContactWithFamilyForOrganizeAndFinanceMilestone =
				!string.IsNullOrEmpty(contactWithFamilyDuringOrganizeAndFinanceStage)
					? int.Parse(contactWithFamilyDuringOrganizeAndFinanceStage)
					: null,
			PublicEstablishmentsIntercommunalCooperation =
			!string.IsNullOrEmpty(publicEstablishmentsIntercommunalCooperation?.Value)
				? double.Parse(publicEstablishmentsIntercommunalCooperation.Value)
				: null
		};

		foreach (var field in data)
			if (_organizeAndFinanceMilestoneHeaderMapping.TryGetValue(field.Header, out var propertyName))
				ParseValidatedData(field, propertyName, mappedData);

		return mappedData;
	}

	private static RealizeAndFollowMappedData MapRealizeAndFollowData(List<ExcelData> data)
	{
		var mappedData = new RealizeAndFollowMappedData(
		ImportFileDataHelper.TryParseValue<double?>(data.Find(x => x.Header.Contains("Coût total des travaux"))?.Value, out var totalCost) ? totalCost : default,
		ImportFileDataHelper.TryParseValue<double?>(data.Find(x => x.Header.Contains(ExcelDataLabel.RealizeAndFollowMilestone.BilledWorkForce, StringComparison.CurrentCultureIgnoreCase))?.Value, out var billedWorkForce) ? billedWorkForce : default,
		ImportFileDataHelper.TryParseValue<double?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.HouseholdAutoFinancing)?.Value, out var householdAutoFinancing) ? householdAutoFinancing : default,
		ImportFileDataHelper.TryParseValue<string?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.IntermediateAirtightnessTestResult)?.Value, out var intermediateAirtightnessTestResult) ? intermediateAirtightnessTestResult : null,
		ImportFileDataHelper.TryParseValue<string?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.WaterproofingTreatmentActions)?.Value, out var waterproofingTreatmentActions) ? waterproofingTreatmentActions : null,
		ImportFileDataHelper.TryParseValue<bool?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.EffectiveComplianceWithWorkRecommendations)?.Value, out var effectiveComplianceWithWorkRecommendations) ? effectiveComplianceWithWorkRecommendations : null,
		ImportFileDataHelper.TryParseValue<bool?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.HasWorkEnablingHomeSupport)?.Value, out var hasWorkEnablingHomeSupport) ? hasWorkEnablingHomeSupport : default,
		ImportFileDataHelper.TryParseValue<int?>(data.Find(x => x.Header.Contains(ExcelDataLabel.RealizeAndFollowMilestone.WellBeingRating))?.Value, out var wellBeingRating) ? wellBeingRating : default,
		ImportFileDataHelper.TryParseValue<int?>(data.Find(x => x.Header.Contains(ExcelDataLabel.RealizeAndFollowMilestone.EducationalFrameworkRating))?.Value, out var educationalFrameworkRating) ? educationalFrameworkRating : default,
		ImportFileDataHelper.TryParseValue<int?>(data.Find(x => x.Header.Contains(ExcelDataLabel.RealizeAndFollowMilestone.FamilySatisfactionWithSupport))?.Value, out var familySatisfactionWithSupport) ? familySatisfactionWithSupport : default,
		ImportFileDataHelper.TryParseValue<bool?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.IsBackToEmployment)?.Value, out var isBackToEmployment) ? isBackToEmployment : default,
		ImportFileDataHelper.TryParseValue<DateTime?>(data.Find(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.EndOfAccompanyingDate)?.Value, out var endOfAccompanyingDate) ? endOfAccompanyingDate : default,
		ImportFileDataHelper.TryParseValue<DateTime?>(data.Find(x => x.Header.Contains(ExcelDataLabel.RealizeAndFollowMilestone.EndOfEncounterDate))?.Value, out var endOfEncounterDate) ? endOfEncounterDate : default,
		ImportFileDataHelper.TryParseValue<int?>(data.LastOrDefault(x => x.Header == ExcelDataLabel.RealizeAndFollowMilestone.ContactWithFamily)?.Value, out var contactWithFamilyDuringRealizeAndFollowStage) ? contactWithFamilyDuringRealizeAndFollowStage : default);

		return mappedData;
	}

	private static List<Guid>? MatchProjectTypes(
		List<ExcelData?>? projectTypesLabels,
		IEnumerable<ProjectTypeDto>? projectTypes)
	{
		if (projectTypesLabels is not null && projectTypes is not null)
			return projectTypes
				.Where(project => projectTypesLabels.Exists(p => p?.Value == Labels.Yes && p.Header == project.Label))
				.Select(project => project.Id).ToList();
		return [];
	}

	private static void ParseValidatedData(ExcelData field, string propertyName, IMappedData mappedData)
	{
		PropertyInfo? property;
		if (mappedData is IdentificationMappedData)
			property = typeof(IdentificationMappedData).GetProperty(propertyName);
		else if (mappedData is OrganizeAndFinanceMappedData)
			property = typeof(OrganizeAndFinanceMappedData).GetProperty(propertyName);
		else
			property = typeof(RealizeAndFollowMappedData).GetProperty(propertyName);

		if (property != null && property.CanWrite)
		{
			var propertyType = property.PropertyType;
			var tryParseMethod =
				typeof(ImportFileDataHelper).GetMethod("TryParseValue")!.MakeGenericMethod(propertyType);

			var parameters = new object[] { field.Value?.Trim() ?? string.Empty, null! };
			var parseSuccess = (bool)tryParseMethod.Invoke(null, parameters)!;
			if (parseSuccess)
				property.SetValue(mappedData, parameters[1]);
			else
				throw new ArgumentException(
					$"Erreur dans le format des données pour le champ : \"{field.Header}\" et la valeur : \"{field.Value}\"");
		}
	}

	private static void ValidatedMainOccupantFields(
		string? trigram,
		string? dateOfBirth,
		string? socioProfessionalCategory,
		List<string> errors)
	{
		if (string.IsNullOrEmpty(trigram)) errors.Add(ExcelDataLabel.Errors.RequiredMainOccupantTrigram);

		if (string.IsNullOrEmpty(dateOfBirth)) errors.Add(ExcelDataLabel.Errors.RequiredMainOccupantDateOfBirth);

		if (string.IsNullOrEmpty(socioProfessionalCategory))
			errors.Add(ExcelDataLabel.Errors.RequiredMainOccupantSocioProfessionalCategory);
	}

	private void ValidateFieldFormats(List<ExcelData> requiredFields, List<string> errors)
	{
		foreach (var field in requiredFields)
			// Check if field header corresponds to Mapping and return propertyName
			if (_identifyMilestoneHeaderMapping.TryGetValue(field.Header, out var propertyName))
			{
				// Get property type from propertyName
				var property = typeof(IdentificationMappedData).GetProperty(propertyName);
				if (property != null && property.CanWrite)
				{
					var propertyType = property.PropertyType;
					var tryParseMethod =
						typeof(ImportFileDataHelper).GetMethod("TryParseValue")!.MakeGenericMethod(propertyType);

					var parameters = new object[] { field.Value?.Trim() ?? string.Empty, null! };

					// Check if TryParseValue method is success for each fields
					var parseSuccess = (bool)tryParseMethod.Invoke(null, parameters)!;
					if (!parseSuccess)
						errors.Add(
							$"Erreur dans le format des données pour le champ : \"{field.Header}\" et la valeur : \"{field.Value}\"");
				}
			}
	}

	private ImportExcelDataCommandResult ValidateRequiredFields(
		List<ExcelData> requiredFields,
		string? mainOccupantTrigram,
		string? mainOccupantDateOfBirth,
		string? mainOccupantSocioProfessionalCategory)
	{
		var errors = new List<string>();

		ValidatedMainOccupantFields(
			mainOccupantTrigram,
			mainOccupantDateOfBirth,
			mainOccupantSocioProfessionalCategory,
			errors);

		errors.AddRange(
			requiredFields.Where(field => string.IsNullOrEmpty(field.Value))
				.Select(field => $"Le champ \"{field.Header}\" est requis."));

		ValidateFieldFormats(requiredFields, errors);
		return errors.Count <= 1
			? ImportExcelDataCommandResult.Success()
			: ImportExcelDataCommandResult.Failure(errors);
	}
}