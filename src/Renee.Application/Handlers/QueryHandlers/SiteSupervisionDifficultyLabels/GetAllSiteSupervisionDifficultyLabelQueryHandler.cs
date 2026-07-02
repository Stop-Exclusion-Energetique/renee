using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.SiteSupervision;
using Renee.Application.Interfaces;
using Renee.Application.Queries.SiteSupervisionDifficultyLabels;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.SiteSupervisionDifficultyLabels;

public class GetAllSiteSupervisionDifficultyLabelQueryHandler(
	ISiteSupervisionDifficultyLabelRepository siteSupervisionDifficultyLabelRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAllSiteSupervisionDifficultyLabelQuery, ReneeOperationResult<IEnumerable<SiteSupervisionDifficultyDto>>>
{
	public override async Task<ReneeOperationResult<IEnumerable<SiteSupervisionDifficultyDto>>> HandleQuery(
		GetAllSiteSupervisionDifficultyLabelQuery request)
	{
		try
		{
			var result = (await siteSupervisionDifficultyLabelRepository.GetAllAsync())
				.Select(d => new SiteSupervisionDifficultyDto(d.Id, d.Label));
			return ReneeOperationResult<IEnumerable<SiteSupervisionDifficultyDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return ReneeOperationResult<IEnumerable<SiteSupervisionDifficultyDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}