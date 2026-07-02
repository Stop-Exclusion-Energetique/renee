using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.CGUVersion;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.CGUVersion;

public class CheckDuplicatedCguVersionByVersionQueryHandler(
	ICguVersionRepository cguVersionRepository,
	ITelemetryService telemetryService) 
	: IRequestHandler<CheckDuplicatedCguVersionByVersionQuery, ReneeOperationResult<bool?>>
{
	public async Task<ReneeOperationResult<bool?>> Handle(
		CheckDuplicatedCguVersionByVersionQuery request, CancellationToken cancellationToken)
	{
		try
		{
			var result = await cguVersionRepository.VerifiyDuplicatedVersionAsync(request.Version);
			return ReneeOperationResult<bool?>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
