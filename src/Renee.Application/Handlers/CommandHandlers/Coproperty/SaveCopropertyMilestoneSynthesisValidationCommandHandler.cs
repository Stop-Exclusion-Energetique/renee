using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty;

public class SaveCopropertyMilestoneSynthesisValidationCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    IUserRepository userRepository,
    ITelemetryService telemetryService):
    IRequestHandler<SaveCopropertyMilestoneSynthesisValidationCommandInput, ReneeStringOperationResult>
{
    public async Task<ReneeStringOperationResult> Handle(SaveCopropertyMilestoneSynthesisValidationCommandInput command,
        CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileSynthesis(command.CopropertyProfileId);

            if (copropertyProfile == null)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

            if ((AccompanyingType?)copropertyProfile.AccompanyingType == AccompanyingType.Targeted &&
                copropertyProfile.CopropertySupportTeamNavigation.TerritorialBuilder is null)
                return ReneeStringOperationResult.Failure(Labels.Errors.NoTerritorialBuildersAffectedOnTargetedCopropertyProfileType);

            var user = await userRepository.GetUserById(command.UserId);
            
            if(user is null)
                return ReneeStringOperationResult.Failure(Labels.Errors.UserNotFound);

            var currentStage = (AccompanyingFileStage)copropertyProfile.CopropertyMilestone;

            UpdateCopropertySynthesis(copropertyProfile, command.Milestone, command.Status, command.UserId);

            var result = await copropertyProfileRepository.UpdateCopropertyProfileSynthesis(copropertyProfile);

            if (result <= 0)
                return ReneeStringOperationResult.Failure(Labels.Errors.StageValidationErrorCopropertyProfileUpdateFailed);

            return ReneeStringOperationResult.Success(Labels.StageValidationCopropertySynthesisSuccess);

        }catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
            return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    private static void UpdateCopropertySynthesis(
        CopropertyProfile copropertyProfile, 
        AccompanyingFileStage milestone,
        AccompanyingFileStatus status,
        Guid userId)
    {
        copropertyProfile.CopropertyMilestone = (int)milestone;
        copropertyProfile.CopropertyStatus = (int)status;

        switch ((AccompanyingFileStage) copropertyProfile.CopropertyMilestone)
        {
            case AccompanyingFileStage.Identify:
                copropertyProfile.IdentifySynthesisValidationDate = DateTime.Now;
                copropertyProfile.IdentifyMilestoneValidatedBy = userId;
                break;
            case AccompanyingFileStage.OrganizingAndFinancing:
                copropertyProfile.OrganizeAndFinanceSynthesisValidationDate = DateTime.Now;
                copropertyProfile.OrganizeAndFinanceMilestoneValidatedBy = userId;
                break;
            case AccompanyingFileStage.RealisationAndFollowing:
                copropertyProfile.RealizeAndFollowSynthesisValidationDate = DateTime.Now;
                copropertyProfile.RealizeAndFollowMilestoneValidatedBy = userId;
                break;
            default:
                break;
        }
    }
}
