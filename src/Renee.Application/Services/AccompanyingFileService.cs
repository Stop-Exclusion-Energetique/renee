using ExcelDataReader;
using MediatR;
using Microsoft.AspNetCore.Components;
using Renee.Application.Commands.Mail;
using Renee.Application.Commands.User;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AbortAccompanyingFile;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Application.Services.Administration.ImportExcelData;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Text;

namespace Renee.Application.Services;

public sealed class AccompanyingFileService(
	IMediator mediator,
	IImportExcelDataService importExcelDataService,
	IImportCsvDataService importCsvDataService,
	IProjectTypeService projectTypeService,
	ITelemetryService telemetryService,
	NavigationManager navigationManager)
	: IAccompanyingFileService
{
	public async Task<ReneeOperationResult<Guid?>> CreateAccompanyingFileWithQuickAdd(
		QuickAddCreatedAccompanyingFileEntities accompanyingFileEntities,
		Guid connectedUserId,
		bool zeroEnergyExclusionTerritoriesProgram,
		AccompanyingType? accompanyingType,
		Guid? territory)
	{
		return await mediator.Send(
			new CreateAccompanyingFileWithQuickAddCommandInput(
				accompanyingFileEntities,
				connectedUserId,
				zeroEnergyExclusionTerritoriesProgram,
				accompanyingType,
				territory));
	}

	public async Task<ReneeOperationResult<bool>> DeleteAccompanyingFile(DeleteAccompanyingFileCommandInput input) =>
		await mediator.Send(input);

	public async Task<ReneeOperationResult<AccompanyingFileDto?>> GetAccompanyingFileById(Guid id, Guid userId, string userRole) =>
		await mediator.Send(new GetAccompanyingFileByIdQuery(id, userId, userRole));

	public async Task<ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>>
		GetAccompanyingFileForOrganizeAndFinanceMilestone(Guid accompanyingFileId, Guid userId, string userRole) =>
		await mediator.Send(new GetAccompanyingFileForOrganizeAndFinanceMilestoneQuery(accompanyingFileId, userId, userRole));

	public async Task<ReneeOperationResult<AccompanyingFileSynthesisDto>> GetAccompanyingFileSynthesis(Guid id) =>
		await mediator.Send(new GetAccompanyingFileSynthesisQuery(id));

	public async Task<ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>> GetIdentifyValidationSynthesisModalData(
		Guid accompanyingFileId) =>
		await mediator.Send(new GetIdentifyAccompanyingFileValidationSynthesisModalByIdQuery(accompanyingFileId));

	public async Task<ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>> GetOrganizeAndFinanceSynthesis(Guid id) =>
		await mediator.Send(new GetOrganizeAndFinanceSynthesisQuery(id));

	public async Task<ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>>
		GetOrganizeAndFinanceValidationSynthesisModalData(Guid accompanyingFileId) =>
		await mediator.Send(
			new GetOrganizeAndFinanceAccompanyingFileValidationSynthesisModalByIdQuery(accompanyingFileId));

	public async Task<ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>> GetRealiseAndFollowSynthesis(Guid id) =>
		await mediator.Send(new GetRealiseAndFollowSynthesisQuery(id));

	public async Task<ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>> GetAccompanyingFilesAwaitingAnahResponse(Guid userId) =>
		await mediator.Send(new GetAllAccompanyingFilesAwaitingAnahResponseQuery(userId));

	public async Task<ReneeOperationResult<bool>> HasAccompanyingFilesAwaitingAnahResponse(Guid userId) =>
		await mediator.Send(new HasAccompanyingFilesAwaitingAnahResponseQuery(userId));

	public async Task<ImportExcelDataCommandResult> ImportAccompanyingFileData(
		Stream input,
		Guid userId,
		RequiredFieldsFromDataImportPage requiredFieldsFromDataImportPage)
	{
		var rawData = importExcelDataService.ProcessExcelFile(input);
		if (rawData is null) return ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.WhileReadingFile]);

		var validationResult = importExcelDataService.ValidateData(rawData, false);
		if (!validationResult.IsSuccess) return validationResult;

		var data = await importExcelDataService.MapExcelData(rawData, requiredFieldsFromDataImportPage);

		return await mediator.Send(new CreateAccompanyingFileFromExcelFileCommandInput(data, userId));
	}

	public async Task<ImportCsvDataCommandResult> ImportAccompanyingFileDataCsv(
		MemoryStream memoryStreamFile,
		Guid userId)
	{
		var importRunId = await mediator.Send(new CreateImportRunCommandInput());
		if (!importRunId.IsSuccess || importRunId.Value is null)
			return ImportCsvDataCommandResult.Failure([
				LineErrorReport.Create(null, Labels.Errors.UnhandledErrorOccured)]);

		try
		{
			var csvData = importCsvDataService.ProcessCsvFile(memoryStreamFile, out List<LineErrorReport> lineErrorsReporting);
			if (csvData.Count == 0)
			{
				await mediator.Send(new CompleteCsvImportErrorReportingCommandInput(
					null,
					ImportCsvResultStatus.Failure,
					(Guid)importRunId.Value,
					lineErrorsReporting));

				return ImportCsvDataCommandResult.Failure(lineErrorsReporting);
			}

			var csvMappingResult = await importCsvDataService.MapCsvData(csvData);
			var createCommand = new CreateAccompanyingFileFromCsvFileCommandInput(csvMappingResult.AccompanyingFilesToCreate, userId, (Guid)importRunId.Value);
			var updateCommand = new UpdateAccompanyingFileFromCsvFileCommandInput(csvMappingResult.AccompanyingFilesToUpdate, userId, (Guid)importRunId.Value);

			var createCommandResult = await mediator.Send(createCommand);
			var updateCommandResult = await mediator.Send(updateCommand);

			var allSuccessMessages = new List<string>();
			var allErrorMessages = lineErrorsReporting;

			allSuccessMessages.AddRange(createCommandResult.SuccessMessages ?? []);
			allSuccessMessages.AddRange(updateCommandResult.SuccessMessages ?? []);

			allErrorMessages.AddRange(createCommandResult.ErrorMessages ?? []);
			allErrorMessages.AddRange(updateCommandResult.ErrorMessages ?? []);

			var joinedErrorMessages = allErrorMessages
				.OrderBy(e => e.LineNumber)
				.ToList();

			var errorsReportingCreationResult = await mediator.Send(new CompleteCsvImportErrorReportingCommandInput(
				csvMappingResult.OperatorName,
				createCommandResult.ImportCsvResultStatus,
				(Guid)importRunId.Value,
				joinedErrorMessages ?? []));

			if (!errorsReportingCreationResult.IsSuccess || !errorsReportingCreationResult.Value)
				joinedErrorMessages?.Add(LineErrorReport.Create(null, CsvDataLabel.Errors.ErrorWhileCreatingErrorsReporting));

			if (createCommandResult.ImportCsvResultStatus == ImportCsvResultStatus.Failure &&
				updateCommandResult.ImportCsvResultStatus == ImportCsvResultStatus.Failure)
				return ImportCsvDataCommandResult.Failure(joinedErrorMessages);

			if (joinedErrorMessages?.Count > 0)
				return ImportCsvDataCommandResult.PartialSuccess(allSuccessMessages, joinedErrorMessages);
		
			return ImportCsvDataCommandResult.Success(allSuccessMessages);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);

			var report = LineErrorReport.Create(null, Labels.Errors.UnhandledErrorOccured);
			await mediator.Send(new CompleteCsvImportErrorReportingCommandInput(
				null,
				ImportCsvResultStatus.Failure,
				(Guid)importRunId.Value,
				[report]
			));

			return ImportCsvDataCommandResult.Failure([report]);
		}
	}

	public async Task<ImportExcelDataCommandResult> ImportAccompanyingFileV3Data(
		MemoryStream memoryStreamFile,
		Guid userId,
		RequiredFieldsFromDataImportPage requiredFields)
	{
		try
		{
			async Task<(IdentificationMappedData? identificationMappedData, OrganizeAndFinanceMappedData?
				organizeAndFinanceMappedData, RealizeAndFollowMappedDataV3? realizeAndFollowMappedData,
				ImportExcelDataCommandResult? importExcelDataCommandResult1)> ReadExcelFile()
			{
				var dataList = new List<ExcelData>();

				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

				using var reader = ExcelReaderFactory.CreateReader(memoryStreamFile);
				var result = reader.AsDataSet(
					new ExcelDataSetConfiguration
					{
						ConfigureDataTable = _ => new ExcelDataTableConfiguration { UseHeaderRow = true }
					});

				// Read the first excel sheet
				var dataTable = result.Tables[0];

				if (VerifyExcelFile(
						dataTable,
						dataList,
						out var validationResult,
						out var importAccompanyingFileV3Data))
					return (null, null, null, importAccompanyingFileV3Data!);
				if (validationResult is { IsSuccess: false }) return (null, null, null, validationResult);

				AddFirstMilestone(dataTable, out var identificationMappedData1, out var failure);
				if (failure != null) return (identificationMappedData1, null, null, failure);

				var organizeAndFinanceMappedData1 = await AddSecondMilestone(dataTable);

				var realizeAndFollowMappedDataV3 = AddThirdMilestone(dataTable);

				if (requiredFields.ReferentSolidarBuilderId == null)
					return (identificationMappedData1, organizeAndFinanceMappedData1, realizeAndFollowMappedDataV3,
						ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.SolidarBuilderNotFound]));
				return (identificationMappedData1, organizeAndFinanceMappedData1, realizeAndFollowMappedDataV3, null);
			}

			var (identificationMappedData, organizeAndFinanceMappedData, realizeAndFollowMappedData,
				importExcelDataCommandResult1) = await ReadExcelFile();
			if (importExcelDataCommandResult1 is { IsSuccess: false }) return importExcelDataCommandResult1;
			if (identificationMappedData == null ||
				organizeAndFinanceMappedData == null ||
				realizeAndFollowMappedData == null)
				return ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.WhileReadingFile]);

			var importSupportTeam = new ImportSupportTeam(
				requiredFields.ReferentDiffuseCoordinatorId,
				requiredFields.ReferentTargetCoordinatorId,
				requiredFields.ReferentEtId,
				(Guid)requiredFields.ReferentSolidarBuilderId!,
				identificationMappedData!.MarkerNature,
				identificationMappedData.CommentOnMarkerNature);

			var importAddress = new ImportAddress(
				identificationMappedData.StreetNumberName,
				identificationMappedData.PostalCode,
				identificationMappedData.Municipality,
				identificationMappedData.Department,
				identificationMappedData.Region,
				identificationMappedData.AdditionnalAddress ?? string.Empty);

			var importInitialState = new ImportHousingInitialStateV3(
				identificationMappedData.DegradationIndex,
				identificationMappedData.UnsanitaryCoefficient,
				identificationMappedData.AnnualEnergyConsumptionBeforeWork,
				identificationMappedData.StartingDpe,
				identificationMappedData.StartingGes,
				identificationMappedData.EnergyDeprivation);

			var importAfterWorkState = new ImportHousingAfterWorkStateV3(
				organizeAndFinanceMappedData.EstimatedDpeLabelAfterWork,
				organizeAndFinanceMappedData.EstimatedGesLabelAfterWork,
				organizeAndFinanceMappedData.EstimatedEnergyConsumptionAfterWork,
				organizeAndFinanceMappedData.EstimatedDpeJumpClass);

			var mainOccupantToCreate = new OccupantToCreate(
				identificationMappedData.Occupants[0].Trigram,
				TryParseMultipleFormatDateTime(identificationMappedData.Occupants[0].DateOfBirth)!.Value.Date,
				EnumHelper.GetEnumValueFromDescription<SocioProfessionalCategory>(
					identificationMappedData.Occupants[0].SocioprofessionalCategory),
				requiredFields.LastName,
				requiredFields.FirstName);

			var secondaryOccupantToCreate = identificationMappedData.Occupants
				.Where(so => so != identificationMappedData.Occupants[0]).Select(o => new OccupantToCreate(
					o.Trigram,
					TryParseMultipleFormatDateTime(o.DateOfBirth)!.Value.Date,
					null,
					null,
					null)).ToList();

			var importHousehold = new ImportHousehold(
				identificationMappedData.IncomeTaxReference,
				identificationMappedData.HouseholdTypology,
				identificationMappedData.SocialContext,
				identificationMappedData.HasOverdueInvoice,
				identificationMappedData.AnahCategory,
				mainOccupantToCreate.CreateMainOccupant(),
				secondaryOccupantToCreate.Select(so => so.CreateSecondaryOccupant()).ToList());

			var importHousing = new ImportHousing(
				identificationMappedData.ConstructionYear,
				identificationMappedData.LivingSpace,
				identificationMappedData.HousingType,
				identificationMappedData.GeographicalHousingAreaTypology,
				identificationMappedData.OwnershipStatus,
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
				realizeAndFollowMappedData.IsBackToEmployment,
				realizeAndFollowMappedData.HasHousingAdaptationWorks,
				realizeAndFollowMappedData.HasFinishingWorks,
				realizeAndFollowMappedData.HasSafetyWorks,
				realizeAndFollowMappedData.HasPreparationWorks,
				realizeAndFollowMappedData.HasEmergencyWorks,
				realizeAndFollowMappedData.HasUnsanitaryExit,
				realizeAndFollowMappedData.TreatedAirTightness,
				realizeAndFollowMappedData.TreatedThermalBridges,
				realizeAndFollowMappedData.HasHumidityManagement);

			var importInvoice = new ImportInvoice(
				realizeAndFollowMappedData.TotalCost,
				realizeAndFollowMappedData.BilledWorkForce);

			var data = new ImportAccompanyingFileFromFileInputCommandInput(
				identificationMappedData.Reference,
				identificationMappedData.FirstContactDate,
				identificationMappedData.StartSupportDate,
				realizeAndFollowMappedData.EndOfEncounterDate,
				realizeAndFollowMappedData.EndOfAccompanyingDate,
				identificationMappedData.MarkerNature,
				identificationMappedData.CommentOnMarkerNature,
				identificationMappedData.ContactWithFamilyForIdentificationMilestones,
				organizeAndFinanceMappedData.ContactWithFamilyForOrganizeAndFinanceMilestone,
				realizeAndFollowMappedData.ContactWithFamilyForRealizeAndFollowMilestone,
				requiredFields.ZeroEnergyExclusionTerritoriesProgram,
				requiredFields.AccompanyingType,
				requiredFields.TerritoryId,
				importSupportTeam,
				importHousehold,
				importHousing,
				importPreWorkPlan,
				importPreFinancingPlan,
				importWorkMonitoring,
				importInvoice);

			return await mediator.Send(new CreateAccompanyingFileFromExcelFileCommandInput(data, userId));
		}
		catch (Exception exception) { return ImportExcelDataCommandResult.Failure([exception.Message]); }
	}

	public async Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForIdentificationMilestone(
		SaveAccompanyingFileIdentificationMilestoneCommandInput input) =>
		await mediator.Send(input);

	public async Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForOrganizeAndFinanceMilestone(
		SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput input) =>
		await mediator.Send(input);

	public async Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForRealizeAndFollowMilestone(
		SaveAccompanyingFileRealizeAndFollowCommandInput input)
	{
		return await mediator.Send(input);
	}

	public async Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileIdentificationSynthesis(
		Guid accompanyingFileId,
		AccompanyingFileStatus status,
		AccompanyingFileStage stage,
		Guid userId,
		bool shouldNotifyUsers)
	{
		var stageValidationResult = await mediator.Send(
			new SaveIdentificationMilestoneSynthesisValidationCommandInput(
				accompanyingFileId,
				status,
				stage,
				userId));

		if (!shouldNotifyUsers || !stageValidationResult.IsSuccess || stageValidationResult.Value == null)
			return stageValidationResult;

		var requiredData = stageValidationResult.Value;

		await mediator.Send(new SendMailCommand(
			MailType.StageReady,
			requiredData.UsersEmail!,
			new MailParameters
			{
				Param1 = requiredData.AccompanyingFileStage.GetDescription(),
				Param2 = requiredData.MainOccupantFullName,
				Param3 = requiredData.AccompanyingType.GetDescription(),
				Param4 = requiredData.SolidarBuilderFullName,
				Param5 = GetStageRedirectionUrl(accompanyingFileId, requiredData.AccompanyingFileStage)
			}));

		return stageValidationResult;
	}

	public async Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileOrganizeAndFinanceSynthesis(
		Guid accompanyingFileId,
		AccompanyingFileStatus status,
		AccompanyingFileStage stage,
		Guid userId,
		bool shouldNotifyUsers)
	{
		var stageValidationResult = await mediator.Send(
			new SaveOrganizeAndFinanceMilestoneSynthesisValidationCommandInput(
				accompanyingFileId,
				status,
				stage,
				userId));

		if (!shouldNotifyUsers || !stageValidationResult.IsSuccess || stageValidationResult.Value == null)
			return stageValidationResult;

		var requiredData = stageValidationResult.Value;

		await mediator.Send(new SendMailCommand(
			MailType.StageReady,
			requiredData.UsersEmail!,
			new MailParameters
			{
				Param1 = requiredData.AccompanyingFileStage.GetDescription(),
				Param2 = requiredData.MainOccupantFullName,
				Param3 = requiredData.AccompanyingType.GetDescription(),
				Param4 = requiredData.SolidarBuilderFullName,
				Param5 = GetStageRedirectionUrl(accompanyingFileId, requiredData.AccompanyingFileStage)
			}));

		return stageValidationResult;
	}

	public async Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileRealizeAndFollowSynthesis(
		Guid accompanyingFileId,
		bool userCanValidateSynthesis,
		Guid userId,
		bool shouldNotifyUsers)
	{
		var stageValidationResult = await mediator.Send(
			new SaveRealizeAndFollowMilestoneSynthesisValidationCommandInput(
				accompanyingFileId,
				userId,
				userCanValidateSynthesis));

		if (!shouldNotifyUsers || !stageValidationResult.IsSuccess || stageValidationResult.Value == null)
			return stageValidationResult;

		var requiredData = stageValidationResult.Value;

		await mediator.Send(new SendMailCommand(
			MailType.StageReady,
			requiredData.UsersEmail!,
			new MailParameters
			{
				Param1 = requiredData.AccompanyingFileStage.GetDescription(),
				Param2 = requiredData.MainOccupantFullName,
				Param3 = requiredData.AccompanyingType.GetDescription(),
				Param4 = requiredData.SolidarBuilderFullName,
				Param5 = GetStageRedirectionUrl(accompanyingFileId, requiredData.AccompanyingFileStage)
			}));

		return stageValidationResult;
	}

	public async Task<ReneeOperationResult<RequiredDataForStageValidationEmail>> ValidateStage(Guid accompanyingFileId, bool isValidated, Guid userId, string? commentOnValidation)
	{
		var stageValidationResult = await mediator.Send(new ValidateAccompanyingFileStageChangeCommandInput(accompanyingFileId, isValidated, userId, commentOnValidation));

		if (!stageValidationResult.IsSuccess || stageValidationResult.Value == null)
			return stageValidationResult;

		var requiredData = stageValidationResult.Value;

		await mediator.Send(new SendMailCommand(
			isValidated ? MailType.StageValidated : MailType.StageRefused,
			requiredData.SolidarBuildersEmail!,
			new MailParameters
			{
				Param1 = requiredData.MainOccupantFullName,
				Param2 = requiredData.SolidarBuilderFirstName,
				Param3 = requiredData.CurrentStage.GetDescription(),
				Param4 = requiredData.AccompanyingFileReference,
				Param5 = requiredData.ValidatorEmail,
				Param6 = GetStageRedirectionUrl(accompanyingFileId, requiredData.UpdatedStage),
				Param7 = !string.IsNullOrEmpty(commentOnValidation) ? commentOnValidation : Labels.NoRejectionCommentEmailText
			}));

		return stageValidationResult;
	}

	public async Task<ReneeStringOperationResult> AbortAccompanyingFile(AbortAccompanyingFileCommandInput input) => 
		await mediator.Send(input);

	private string GetStageRedirectionUrl(Guid accompanyingFileId, AccompanyingFileStage currentStage)
	{
		var stageEndpoint = currentStage switch
		{
			AccompanyingFileStage.Identify => Endpoints.NewOccupant,
			AccompanyingFileStage.OrganizingAndFinancing => Endpoints.OrganizeAndFinanceStage,
			AccompanyingFileStage.RealisationAndFollowing or AccompanyingFileStage.Finished => Endpoints.RealiseAndFollowStage,
			_ => throw new InvalidEnumArgumentException($"{nameof(AccompanyingFileStage)} is an invalid stage")
		};

		return navigationManager.ToAbsoluteUri($"{stageEndpoint}/{accompanyingFileId}").ToString();
	}

	private static void AddFirstMilestone(
		DataTable dataTable,
		out IdentificationMappedData? identificationMappedData,
		out ImportExcelDataCommandResult? failure)
	{
		var contactWithFamilyDuringIdentifyStage = dataTable.Rows[48][4].ToString()?.Trim();
		var reference = dataTable.Rows[2][4].ToString()?.Trim();
		var firstContactDate = dataTable.Rows[8][4].ToString()?.Trim();
		var startSupportDate = dataTable.Rows[9][4].ToString()?.Trim();
		var streetNumberName = dataTable.Rows[18][4].ToString()?.Trim();
		var postalCode = dataTable.Rows[20][4].ToString()?.Trim();
		var municipality = dataTable.Rows[21][4].ToString()?.Trim();
		var department = dataTable.Rows[22][4].ToString()?.Trim();
		var region = dataTable.Rows[23][4].ToString()?.Trim();

		var degradationIndex = dataTable.Rows[47][4].ToString()?.Trim();
		var unsanitaryCoefficient = dataTable.Rows[46][4].ToString()?.Trim();

		if (reference == null ||
			firstContactDate == null ||
			startSupportDate == null ||
			streetNumberName == null ||
			postalCode == null ||
			municipality == null ||
			department == null ||
			region == null)
		{
			failure = ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.WhileSavingFolder]);
			identificationMappedData = null;
			return;
		}

		var markerNature =
			(int?)EnumHelper.GetEnumValueFromDescription<MarkerNatureV3>(dataTable.Rows[6][4].ToString()?.Trim());
		_ = double.TryParse(dataTable.Rows[42][4].ToString()?.Trim(), out var annualEnergyConsumptionBeforeWork);

		var dateFirstContact = TryParseMultipleFormatDateTime(firstContactDate)!.Value;
		var dateStartSupportDate = TryParseMultipleFormatDateTime(startSupportDate)!.Value;

		identificationMappedData =
			new
				IdentificationMappedData(
					reference,
					dateFirstContact,
					dateStartSupportDate,
					streetNumberName,
					postalCode,
					municipality,
					department,
					region)
			{
				CommentOnMarkerNature = dataTable.Rows[7][4].ToString()?.Trim(),
				MarkerNature = markerNature is null ? MarkerNature.Other : (MarkerNature)markerNature,
				ContactWithFamilyForIdentificationMilestones =
						!string.IsNullOrEmpty(contactWithFamilyDuringIdentifyStage)
							? int.Parse(contactWithFamilyDuringIdentifyStage)
							: null,
				DegradationIndex = EnumHelper.GetEnumValueFromDescription<DegradationIndex>(degradationIndex),
				UnsanitaryCoefficient =
						EnumHelper.GetEnumValueFromDescription<UnsanitaryCoefficient>(unsanitaryCoefficient),
				AdditionnalAddress = dataTable.Rows[19][4].ToString()?.Trim(),
				AnahCategory = dataTable.Rows[38][4].ToString()?.Trim(),
				AnnualEnergyConsumptionBeforeWork = annualEnergyConsumptionBeforeWork,
				ConstructionYear =
						int.TryParse(dataTable.Rows[31][4].ToString()?.Trim(), out var constructionYear)
							? constructionYear
							: null,
				EnergyDeprivation =
						EnumHelper.GetEnumValueFromDescription<EnergyDeprivation>(
							dataTable.Rows[45][4].ToString()?.Trim()),
				GeographicalHousingAreaTypology =
						EnumHelper.GetEnumValueFromDescription<GeographicalHousingAreaTypology>(
							dataTable.Rows[24][4].ToString()?.Trim()),
				HouseholdTypology =
						EnumHelper.GetEnumValueFromDescription<HouseholdTypology>(
							dataTable.Rows[25][4].ToString()?.Trim()),
				HousingType =
						EnumHelper.GetEnumValueFromDescription<HousingType>(dataTable.Rows[30][4].ToString()?.Trim()),
				IncomeTaxReference =
						double.TryParse(dataTable.Rows[33][4].ToString()?.Trim(), out var incomeTaxReference)
							? incomeTaxReference
							: null,
				LivingSpace =
						int.TryParse(dataTable.Rows[32][4].ToString()?.Trim(), out var livingSpace)
							? livingSpace
							: null,
				OwnershipStatus =
						EnumHelper.GetEnumValueFromDescription<OwnershipStatus>(
							dataTable.Rows[29][4].ToString()?.Trim()),
				SocialContext = dataTable.Rows[28][4].ToString()?.Trim(),
				HasOverdueInvoice =
						string.Equals(
							dataTable.Rows[41][4].ToString()?.Trim(),
							"Oui",
							StringComparison.CurrentCultureIgnoreCase),
				StartingDpe =
						EnumHelper.GetEnumValueFromDescription<DpeLabel>(dataTable.Rows[39][4].ToString()?.Trim()),
				StartingGes =
						EnumHelper.GetEnumValueFromDescription<GesLabel>(dataTable.Rows[40][4].ToString()?.Trim()),
				Occupants = []
			};

		if (!string.IsNullOrEmpty(dataTable.Rows[11][4].ToString()?.Trim()) &&
			!string.IsNullOrEmpty(dataTable.Rows[12][4].ToString()?.Trim()) &&
			!string.IsNullOrEmpty(dataTable.Rows[13][4].ToString()?.Trim()))
			identificationMappedData.Occupants.Add(
				new ImportOccupantExcelData(
					dataTable.Rows[11][4].ToString()?.Trim()!,
					dataTable.Rows[12][4].ToString()?.Trim()!,
					dataTable.Rows[13][4].ToString()?.Trim()));
		if (!string.IsNullOrEmpty(dataTable.Rows[14][4].ToString()?.Trim()) &&
			!string.IsNullOrEmpty(dataTable.Rows[15][4].ToString()?.Trim()))
		{
			identificationMappedData.Occupants.Add(
				new ImportOccupantExcelData(
					dataTable.Rows[14][4].ToString()?.Trim()!,
					dataTable.Rows[15][4].ToString()?.Trim()!,
					null));
		}

		if (!string.IsNullOrEmpty(dataTable.Rows[16][4].ToString()?.Trim()) &&
			!string.IsNullOrEmpty(dataTable.Rows[17][4].ToString()?.Trim()))
		{
			identificationMappedData.Occupants.Add(
				new ImportOccupantExcelData(
					dataTable.Rows[16][4].ToString()?.Trim()!,
					dataTable.Rows[17][4].ToString()?.Trim()!,
					null));
		}

		failure = null;
	}

	private async Task<List<Guid>> AddProjectTypeIds(DataTable dataTable)
	{
		var projectTypesResult = await projectTypeService.GetAllProjectTypes();
		var projectTypes = projectTypesResult.IsSuccess ? projectTypesResult.Value!.ToList() : [];
		var projectTypeIds = new List<Guid>();
		if (string.Equals(dataTable.Rows[78][4].ToString()?.Trim(), "Oui", StringComparison.CurrentCultureIgnoreCase))
			projectTypeIds.Add(
				projectTypes.FirstOrDefault(x =>
					x.Label == ExcelDataLabel.OrganizeAndFinanceMilestone.HousingAdaptationWork)?.Id ??
				Guid.Empty);

		if (string.Equals(dataTable.Rows[79][4].ToString()?.Trim(), "Oui", StringComparison.CurrentCultureIgnoreCase))
			projectTypeIds.Add(
				projectTypes.FirstOrDefault(x => x.Label == ExcelDataLabel.OrganizeAndFinanceMilestone.FinishingWork)
					?.Id ??
				Guid.Empty);

		if (string.Equals(dataTable.Rows[80][4].ToString()?.Trim(), "Oui", StringComparison.CurrentCultureIgnoreCase))
			projectTypeIds.Add(
				projectTypes.FirstOrDefault(x => x.Label == ExcelDataLabel.OrganizeAndFinanceMilestone.SecurityWork)
					?.Id ??
				Guid.Empty);

		if (string.Equals(dataTable.Rows[81][4].ToString()?.Trim(), "Oui", StringComparison.CurrentCultureIgnoreCase))
			projectTypeIds.Add(
				projectTypes.FirstOrDefault(x => x.Label == ExcelDataLabel.OrganizeAndFinanceMilestone.PreparationWork)
					?.Id ??
				Guid.Empty);

		if (string.Equals(dataTable.Rows[83][4].ToString()?.Trim(), "Oui", StringComparison.CurrentCultureIgnoreCase))
			projectTypeIds.Add(
				projectTypes.FirstOrDefault(x => x.Label == ExcelDataLabel.OrganizeAndFinanceMilestone.UnsanitaryExit)
					?.Id ??
				Guid.Empty);

		return projectTypeIds;
	}

	private async Task<OrganizeAndFinanceMappedData> AddSecondMilestone(DataTable dataTable)
	{
		//jalon 2
		var contactWithFamilyDuringOrganizeAndFinanceStage = dataTable.Rows[69][4].ToString()?.Trim();

		var projectTypeIds = await AddProjectTypeIds(dataTable);

		var publicEstablishmentsIntercommunalCooperation = dataTable.Rows[61][4].ToString()?.Trim();

		var organizeAndFinanceMappedData = new OrganizeAndFinanceMappedData
		{
			ProjectTypes = projectTypeIds,
			ContactWithFamilyForOrganizeAndFinanceMilestone =
				!string.IsNullOrEmpty(contactWithFamilyDuringOrganizeAndFinanceStage)
					? int.Parse(contactWithFamilyDuringOrganizeAndFinanceStage)
					: null,
			PublicEstablishmentsIntercommunalCooperation =
				!string.IsNullOrEmpty(publicEstablishmentsIntercommunalCooperation) &&
				double.TryParse(publicEstablishmentsIntercommunalCooperation, out var d)
					? d
					: null,
			RenovationType =
				EnumHelper.GetEnumValueFromDescription<RenovationType>(dataTable.Rows[51][4].ToString()?.Trim()),
			NextStepAndVigilancePoints = dataTable.Rows[52][4].ToString()?.Trim(),
			IsEmergencyWorks =
				string.Equals(
					dataTable.Rows[82][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			TreatedAirTightness =
				EnumHelper.GetEnumValueFromDescription<PartlyStateTreatment>(
					dataTable.Rows[84][4].ToString()?.Trim()),
			TreatedThermalBridge =
				EnumHelper.GetEnumValueFromDescription<PartlyStateTreatment>(
					dataTable.Rows[85][4].ToString()?.Trim()),
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
				EnumHelper.GetEnumValueFromDescription<PartlyStateTreatment>(
					dataTable.Rows[86][4].ToString()?.Trim()),
			EstimatedDpeLabelAfterWork =
				EnumHelper.GetEnumValueFromDescription<DpeLabel>(dataTable.Rows[54][4].ToString()?.Trim()),
			EstimatedGesLabelAfterWork =
				EnumHelper.GetEnumValueFromDescription<GesLabel>(dataTable.Rows[55][4].ToString()?.Trim()),
			EstimatedEnergyConsumptionAfterWork =
				double.TryParse(dataTable.Rows[56][4].ToString()?.Trim(), out var energy) ? energy : null,
			EstimatedDpeJumpClass =
				int.TryParse(dataTable.Rows[57][4].ToString()?.Trim(), out var dpeJump) ? dpeJump : null,
			RegionAids =
				double.TryParse(dataTable.Rows[59][4].ToString()?.Trim(), out var regionAids) ? regionAids : null,
			DepartmentAids =
				double.TryParse(dataTable.Rows[60][4].ToString()?.Trim(), out var departmentAids)
					? departmentAids
					: null,
			MunicipalityAids =
				double.TryParse(dataTable.Rows[62][4].ToString()?.Trim(), out var municipalityAids)
					? municipalityAids
					: null,
			PrivateActors =
				double.TryParse(dataTable.Rows[63][4].ToString()?.Trim(), out var privateActors)
					? privateActors
					: null,
			PensionFunds =
				double.TryParse(dataTable.Rows[64][4].ToString()?.Trim(), out var pensionFunds)
					? pensionFunds
					: null,
			HouseholdMaximumSavingAmountForRenovationProject =
				double.TryParse(dataTable.Rows[65][4].ToString()?.Trim(), out var maxAmountHouseholdSavings)
					? maxAmountHouseholdSavings
					: null,
			MaximumAmountSupportFamilyMembersRenovationProject =
				double.TryParse(dataTable.Rows[66][4].ToString()?.Trim(), out var maxAmountSupportFamilyMembers)
					? maxAmountSupportFamilyMembers
					: null,
			EstimatedRemainingAmount =
				double.TryParse(dataTable.Rows[67][4].ToString()?.Trim(), out var estimatedRemainingAmount)
					? estimatedRemainingAmount
					: null
		};
		return organizeAndFinanceMappedData;
	}

	private static RealizeAndFollowMappedDataV3 AddThirdMilestone(DataTable dataTable)
	{
		var realizeAndFollowMappedData = new RealizeAndFollowMappedDataV3
		{
			TotalCost =
				double.TryParse(dataTable.Rows[71][4].ToString()?.Trim(), out var totalCost) ? totalCost : null,
			BilledWorkForce =
				double.TryParse(dataTable.Rows[72][4].ToString()?.Trim(), out var billedWorkForce)
					? billedWorkForce
					: null,
			HouseholdAutoFinancing =
				double.TryParse(dataTable.Rows[74][4].ToString()?.Trim(), out var householdAutoFinancing)
					? householdAutoFinancing
					: null,
			IntermediateAirtightnessTestResult = dataTable.Rows[75][4].ToString()?.Trim(),
			WaterproofingTreatmentActions = dataTable.Rows[76][4].ToString()?.Trim(),
			HasEffectiveComplianceWithWorkRecommendations = 
				string.Equals(
					dataTable.Rows[77][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasHousingAdaptationWorks =
				string.Equals(
					dataTable.Rows[78][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasFinishingWorks =
				string.Equals(
					dataTable.Rows[79][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasSafetyWorks =
				string.Equals(
					dataTable.Rows[80][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasPreparationWorks =
				string.Equals(
					dataTable.Rows[81][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasEmergencyWorks =
				string.Equals(
					dataTable.Rows[82][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasUnsanitaryExit =
				string.Equals(
					dataTable.Rows[83][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			TreatedAirTightness =
				EnumHelper.GetEnumValueFromDescription<PartlyStateTreatment>(
					dataTable.Rows[84][4].ToString()?.Trim()),
			TreatedThermalBridges =
				EnumHelper.GetEnumValueFromDescription<PartlyStateTreatment>(
					dataTable.Rows[85][4].ToString()?.Trim()),
			HasHumidityManagement =
				string.Equals(
					dataTable.Rows[86][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			HasWorkEnablingHomeSupport =
				string.Equals(
					dataTable.Rows[87][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			WellBeingRating =
				int.TryParse(dataTable.Rows[88][4].ToString()?.Trim(), out var wellBeingRating)
					? wellBeingRating
					: null,
			EducationalFrameworkRating =
				int.TryParse(dataTable.Rows[89][4].ToString()?.Trim(), out var educationalFrameworkRating)
					? educationalFrameworkRating
					: null,
			FamilySatisfactionWithSupport =
				int.TryParse(dataTable.Rows[91][4].ToString()?.Trim(), out var familySatisfactionWithSupport)
					? familySatisfactionWithSupport
					: null,
			IsBackToEmployment =
				string.Equals(
					dataTable.Rows[90][4].ToString()?.Trim(),
					"Oui",
					StringComparison.CurrentCultureIgnoreCase),
			EndOfAccompanyingDate = TryParseMultipleFormatDateTime(dataTable.Rows[92][4].ToString()?.Trim()),
			EndOfEncounterDate = TryParseMultipleFormatDateTime(dataTable.Rows[94][4].ToString()?.Trim()),
			ContactWithFamilyForRealizeAndFollowMilestone = int.TryParse(
				dataTable.Rows[95][4].ToString()?.Trim(),
				out var contactWithFamilyForRealizeAndFollowMilestone)
				? contactWithFamilyForRealizeAndFollowMilestone
				: null
		};
		return realizeAndFollowMappedData;
	}

	private static DateTime? TryParseMultipleFormatDateTime(string? dateTimeString)
	{
		if (string.IsNullOrWhiteSpace(dateTimeString)) return null;

		var cultureFr = CultureInfo.GetCultureInfo("fr-FR");
		if (DateTime.TryParseExact(dateTimeString, "G", cultureFr, DateTimeStyles.None, out var dateTime3))
			return dateTime3;

		if (DateTime.TryParseExact(dateTimeString, "d", cultureFr, DateTimeStyles.None, out var dateTime4))
			return dateTime4;

		if (DateTime.TryParseExact(
				dateTimeString,
				"MM/dd/yyyy hh:mm:ss",
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var dateTime))
			return dateTime;

		if (DateTime.TryParseExact(
				dateTimeString,
				"MM/dd/yyyy",
				CultureInfo.InvariantCulture,
				DateTimeStyles.None,
				out var dateTime2))
			return dateTime2;

		throw new InvalidCastException(
			$"Unable to convert string to DateTime {dateTimeString}, {CultureInfo.DefaultThreadCurrentCulture?.DisplayName}");
	}

	private bool VerifyExcelFile(
		DataTable dataTable,
		List<ExcelData> dataList,
		out ImportExcelDataCommandResult? validationResult,
		out ImportExcelDataCommandResult? importAccompanyingFileV3Data)
	{
		// Index begins at 2 to avoid useless file headers
		for (var i = 2; i < dataTable.Rows.Count; i++)
		{
			var header = dataTable.Rows[i][3]?.ToString(); // Column C
			var value = dataTable.Rows[i][4] is DateTime dateTimeValue // Column D
				? dateTimeValue.ToString(Labels.DateFormatUtc, CultureInfo.InvariantCulture)
				: dataTable.Rows[i][4].ToString();

			if (string.IsNullOrWhiteSpace(header)) continue;

			// This value corresponds to another header (EnergeticEfficiency)
			if (header == Labels.PreWorkPlanProjectType)
				header = ExcelDataLabel.OrganizeAndFinanceMilestone.EnergeticEfficiency;

			if (!string.IsNullOrWhiteSpace(value) &&
				(header.Contains(Labels.DegradationIndex) || header.Contains(Labels.UnsanitaryCoefficient)))
			{
				var extractedValueWithoutMiddleSpaces = String.Join("", value.Split(" "));
				value = extractedValueWithoutMiddleSpaces;
			}

			dataList.Add(new ExcelData(header.Trim(), value?.Trim() ?? null));
		}

		if (dataList.Count == 0)
		{
			importAccompanyingFileV3Data =
				ImportExcelDataCommandResult.Failure([ExcelDataLabel.Errors.WhileReadingFile]);
			validationResult = null;
			return true;
		}

		validationResult = importExcelDataService.ValidateData(dataList, true);
		importAccompanyingFileV3Data = null;
		return false;
	}

	public async Task SendAccompanyingFileAbortMails(
		Guid accompanyingFileId, 
		MailType mailType = MailType.AbortAccompanyingFileRequest, 
		bool sendToSolidarBuilder = false, 
		string comment = Labels.NoComment)
	{
		var result = await mediator.Send(new GetAbortMailDataQuery(accompanyingFileId, sendToSolidarBuilder));

		if (!result.IsSuccess || result.Value == null)
			return;

		var abortData = result.Value;

		switch (mailType)
		{
			case MailType.AbortAccompanyingFileRequest:
				await mediator.Send(new SendMailCommand(
					mailType,
					abortData.RecipientEmail,
					new MailParameters 
					{ 
						Param1 = abortData.AccompanyingFileReference,
						Param2 = abortData.AccompanyingFileStage.GetDescription(),
						Param3 = abortData.SolidarBuilderFullName,
						Param4 = abortData.IsBillingRequested ? Labels.AbortWithBillingRequest : Labels.AbortWithoutBillingRequest,
						Param5 = StringHelper.BoolToString(abortData.IsBillingRequested),
						Param6 = GetStageRedirectionUrl(accompanyingFileId, abortData.AccompanyingFileStage)
					}));
				break;
			case MailType.Abort:
				await mediator.Send(new SendMailCommand(
					mailType,
					abortData.RecipientEmail,
					new MailParameters 
					{ 
						Param1 = abortData.SolidarBuilderFullName,
						Param2 = abortData.AccompanyingFileReference, 
						Param3 = abortData.ValidatorFullName,
						Param4 = GetStageRedirectionUrl(accompanyingFileId, abortData.AccompanyingFileStage)
					}));
				break;
			case MailType.AbortCancellation:
				await mediator.Send(new SendMailCommand(
					mailType,
					abortData.RecipientEmail,
					new MailParameters 
					{ 
						Param1 = abortData.SolidarBuilderFullName,
						Param2 = abortData.AccompanyingFileReference, 
						Param3 = abortData.ValidatorFullName,
						Param4 = comment,
						Param5 = GetStageRedirectionUrl(accompanyingFileId, abortData.AccompanyingFileStage)
					}));
				break;
			default:
				throw new InvalidEnumArgumentException($"{nameof(MailType)} valeur non prise en charge pour l'abandon d'un dossier : {mailType}");
		}
	}

	public async Task<ReneeStringOperationResult> ValidateOrNotAccompanyingFileAbort(Guid accompanyingFileId, Guid userId, string commentOnAbort, string accompanyingFileReference, string userName, bool shouldAbort)
	{
		var commandResult = await mediator.Send(new ValidateAccompanyingFileAbortCommandInput(accompanyingFileId, commentOnAbort, shouldAbort, userId));

		if (commandResult.IsSuccess)
			await SendAccompanyingFileAbortMails(
				accompanyingFileId,
				shouldAbort ? MailType.Abort : MailType.AbortCancellation,
				true,
				commentOnAbort
			);

		return commandResult;
	}

    public async Task<ReneeStringOperationResult> UpdateAccompanyingFileTargetInformations(UpdateAccompanyingFileTargetInformationsCommandInput input) => await mediator.Send(input);

    public async Task<ReneeStringOperationResult> UpdateAccompanyingFileFacturationInformations(UpdateAccompanyingFileFacturationInformationsCommandInput input) => await mediator.Send(input);

	public async Task<ReneeOperationResult<bool>> UpdateAccompanyingFilesForAnahGrantCheck(Guid userId, List<AccompanyingFileGrantDateResume> grantDates)
	{ 
		var updateAccompanyingFilesResult = await mediator.Send(new SaveAccompanyingFilesForAnahGrantCheckCommandInput(userId, grantDates));

		if (!updateAccompanyingFilesResult.IsSuccess || !updateAccompanyingFilesResult.Value)
			return updateAccompanyingFilesResult;

		var updateUserResult = await mediator.Send(new UpdateUserNextDateForAnahGrantCheckCommand(userId));

		return updateUserResult;
	}
}