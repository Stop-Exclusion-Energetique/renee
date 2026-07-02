using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetAllAdminUsersMailQuery : IQuery<ReneeOperationResult<List<string?>>>
{
}