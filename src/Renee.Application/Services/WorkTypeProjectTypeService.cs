using MediatR;
using Renee.Application.DTOs.WorkTypeProjectType;
using Renee.Application.Interfaces;
using Renee.Application.Queries.WorkTypeProjectType;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class WorkTypeProjectTypeService(IMediator mediator) : IWorkTypeProjectTypeService
{
	public async Task<ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>> GetAllWorkTypeProjectTypes() =>
		await mediator.Send(new GetAllWorkTypesProjectTypesQuery());
}