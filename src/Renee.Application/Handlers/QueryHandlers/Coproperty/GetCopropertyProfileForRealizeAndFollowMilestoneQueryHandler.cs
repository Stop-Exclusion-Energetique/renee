using MediatR;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Coproperty;

public class GetCopropertyProfileForRealizeAndFollowMilestoneQueryHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    :IRequestHandler<GetCopropertyProfileForRealizeAndFollowMilestoneQuery, ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>>
{
    public async Task<ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>> Handle(
        GetCopropertyProfileForRealizeAndFollowMilestoneQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile =
                await copropertyProfileRepository.GetCopropertyProfileForRealizeAndFollowMilestoneAsync((Guid)request.CopropertyProfileId!);

            if (copropertyProfile == null)
                return ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>.Failure(Labels.Errors.ErrorWhileLoadingCopropertyProfile);

            if (IsUserInSupportTeam(copropertyProfile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole))
                return ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>.Success(ToCopropertyProfileForRealizeAndFollow(copropertyProfile));

            return ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
    
    private static bool IsUserInSupportTeam(CopropertyProfile copropertyProfile, Guid userId)
        => userId == copropertyProfile.CopropertySupportTeamNavigation?.TerritorialBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SecondTerritorialBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SolidarBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SecondSolidarBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.ThirdSolidarBuilder;

    private static bool IsUserACoordinatorOrAdmin(string userRole)
        => userRole.Equals(Constants.DiffuseCoordinatorRole) ||
            userRole.Equals(Constants.TargetedCoordinatorRole) ||
            userRole.Equals(Constants.AdminRole);

    private static CopropertyProfileRealizeAndFollowDto ToCopropertyProfileForRealizeAndFollow(CopropertyProfile copropertyProfile)
        => new()
        {
            Id = copropertyProfile.Id,
            Reference = copropertyProfile.CopropertyReference,
            Stage = (AccompanyingFileStage)copropertyProfile.CopropertyMilestone,
            Status = (AccompanyingFileStatus)copropertyProfile.CopropertyStatus,
            TerritorialBuilderId = copropertyProfile.CopropertySupportTeamNavigation.TerritorialBuilder,
            CreationDatetimeUtc = copropertyProfile.CreationDate,
            LastUpdateDatetimeUtc = copropertyProfile.LastUpdateDate,
            SolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SolidarBuilder,
            SecondSolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SecondSolidarBuilder,
            ThirdSolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.ThirdSolidarBuilder,
            DiffuseCoordinatorId = copropertyProfile.CopropertySupportTeamNavigation.DiffuseCoordinator,
            SecondTerritorialBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SecondSolidarBuilder,
            TargetedCoordinatorId = copropertyProfile.CopropertySupportTeamNavigation.TargetCoordinator,
            IsInTzeeProgram = copropertyProfile.ZeroEnergyExclusionTerritoriesProgram,
            CollectiveWorksStartDate = copropertyProfile.CopropertyWorkTrackingNavigation.CollectiveWorksStartDate,
            ActualCompletionDate = copropertyProfile.CopropertyWorkTrackingNavigation.ActualCompletionDate,
            ProgressPercentage = copropertyProfile.CopropertyWorkTrackingNavigation.ProgressPercentage,
            FollowUpComment = copropertyProfile.CopropertyWorkTrackingNavigation.FollowUpComment,
            InvoiceTotalAmount = copropertyProfile.CopropertyWorkTrackingNavigation.InvoiceTotalAmount,
            PlannedEndDate = copropertyProfile.CopropertyWorkTrackingNavigation.PlannedEndDate
        };
}
