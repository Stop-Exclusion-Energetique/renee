using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public sealed class GetRegisteredUserAndLastCguQuery(Guid id) : IRequest<ReneeOperationResult<UserVersionCguDto>>
{
    public Guid Id { get; } = id;
}
