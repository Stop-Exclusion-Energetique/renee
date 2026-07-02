using Renee.Domain.Documents;
using Renee.Domain.Enums;

namespace Renee.Application.Interfaces;

public interface IFileService
{
	Task<(MemoryStream?, string)> DownloadFileForSynthesisAsync(string blobName);
	Task<Document?> DownloadDocumentForAccompanyingFileMenuAsync(string blobName, AccompanyingFileStage? stage = null);
	Task UploadFileAsync(string blobName, string fileName, MemoryStream memoryStream, Guid? userId = null);
	Task DeleteFileAsync(string blobName);
	Task<List<Document>> GetAllDocumentsAsync(string accompanyingFileReference);
}