using Renee.Application.Abstraction.Query;
using Renee.Application.Queries.Coproperty.QueryObjectResult;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Coproperty;

public class GetCopropertyProfileListQuery(string role, Guid? userId =  null)
    : IQuery<ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>>
{
    public string Role { get; set; } = role;
    public Guid? UserId { get; set;} = userId;
}
