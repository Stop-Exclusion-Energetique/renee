using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty.RealizeAndFollowStageUseCase;

public class SaveCopropertyProfileRealizeAndFollowMilestoneCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput command, CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync(command.CopropertyProfileId);

            if (copropertyProfile == null) return ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

            copropertyProfile.UpdatedBy = command.ConnectedUserId;
            copropertyProfile.LastUpdateDate = DateTime.UtcNow;

            UpdateCopropertyProfileWorkTracking(copropertyProfile.CopropertyWorkTrackingNavigation, command.UpdateCopropertyWorkTracking);

            var numberItemsChanged = await copropertyProfileRepository.UpdateCopropertyProfileForForRealizeAndFollowMilestoneAsync(copropertyProfile);

            return numberItemsChanged > -1
                ? ReneeOperationResult<bool>.Success(true, Labels.SaveMilestoneSuccess)
                : ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileSavingMilestone);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    public static void UpdateCopropertyProfileWorkTracking(CopropertyWorkTracking copropertyWorkTracking, UpdateCopropertyWorkTracking updateCopropertyWorkTracking)
    {
        copropertyWorkTracking.CollectiveWorksStartDate = updateCopropertyWorkTracking.CollectiveWorksStartDate;
        copropertyWorkTracking.PlannedEndDate = updateCopropertyWorkTracking.PlannedEndDate;
        copropertyWorkTracking.ProgressPercentage = updateCopropertyWorkTracking.ProgressPercentage;
        copropertyWorkTracking.FollowUpComment = updateCopropertyWorkTracking.FollowUpComment;
        copropertyWorkTracking.InvoiceTotalAmount = updateCopropertyWorkTracking.InvoiceTotalAmount;
        copropertyWorkTracking.ActualCompletionDate = updateCopropertyWorkTracking.ActualCompletionDate;

    }
}
