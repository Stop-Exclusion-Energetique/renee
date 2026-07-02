using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput(
    Guid CopropertyProfileId,
    UpdateCopropertyWorkTracking UpdateCopropertyWorkTracking,
    Guid ConnectedUserId
    ) : IRequest<ReneeOperationResult<bool>>;

public record UpdateCopropertyWorkTracking(
    DateTime? CollectiveWorksStartDate,
    DateTime? PlannedEndDate,
    double? ProgressPercentage,
    DateTime? ActualCompletionDate,
    double? InvoiceTotalAmount,
    string? FollowUpComment
    );