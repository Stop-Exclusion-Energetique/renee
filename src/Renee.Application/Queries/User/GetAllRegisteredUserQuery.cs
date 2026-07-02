using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.User;

public class GetAllRegisteredUserQuery : IQuery<ReneeOperationResult<List<GetAllRegisteredQyeryObjectResult>>>
{}

public record GetAllRegisteredQyeryObjectResult(Guid UserId, string FirstName, string LastName, Guid UserRoleId, string RoleName, Guid? ReportingStructureId, string? ReportingStructureName)
{}