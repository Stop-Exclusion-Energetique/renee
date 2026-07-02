using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty;

public class ValidateCopropertyProfileStageChangeCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    IUserRepository userRepository,
    ITelemetryService telemetryService) : IRequestHandler<ValidateCopropertyProfileStageChangeCommandInput, ReneeStringOperationResult>
{
    public async Task<ReneeStringOperationResult> Handle(ValidateCopropertyProfileStageChangeCommandInput command, CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileSynthesis(command.CopropertyProfileId);

            if (copropertyProfile == null)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

            var user = await userRepository.GetUserById(command.UserId);

            if (user is null)
                return ReneeStringOperationResult.Failure(Labels.Errors.UserNotFound);

            var currentAccompanyingFileStage = (AccompanyingFileStage)copropertyProfile.CopropertyMilestone;
            ChangeCopropertyProfileStageAndStatus(copropertyProfile, command.IsValidated, command.UserId, command.CommentOnValidation);
            var result = await copropertyProfileRepository.UpdateCopropertyProfileSynthesis(copropertyProfile);

            if (result == 0)
                return ReneeStringOperationResult.Failure(Labels.Errors.StageValidationErrorCopropertyProfileUpdateFailed);

            return ReneeStringOperationResult.Success(Labels.StageValidationCopropertySynthesisSuccess);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    private void ChangeCopropertyProfileStageAndStatus(CopropertyProfile copropertyProfile, bool isValidated, Guid userId, string? commentOnValidation)
    {
        var stage = (AccompanyingFileStage)copropertyProfile.CopropertyMilestone;
        if (isValidated)
        {
            switch (stage)
            {
                case AccompanyingFileStage.Identify:
                    copropertyProfile.CopropertyMilestone = (int)AccompanyingFileStage.OrganizingAndFinancing;
                    copropertyProfile.CopropertyStatus = (int)AccompanyingFileStatus.InProgress;
                    copropertyProfile.IdentifySynthesisValidationDate = DateTime.UtcNow;
                    copropertyProfile.IdentifyMilestoneValidatedBy = userId;
                    break;

                case AccompanyingFileStage.OrganizingAndFinancing:
                    copropertyProfile.CopropertyMilestone = (int)AccompanyingFileStage.RealisationAndFollowing;
                    copropertyProfile.CopropertyStatus = (int)AccompanyingFileStatus.InProgress;
                    copropertyProfile.OrganizeAndFinanceSynthesisValidationDate = DateTime.UtcNow;
                    copropertyProfile.OrganizeAndFinanceMilestoneValidatedBy = userId;
                    break;

                case AccompanyingFileStage.RealisationAndFollowing:
                    copropertyProfile.CopropertyMilestone = (int)AccompanyingFileStage.Finished;
                    copropertyProfile.CopropertyStatus = (int)AccompanyingFileStatus.Finished;
                    copropertyProfile.RealizeAndFollowSynthesisValidationDate = DateTime.UtcNow;
                    copropertyProfile.RealizeAndFollowMilestoneValidatedBy = userId;
                    copropertyProfile.ClosedBy = userId;
                    copropertyProfile.CloseDate = DateTime.UtcNow;
                    break;
            }
        }
        else
        {
            copropertyProfile.CopropertyStatus = (int)AccompanyingFileStatus.Rejected;
            copropertyProfile.ClosedBy = userId;
            copropertyProfile.CloseDate = DateTime.UtcNow;
            switch (stage)
            {
                case AccompanyingFileStage.Identify:
                    copropertyProfile.IdentifyMilestoneValidatedBy = userId;
                    break;

                case AccompanyingFileStage.OrganizingAndFinancing:
                    copropertyProfile.OrganizeAndFinanceMilestoneValidatedBy = userId;
                    break;

                case AccompanyingFileStage.RealisationAndFollowing:
                    copropertyProfile.RealizeAndFollowMilestoneValidatedBy = userId;
                    break;
            }

            if (!string.IsNullOrWhiteSpace(commentOnValidation))
            {
                copropertyProfile.RejectionCommentOnSynthesisValidation = commentOnValidation;
            }
        }
    }
}
