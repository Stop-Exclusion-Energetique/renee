using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class UpdateAccompanyingFileTargetInformationsCommandHandler(IAccompanyingFileRepository accompanyingFileRepository,
    IUserRepository userRepository,
    ITelemetryService telemetryService) : IRequestHandler<UpdateAccompanyingFileTargetInformationsCommandInput, ReneeStringOperationResult>
{
    public async Task<ReneeStringOperationResult> Handle(UpdateAccompanyingFileTargetInformationsCommandInput request, CancellationToken cancellationToken)
    {
        try
        {
            var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileSupportTeam(request.AccompanyingFileId);

            if (accompanyingFile is null) return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingTargetInformation);

            accompanyingFile.AccompanyingFileHousingNavigation.GeographicAreaTypology = (int)request.GeographicalHousingAreaTypology;
            accompanyingFile.AccompanyingFileTerritory = request.AccompanyingTerritory;
            if (request.AccompanyingType is not null && accompanyingFile.AccompanyingType != (int)request.AccompanyingType)
            {
                switch (request.AccompanyingType)
                {
                    case Domain.Enums.AccompanyingType.Targeted:
                        {
                            accompanyingFile.AccompanyingType = (int)Domain.Enums.AccompanyingType.Targeted;
                            var result = await SwitchingFromDiffuseToTargeted(accompanyingFile, request.AccompanyingTerritory!.Value);
                            if (!result.IsSuccess) return result;
                            break;
                        }
                    case Domain.Enums.AccompanyingType.Diffuse:
                        {
                            accompanyingFile.AccompanyingType = (int)Domain.Enums.AccompanyingType.Diffuse;
                            var result = await SwitchingFromTargetToDiffuse(accompanyingFile);
                            if (!result.IsSuccess) return result;
                            break;
                        }
                }
            }
            var updateResult = await accompanyingFileRepository.UpdateAccompanyingFileTargetInformations(accompanyingFile);
            if (!updateResult) return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingTargetInformation);

            return ReneeStringOperationResult.Success(Labels.UpdateOfAccompanyingFileTargetInformationSuccess);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
    
    private async System.Threading.Tasks.Task<ReneeStringOperationResult> SwitchingFromDiffuseToTargeted(Domain.Entity.AccompanyingFile accompanyingFile, Guid territoryId)
    {
        var targetedCoordinators = (await userRepository.GetUsersByRole(Domain.Constants.TargetedCoordinatorRole, false)).FirstOrDefault();
        var territorialBuilders = (await userRepository.GetUsersByRole(Domain.Constants.TerritorialBuilderRole, false))
            .FirstOrDefault(u => u.TerritoryId == territoryId &&
                u.ReportingStructure == accompanyingFile.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructure
            );

        if (targetedCoordinators is null) return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingTargetInformation);
        if (territorialBuilders is null) return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingTargetInformation);

        accompanyingFile.AccompanyingFileSupportTeamNavigation.TargetCoordinator = targetedCoordinators.Id;
        accompanyingFile.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator = null;
        accompanyingFile.AccompanyingFileSupportTeamNavigation.TerritorialBuilder = territorialBuilders?.Id;

                return ReneeStringOperationResult.Success(Labels.UpdateSupportTeamSuccess);
            }

    private async System.Threading.Tasks.Task<ReneeStringOperationResult> SwitchingFromTargetToDiffuse(Domain.Entity.AccompanyingFile accompanyingFile)
    {
        var diffuseCoordinators = (await userRepository.GetUsersByRole(Domain.Constants.DiffuseCoordinatorRole, false)).FirstOrDefault();

        if (diffuseCoordinators is null) return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingTargetInformation);

        accompanyingFile.AccompanyingFileSupportTeamNavigation.DiffuseCoordinator = diffuseCoordinators.Id;
        accompanyingFile.AccompanyingFileSupportTeamNavigation.TargetCoordinator = null;
        accompanyingFile.AccompanyingFileSupportTeamNavigation.TerritorialBuilder = null;
        accompanyingFile.AccompanyingFileSupportTeamNavigation.SecondTerritorialBuilder = null;

                return ReneeStringOperationResult.Success(Labels.UpdateSupportTeamSuccess);
            }
}
