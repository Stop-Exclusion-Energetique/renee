using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Administration;

public sealed class GetImpersonateForConnectedUserQuery(Guid? id) : IRequest<ReneeOperationResult<RegisteredUserDto?>>
{
	internal Guid? Id { get; } = id;
}