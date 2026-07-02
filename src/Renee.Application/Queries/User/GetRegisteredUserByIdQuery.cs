using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public sealed class GetRegisteredUserByIdQuery(Guid id) : IRequest<ReneeOperationResult<UserDto?>>
{
	public Guid Id { get; } = id;
}