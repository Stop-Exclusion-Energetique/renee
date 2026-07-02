using MediatR;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Coproperty;

public sealed class GetCopropertyProfileForIdentificationMilestoneQuery(Guid id, Guid userId, string userRole): IRequest<ReneeOperationResult<CopropertyProfileIdentificationDto?>>
{
    public Guid? CopropertyProfileId { get; } = id;
    public Guid UserId { get; } = userId;
    public string UserRole { get; } = userRole;
}
