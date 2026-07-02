using MediatR;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.WorkPackage;
using Renee.Application.DTOs.WorkPackageWorkTypeCost;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Coproperty;

public class GetCopropertyProfileForOrganizeAndFinanceMilestoneQueryHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery, ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>>
{
    public async Task<ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>> Handle(
        GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile =
                await copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync((Guid)request.CopropertyProfileId!);

            if (copropertyProfile is null)
                return ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>.Failure(Labels.Errors.ErrorWhileLoadingCopropertyProfile);

            if (IsUserInSupportTeam(copropertyProfile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole))
                return ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>.Success(ToCopropertyProfileForOrganizeAndFinance(copropertyProfile));

            return ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
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

    private static CopropertyProfileOrganizeAndFinanceDto ToCopropertyProfileForOrganizeAndFinance(CopropertyProfile copropertyProfile)
    {
        return new CopropertyProfileOrganizeAndFinanceDto
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
            DateOfAgVote = copropertyProfile.CopropertyWorkFinanceNavigation.DateOfAgVote,
            MprCoproAids = copropertyProfile.CopropertyWorkFinanceNavigation.MprCoproAids,
            ComplementaryAids = copropertyProfile.CopropertyWorkFinanceNavigation.ComplementaryAids,
            WorkPackages = copropertyProfile.CopropertyWorkFinanceNavigation.WorkPackages.Select(
                        wp => new WorkPackageDto(
                            wp.Id,
                            wp.EnergeticsEffectAfterWorks,
                            wp.WorkPackageWorkTypeCosts.Select(
                                    wtc => new WorkPackageWorkTypeCostDto(wtc.WorkType, wtc.Cost, wtc.Description))
                                .ToList())).ToList(),
        };
    }

}
