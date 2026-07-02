using MediatR;
using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.UserCredential;

public sealed class GetRegisteredUserOnCredentialsQuery(string email) : IRequest<ReneeOperationResult<RegisteredUserDto?>>
{
	internal string Email { get; } = email;
}