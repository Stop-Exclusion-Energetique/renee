using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension.ToRepository;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty.OrganizeAndFinanceStageUseCase;

public class SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput command, CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(command.CopropertyProfileId);

            if (copropertyProfile == null) return ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

            copropertyProfile.UpdatedBy = command.ConnectedUserId;
            copropertyProfile.LastUpdateDate = DateTime.UtcNow;

            UpdateCopropertyAids(copropertyProfile.CopropertyWorkFinanceNavigation, command.UpdateAids);

            var (workPackagesToAdd, workPackagesToUpdate, workPackagesToRemove) =
                UpdateCopropertyWorkFinanceWorkPackages(copropertyProfile.CopropertyWorkFinanceNavigation, command.GetUpdatedWorkPackages());

            var updatedWorkPackages = new WorkPackageUpdate(workPackagesToAdd, workPackagesToRemove);

            var workPackageToUpdate = new List<WorkPackageToUpdate>();
            foreach (var wp in workPackagesToUpdate)
            {
                var matchingWorkPackage = copropertyProfile.CopropertyWorkFinanceNavigation
                    .WorkPackages.FirstOrDefault(mwp => mwp.Id == wp.Id);
                if (matchingWorkPackage != null)
                {
                    var (toAdd, toUpdate, toDelete) = UpdateCopropertyWorkTypeCosts(wp, matchingWorkPackage);
                    workPackageToUpdate.Add(new WorkPackageToUpdate(wp, toAdd, toUpdate, toDelete));
                }
            }

            var numberItemsChanged = await copropertyProfileRepository
                .UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
                copropertyProfile,
                workPackagesToAdd,
                workPackagesToRemove,
                workPackageToUpdate
                );

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

    public static void UpdateCopropertyAids(CopropertyWorkFinance copropertyWorkFinance, UpdateAids updateAids)
    {
        copropertyWorkFinance.MprCoproAids = updateAids.MprCoproAids;
        copropertyWorkFinance.ComplementaryAids = updateAids.ComplementaryAids;
        copropertyWorkFinance.DateOfAgVote = updateAids.DateOfAgVote;
    }

    public static (List<WorkPackage>, List<WorkPackage>, List<WorkPackage>) UpdateCopropertyWorkFinanceWorkPackages(
        CopropertyWorkFinance copropertyWorkFinance,
        List<WorkPackage> updatedWorkPackages)
    {
        var itemToRemove = copropertyWorkFinance.WorkPackages.Where(wp => !updatedWorkPackages.Exists(uwp => uwp.Id == wp.Id))
            .ToList();

        foreach (var item in itemToRemove) item.CopropertyWorkFinance = copropertyWorkFinance.Id;

        var itemToAdd = updatedWorkPackages.Where(uwp => copropertyWorkFinance.WorkPackages.All(wp => wp.Id != uwp.Id)).ToList();

        foreach (var item in itemToAdd) item.CopropertyWorkFinance = copropertyWorkFinance.Id;

        var itemToUpdate = updatedWorkPackages.Where(uwp => copropertyWorkFinance.WorkPackages.Any(wp => wp.Id == uwp.Id))
            .ToList();

        foreach (var item in itemToUpdate) item.CopropertyWorkFinance = copropertyWorkFinance.Id;

        return (itemToAdd, itemToUpdate, itemToRemove);
    }

    public static (List<WorkPackageWorkTypeCost>, List<WorkPackageWorkTypeCost>, List<WorkPackageWorkTypeCost>)
        UpdateCopropertyWorkTypeCosts(WorkPackage updatedWorkPackage, WorkPackage oldWorkPackage)
    {
        var itemToRemove = oldWorkPackage.WorkPackageWorkTypeCosts
            .Where(wtc => !updatedWorkPackage.WorkPackageWorkTypeCosts.ToList().Exists(uwtc => uwtc.WorkType == wtc.WorkType)).ToList();

        foreach (var item in itemToRemove) item.WorkPackage = updatedWorkPackage.Id;

        var itemToAdd = updatedWorkPackage.WorkPackageWorkTypeCosts
            .Where(uwp => oldWorkPackage.WorkPackageWorkTypeCosts.All(wp => wp.WorkType != uwp.WorkType)).ToList();

        foreach (var item in itemToAdd) item.WorkPackage = updatedWorkPackage.Id;

        var itemToUpdate = updatedWorkPackage.WorkPackageWorkTypeCosts
            .Where(uwp => oldWorkPackage.WorkPackageWorkTypeCosts.Any(wp => wp.WorkType == uwp.WorkType)).ToList();

        foreach (var item in itemToUpdate) item.WorkPackage = updatedWorkPackage.Id;

        return (itemToAdd, itemToUpdate, itemToRemove);
    }

}
