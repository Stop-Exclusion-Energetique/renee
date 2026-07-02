using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record UpdateAccompanyingFileMaximumValue(int? maximalNumberOfAccompanyingFileCreated, 
    int? maximalNumberOfAccompanyingFileToValidateFirstStage, 
    DateTime? accompanyingFileModificationDeadline, 
    DateTime? accompanyingFileAlertBannerStartDate, 
    DateTime? accompanyingFileAlertBannerEndDate, 
    string? accompanyingFileAlertBannerMessage): IRequest<ReneeOperationResult<bool>>;