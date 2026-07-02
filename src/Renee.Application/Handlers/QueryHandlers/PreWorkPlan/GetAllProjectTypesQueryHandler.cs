using MediatR;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.Interfaces;
using Renee.Application.Queries.PreWorkPlan;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.PreWorkPlan;

public class GetAllProjectTypesQueryHandler(
	IProjectTypeRepository preWorkPlanProjectTypeRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllProjectTypesQuery, ReneeOperationResult<IEnumerable<ProjectTypeDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<ProjectTypeDto>>> Handle(
		GetAllProjectTypesQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = (await preWorkPlanProjectTypeRepository.GetAllAsync()).Select(pt => new ProjectTypeDto(pt.Id, pt.Label));
			return ReneeOperationResult<IEnumerable<ProjectTypeDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<ProjectTypeDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}