using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Application.Interfaces;
using Renee.Application.Queries.CGUVersion;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.CguVersion;

public class GetAllCguVersionQueryHandler(
	ICguVersionRepository cguVersionRepository,
	ITelemetryService telemetryService) 
	: IRequestHandler<GetAllCguVersionsQuery, ReneeOperationResult<IEnumerable<CguVersionDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<CguVersionDto>>> Handle(
		GetAllCguVersionsQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var cgus = await cguVersionRepository.GetAllVersionsAsync();
			var result = cgus.Select(cgu => new CguVersionDto { Id = cgu.Id, Label = cgu.Label, Version = cgu.Version, CreatedDate = cgu.CreatedAt });
			return ReneeOperationResult<IEnumerable<CguVersionDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<CguVersionDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
