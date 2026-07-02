using Renee.Application.Queries.Administration;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces.Administration;

public interface IAdministrationConstantsManagementService
{
    Task<ReneeOperationResult<bool>> UpdateAccompanyingFileMaximumValue(int? maximalNumberOfAccompanyingFileCreated, 
        int? maximalNumberOfAccompanyingFileToValidateFirstStage, 
        DateTime? accompanyingFileModificationDeadline, 
        DateTime? accompanyingFileAlertBannerStartDate,
        DateTime? accompanyingFileAlertBannerEndDate, 
        string? accompanyingFileAlertBannerMessage);

    Task<ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>> GetBannerDisplayStatusAsync();
}