using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ExcelExport;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public record GetCompleteAccompanyingFileForBillingLogAndAdministrationExcelExportQuery
    : IQuery<ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>>
{
    public Guid UserId { get; set; }
    public string UserRole { get; set; } = string.Empty;
}
