using MediatR;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Coproperty;

public class GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(Guid id, Guid userId, string userRole) : IRequest<ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>>
{
    public Guid? CopropertyProfileId { get; } = id;
    public Guid UserId { get; } = userId;
    public string UserRole { get; } = userRole;
}
