using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetUserByEmailForDuplicateVerificationQuery(string email) : IRequest<ReneeOperationResult<bool?>>
{
	public string? Email { get; } = email;
}