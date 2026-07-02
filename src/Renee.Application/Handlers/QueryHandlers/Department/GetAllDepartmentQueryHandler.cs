using MediatR;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Department;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Department;

public class GetAllDepartmentsQueryHandler(
	IDepartmentRepository departmentRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllDepartmentsQuery, ReneeOperationResult<IEnumerable<DepartmentDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<DepartmentDto>>> Handle(
		GetAllDepartmentsQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var departments = await departmentRepository.GetAllAsync();
			var result = departments.Select(c => new DepartmentDto { Id = c.Id, Name = c.Name, Number = c.Number });
			return ReneeOperationResult<IEnumerable<DepartmentDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<DepartmentDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}