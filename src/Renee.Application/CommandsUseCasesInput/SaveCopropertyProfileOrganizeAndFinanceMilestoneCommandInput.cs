using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
    Guid CopropertyProfileId,
    UpdateAids UpdateAids,
    List<UpdateWorkPackage> UpdateWorkPackages,
    Guid ConnectedUserId
    ) : IRequest<ReneeOperationResult<bool>>
{
    public List<WorkPackage> GetUpdatedWorkPackages() =>
        UpdateWorkPackages.Select(uwp => uwp.CreateUpdatedWorkPackage()).ToList();
}

public record UpdateAids(
    DateTime? DateOfAgVote,
    double? MprCoproAids,
    double? ComplementaryAids);

