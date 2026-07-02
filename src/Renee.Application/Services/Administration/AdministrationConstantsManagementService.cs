using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.Queries.Administration;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces.Administration;

public class AdministrationConstantsManagementService(IMediator mediator) : IAdministrationConstantsManagementService
{
    public Task<ReneeOperationResult<GetBannerDisplayDatesDisplayQueryObjectResult>> GetBannerDisplayStatusAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<ReneeOperationResult<bool>> UpdateAccompanyingFileMaximumValue(int? maximalNumberOfAccompanyingFileCreated, 
        int? maximalNumberOfAccompanyingFileToValidateFirstStage, 
        DateTime? accompanyingFileModificationDeadline, 
        DateTime? accompanyingFileAlertBannerStartDate, 
        DateTime? accompanyingFileAlertBannerEndDate, 
        string? accompanyingFileAlertBannerMessage)
        => await mediator.Send(new UpdateAccompanyingFileMaximumValue(maximalNumberOfAccompanyingFileCreated,
            maximalNumberOfAccompanyingFileToValidateFirstStage, 
            accompanyingFileModificationDeadline, 
            accompanyingFileAlertBannerStartDate, 
            accompanyingFileAlertBannerEndDate, 
            accompanyingFileAlertBannerMessage));
    
    
}