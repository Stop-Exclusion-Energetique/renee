using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Application.Queries.Coproperty.QueryObjectResult;
using Renee.Domain.Entity;
using Renee.Domain;
using Renee.Domain.Repositories;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Handlers.QueryHandlers.Coproperty
{
    public class GetCopropertyProfileListQueryHandler(
        ICopropertyProfileRepository copropertyProfileRepository,
        IUserRepository userRepository,
        ITelemetryService telemetryService)
        : QueryHandler<GetCopropertyProfileListQuery, ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>>
    {
        public override async Task<ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>> HandleQuery(
            GetCopropertyProfileListQuery request)
        {
            try
            {
                if (request.UserId == null) 
                    return ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);

                var user = await userRepository.GetUserById((Guid)request.UserId);
                if (user is null)
                    return ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);

                if (user?.ReportingStructureId == null) 
                return ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);

                var copropertyProfiles = new List<CopropertyProfile>();

                copropertyProfiles = request.Role switch
                {
                    Constants.SolidarBuilderRole => await copropertyProfileRepository.GetAllCopropertyProfilesBySolidarBuilderReportingStructure((Guid)user.ReportingStructureId),
                    Constants.AssociationMemberRole or Constants.AdminRole => await copropertyProfileRepository.GetAllCopropertyProfilesForAssociationMemberDisplay(),
                    Constants.DiffuseCoordinatorRole or Constants.TargetedCoordinatorRole => await copropertyProfileRepository.GetAllCopropertyProfilesForCoordinators(),
                    Constants.TerritorialBuilderRole => await copropertyProfileRepository.GetAllCopropertyProfilesForTerritorialBuilder(request.UserId.Value),
                    Constants.StructuralReferentRole => await copropertyProfileRepository.GetAllCopropertyProfilesForStructuralReferent((Guid)user.ReportingStructureId),
                    _ => copropertyProfiles
                };

                var copropertyProfilesResumes = copropertyProfiles?.Select(cp => new CopropertyProfileResume
                {
                    Id = cp.Id,
                    Reference = cp.CopropertyReference,
                    Address = cp.CopropertyHousingNavigation.HousingAddressNavigation.Label,
                    Stage = (AccompanyingFileStage) cp.CopropertyMilestone,
                    Status = (AccompanyingFileStatus) cp.CopropertyStatus,
                    CreateDateUtc = cp.CreationDate ?? DateTime.UtcNow,
                    LastUpdatedDateUtc = cp.LastUpdateDate ?? DateTime.UtcNow,
                    SolidarBuilder = cp.CopropertySupportTeamNavigation.SolidarBuilder,
                    SecondSolidarBuilder = cp.CopropertySupportTeamNavigation.SecondSolidarBuilder,
                    ThirdSolidarBuilder = cp.CopropertySupportTeamNavigation.ThirdSolidarBuilder,
                    ReportingStructureId = cp.CopropertySupportTeamNavigation.SolidarBuilderNavigation?.ReportingStructureId,
                    SecondTerritorialBuilder = cp.CopropertySupportTeamNavigation.SecondTerritorialBuilder,
                    TerritorialBuilder = cp.CopropertySupportTeamNavigation.TerritorialBuilder,
                    DiffuseCoordinatorId = cp.CopropertySupportTeamNavigation.DiffuseCoordinator,
                    TargetCoordinatorId = cp.CopropertySupportTeamNavigation.TargetCoordinator,
                    TerritoryId = cp.CopropertyTerritory
                }).OrderBy(cp => cp.CreateDateUtc).ToList();

                var result = new GetCopropertyProfileListQueryObjectResult { CopropertyProfiles = copropertyProfilesResumes ?? [] };
                return ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>.Success(result);
            }
            catch (Exception ex)
            {
                await telemetryService.TrackExceptionAsync(ex, CancellationToken.None);
                return ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>.Failure(Labels.Errors.ErrorWhileLoadingCopropertyProfile);
            }
        }
    }
}
