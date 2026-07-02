using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IAccompanyingFileRepository
{
	Task<int> CountAllActiveAndInTzeeProgramAccompanyingFile();

	Task<int> CountAllActiveAndInTzeeProgramAccompanyingFileForMilestone1();

	Task<int> AddAccompanyingFile(AccompanyingFile accompanyingFile);

	Task<int> AddAccompanyingFiles(List<AccompanyingFile> accompanyingFiles);

    Task<int> DeleteAccompanyingFile(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileByIdForIdentificationMilestoneAsync(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileDataForAnahSynthesisPdf(string accompanyingFileReference);

	Task<AccompanyingFile?> GetAccompanyingFileDataForWorkCertificatePdf(string accompanyingFileReference);

	Task<AccompanyingFile?> GetAccompanyingFileForAirtable(string accompanyingFileReference);

	Task<AccompanyingFile> GetBaseAccompanyingFile(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileForStageValidation(Guid accompanyingFileId);

	Task<List<AccompanyingFile>> GetAccompanyingFileStatisticsForAssociationMember(
		DateTime? fromDate,
		DateTime? toDate,
		List<Guid?>? reportingStructures = null);

	Task<AccompanyingFile?> GetAccompanyingFileSupportTeam(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileSynthesis(Guid accompanyingFileId);

	Task<AccompanyingFile?> GetAccompanyingFileToRetrieveSupportTeam(Guid accompanyingFileId);

	Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFileForAssociationMembersAndAdmins(
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize);

	Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForSolidarBuilders(
		Guid userId,
		Guid reportingStructureId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize);

	Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForCoordinators(
		Guid userId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize);

	Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForTerritorialBuilders(
		Guid userId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize);

	Task<(List<Guid> ReportingStructureIds, List<Guid> SolidarBuilderIds)> GetTerritorialBuilderFilterData(
		Guid userId);

	Task<(List<AccompanyingFileResumeView>, int)> GetAllAccompanyingFilesForStructuralReferents(
		Guid reportingStructureId,
		AccompanyingFilesListFilterOptions filterOptions,
		int numberOfItemsToSkip,
		int pageSize);

	Task<List<AccompanyingFile>> GetAllAwaitingAnahResponseAsync(Guid userId);

	Task<bool> HasAwaitingAnahResponseAsync(Guid userId);

	Task<(List<AccompanyingFile>, AverageAccompanyingDuration)> GetStatisticsForCoordinatorsIndex(
		Guid userId,
		DateTime? fromDate,
		DateTime? toDate,
		bool shouldFilterOnUserAllAccompanyingFile,
		AccompanyingFilesStatisticsFilterOptions filterOptions,
		bool IsTargetedCoordinator);

	Task<List<AccompanyingFile>> GetStatisticsForSolidarBuilderIndex(Guid userId, DateTime? fromDate, DateTime? toDate);

	Task<List<AccompanyingFile>> GetStatisticsForSolidarBuilderReportingStructure(
		Guid reportingStructureId,
		DateTime? fromDate,
		DateTime? toDate);

	Task<List<AccompanyingFile>> GetStatisticsForTerritorialBuilderIndex(
		Guid userId,
		DateTime? fromDate,
		DateTime? toDate,
		bool shouldFilterOnUserAllAccompanyingFile,
		AccompanyingFilesStatisticsFilterOptions filterOptions);

	Task<List<AccompanyingFile>> GetStatisticsForStructuralReferentIndex(
		Guid reportingStructureId,
		DateTime? fromDate,
		DateTime? toDate);

	Task<List<AccompanyingFile>> GetAccompanyingFilesByExternalReferences(List<string?> references);

	Task<AccompanyingFile?> GetAccompanyingFileForCsvImport(Guid accompanyingFileId);

    Task<int> UpdateAccompanyingFileByIdForIdentificationMilestoneAsync(
		AccompanyingFile accompanyingFile,
		SaveAndSubmitIdentificationMilestoneData data);

	Task<int> UpdateBaseAccompanyingFile(AccompanyingFile accompanyingFile);

	Task<int?> UpdateAccompanyingFileByIdForOrganizeAndFinanceMilestoneAsync(
		AccompanyingFile accompanyingFile,
		SaveAndSubmitOrganizeAndFinanceMilestoneData data);

	Task<int> UpdateAccompanyingFileByIdForRealiseAndFollowMilestoneAsync(
		AccompanyingFile accompanyingFile,
		SaveAndSubmitRealizeAndFollowMilestoneData data);

	Task<int> UpdateAccompanyingFilesCopropertyProfileId(List<Guid> accompanyingFileIds, Guid copropertyProfileId);

	Task<int> UpdateAccompanyingFileImportedWithCsvAsync(
		AccompanyingFile accompanyingFile,
		SaveAndSubmitIdentificationMilestoneData identificationMilestoneData,
		SaveAndSubmitOrganizeAndFinanceMilestoneData organizeAndFinanceMilestoneData,
		SaveAndSubmitRealizeAndFollowMilestoneData realizeAndFollowMilestoneData);

    Task<int> UpdateAccompanyingFileSynthesis(
		AccompanyingFile accompanyingFile,
		List<Invoice>? createdInvoices = null);

	Task<int> UpdateAccompanyingSupportTeam(
		AccompanyingFile accompanyingFile,
		User entity,
		string supportTeamMemberRole);

	Task<int> UpdateAccompanyingFilesForAnahGrantCheck(List<AccompanyingFile> accompanyingFiles);

	Task<List<AccompanyingFile>> GetAllAccompanyingFileForExcelExport();

	Task<List<AccompanyingFile>> GetSolidarOrTerritorialBuilderAccompanyingFileForExcelExport(Guid userId);

	Task<List<AccompanyingFile>> GetStructuralReferentAccompanyingFileForExcelExport(Guid nationalStructureId);

	Task<bool> UpdateAccompanyingFileTargetInformations(AccompanyingFile accompanyingFile);

	Task<AccompanyingFile?> GetAccompanyingFileWithBillingLog(Guid accompanyingFileId);

	Task<bool> UpdateAccompanyingFileBillingLog(AccompanyingFile accompanyingFile);

	Task<List<AccompanyingFile>> GetAllAccompanyingFileForBillingLogAndAdministrationExcelExport();
	Task<List<AccompanyingFile>> GetSolidarOrTerritorialBuilderAccompanyingFileForBillingLogAndAdministrationExcelExport(Guid userId);
	Task<List<AccompanyingFile>> GetStructuralReferentAccompanyingFileForBillingLogAndAdministrationExcelExport(Guid nationalStructureId);
}
