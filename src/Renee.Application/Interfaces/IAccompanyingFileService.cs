using Renee.Application.CommandsUseCasesInput;
using Renee.Application.CommandsUseCasesInput.Results;
using Renee.Application.DTOs.AccompanyingFile;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IAccompanyingFileService
{
	Task<ReneeOperationResult<Guid?>> CreateAccompanyingFileWithQuickAdd(
		QuickAddCreatedAccompanyingFileEntities accompanyingFileEntities,
		Guid connectedUserId,
		bool zeroEnergyExclusionTerritoriesProgram,
		AccompanyingType? accompanyingType,
		Guid? territory);

	Task<ReneeOperationResult<bool>> DeleteAccompanyingFile(DeleteAccompanyingFileCommandInput input);

	Task<ReneeOperationResult<AccompanyingFileDto?>> GetAccompanyingFileById(Guid id, Guid userId, string userRole);

	Task<ReneeOperationResult<GetAccompanyingFileForOrganizeAndFinanceMilestoneQueryObjectResult>>
		GetAccompanyingFileForOrganizeAndFinanceMilestone(Guid accompanyingFileId, Guid userId, string userRole);

	Task<ReneeOperationResult<AccompanyingFileSynthesisDto>> GetAccompanyingFileSynthesis(Guid id);

	Task<ReneeOperationResult<IdentifyAccompanyingFileValidationSynthesisModalDto?>> GetIdentifyValidationSynthesisModalData(
		Guid accompanyingFileId);

	Task<ReneeOperationResult<GetOrganizeAndFinanceSynthesisQueryObjectResult>> GetOrganizeAndFinanceSynthesis(Guid id);

	Task<ReneeOperationResult<OrganizeAndFinanceAccompanyingFileValidationSynthesisModalDto?>>
		GetOrganizeAndFinanceValidationSynthesisModalData(Guid accompanyingFileId);

	Task<ReneeOperationResult<GetRealiseAndFollowSynthesisQueryObjectResult>> GetRealiseAndFollowSynthesis(Guid id);

	Task<ReneeOperationResult<List<AccompanyingFileAwaitingAnahResponseResume>>> GetAccompanyingFilesAwaitingAnahResponse(Guid userId);

	Task<ReneeOperationResult<bool>> HasAccompanyingFilesAwaitingAnahResponse(Guid userId);

	Task<ImportExcelDataCommandResult> ImportAccompanyingFileData(
		Stream input,
		Guid userId,
		RequiredFieldsFromDataImportPage requiredFieldsFromDataImportPage);

	Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForIdentificationMilestone(
		SaveAccompanyingFileIdentificationMilestoneCommandInput input);

	Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForOrganizeAndFinanceMilestone(
		SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput input);

	Task<ReneeOperationResult<bool>> UpdateAccompanyingFileForRealizeAndFollowMilestone(
		SaveAccompanyingFileRealizeAndFollowCommandInput input);

	Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileIdentificationSynthesis(
		Guid accompanyingFileId,
		AccompanyingFileStatus status,
		AccompanyingFileStage stage,
		Guid userId,
		bool shouldNotifyUsers);

	Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileOrganizeAndFinanceSynthesis(
		Guid accompanyingFileId,
		AccompanyingFileStatus status,
		AccompanyingFileStage stage,
		Guid userId,
		bool shouldNotifyUsers);

	Task<ReneeOperationResult<RequiredDataForStageSynthesisValidationEmail>> UpdateAccompanyingFileRealizeAndFollowSynthesis(
		Guid accompanyingFileId,
		bool userCanValidateSynthesis,
		Guid userId,
		bool shouldNotifyUsers);

	Task<ReneeStringOperationResult> AbortAccompanyingFile(AbortAccompanyingFileCommandInput input);

	Task<ReneeOperationResult<RequiredDataForStageValidationEmail>> ValidateStage(Guid accompanyingFileId, bool isValidated, Guid userId, string? commentOnValidation);
	Task<ImportExcelDataCommandResult> ImportAccompanyingFileV3Data(
		MemoryStream memoryStreamFile,
		Guid userId,
		RequiredFieldsFromDataImportPage requiredFields);

	Task<ImportCsvDataCommandResult> ImportAccompanyingFileDataCsv(
		MemoryStream memoryStreamFile,
		Guid userId);

	Task SendAccompanyingFileAbortMails(
		Guid accompanyingFileId,
		MailType mailType = MailType.AbortAccompanyingFileRequest,
		bool sendToSolidarBuilder = false,
		string comment = Labels.NoComment);

	Task<ReneeStringOperationResult> ValidateOrNotAccompanyingFileAbort(Guid accompanyingFileId, Guid userId, string commentOnAbort, string accompanyingFileReference, string userName, bool shouldAbort);

	Task<ReneeStringOperationResult> UpdateAccompanyingFileTargetInformations(UpdateAccompanyingFileTargetInformationsCommandInput input);

	Task<ReneeStringOperationResult> UpdateAccompanyingFileFacturationInformations(UpdateAccompanyingFileFacturationInformationsCommandInput input);

	Task<ReneeOperationResult<bool>> UpdateAccompanyingFilesForAnahGrantCheck(Guid userId, List<AccompanyingFileGrantDateResume> grantDates);
}