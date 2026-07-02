using MediatR;
using Renee.Application.DTOs.WorkTypesLabels;
using Renee.Application.Interfaces;
using Renee.Application.Queries.WorkTypesLabels;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class WorkTypesLabelsService(IMediator mediator) : IWorkTypesLabelsService
{
	public async Task<ReneeOperationResult<IEnumerable<WorkTypesLabelsDto>>> GetAllWorkTypesLabels() =>
		await mediator.Send(new GetAllWorkTypesLabelsQuery());
}