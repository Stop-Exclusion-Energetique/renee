using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ExcelExport;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetCompleteAccompanyingFileForExcelExportQuery : IQuery<ReneeOperationResult<GetCompleteAccompanyingFileForExcelExportQueryObjectResult>>
{
	public Guid UserId { get; set; }
	public string UserRole { get; set; } = string.Empty;
}

public record GetCompleteAccompanyingFileForExcelExportQueryObjectResult(List<AccompanyingFileExcelExportDto> AccompanyingFileExcelExportDto);