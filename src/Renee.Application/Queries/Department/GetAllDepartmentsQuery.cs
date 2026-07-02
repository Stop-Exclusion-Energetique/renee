using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Department;

public sealed class GetAllDepartmentsQuery : IRequest<ReneeOperationResult<IEnumerable<DepartmentDto>>>;