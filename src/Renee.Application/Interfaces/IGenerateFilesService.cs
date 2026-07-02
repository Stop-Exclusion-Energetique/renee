using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IGenerateFilesService
{
	Task<ReneeOperationResult<string>> GenerateAnahSynthesis(string path, string accompanyingFileReference, Guid connectedUserId);
	Task<ReneeOperationResult<MemoryStream>> ExportAccompanyingFileForExcel(Guid userId, string userRole);
	Task<ReneeOperationResult<string>> GenerateWorkCertificate(string path, string accompanyingFileReference, Guid connectedUserId);
	Task<ReneeOperationResult<MemoryStream>> ExportAccompanyingFileBillingInformationAndAdministrativeData(Guid userId, string userRole);
}