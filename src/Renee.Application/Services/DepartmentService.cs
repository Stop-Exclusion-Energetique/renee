using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Department;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class DepartmentService(IMediator mediator) : IDepartmentService
{
	public async Task<ReneeOperationResult<IEnumerable<DepartmentDto>>> GetAllAsync() => await mediator.Send(new GetAllDepartmentsQuery());
}