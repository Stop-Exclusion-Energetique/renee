using MediatR;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.Interfaces;
using Renee.Application.Queries.PreWorkPlan;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class ProjectTypeService(IMediator mediator) : IProjectTypeService
{
	public async Task<ReneeOperationResult<IEnumerable<ProjectTypeDto>>> GetAllProjectTypes() =>
		await mediator.Send(new GetAllProjectTypesQuery());
}