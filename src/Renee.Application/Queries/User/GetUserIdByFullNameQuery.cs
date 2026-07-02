using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public sealed class GetUserIdByFullNameQuery(string fullname) : IRequest<ReneeOperationResult<Guid?>>
{
	public string Fullname { get; } = fullname;
}