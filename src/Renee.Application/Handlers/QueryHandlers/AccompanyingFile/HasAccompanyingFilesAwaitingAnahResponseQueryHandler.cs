using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public sealed class HasAccompanyingFilesAwaitingAnahResponseQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<HasAccompanyingFilesAwaitingAnahResponseQuery, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(HasAccompanyingFilesAwaitingAnahResponseQuery request, CancellationToken cancellationToken)
	{
		try
		{
			return ReneeOperationResult<bool>.Success(await accompanyingFileRepository.HasAwaitingAnahResponseAsync(request.UserId));
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}