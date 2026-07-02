using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.WorkTypeProjectType;
using Renee.Application.Interfaces;
using Renee.Application.Queries.WorkTypeProjectType;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.WorkTypeProjectType;

public class GetAllWorkTypesProjectTypesQueryHandler(
	IWorkTypeProjectTypeRepository workTypeProjectTypeRepository,
	ITelemetryService telemetryService) : QueryHandler<GetAllWorkTypesProjectTypesQuery, ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>>
{
	public override async Task<ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>> HandleQuery(GetAllWorkTypesProjectTypesQuery request)
	{
		try
		{
			var result = (await workTypeProjectTypeRepository.GetAllAsync()).
				Select(wtpt => new WorkTypeProjectTypeDto(wtpt.WorkTypeId, wtpt.ProjectTypeId));
			return ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<IEnumerable<WorkTypeProjectTypeDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}