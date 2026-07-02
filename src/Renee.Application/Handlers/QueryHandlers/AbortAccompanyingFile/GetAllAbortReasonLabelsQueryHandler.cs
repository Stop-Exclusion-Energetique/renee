using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.AbortReasonLabel;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AbortAccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AbortReasonLabels;

public class GetAllAbortReasonLabelsQueryHandler(
	IAbortReasonLabelRepository abortReasonLabelRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAllAbortReasonLabelsQuery, ReneeOperationResult<List<AbortReasonLabelDto>>>
{
	public override async Task<ReneeOperationResult<List<AbortReasonLabelDto>>> HandleQuery(GetAllAbortReasonLabelsQuery request)
	{
		try
		{
			var result = (await abortReasonLabelRepository.GetAllAsync())
				.Select(ar => new AbortReasonLabelDto(ar.Id, ar.Label)).ToList();
			return ReneeOperationResult<List<AbortReasonLabelDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<List<AbortReasonLabelDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}