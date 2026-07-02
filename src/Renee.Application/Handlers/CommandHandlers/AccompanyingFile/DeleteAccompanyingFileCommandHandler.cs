using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.AccompanyingFile;

public class DeleteAccompanyingFileCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<DeleteAccompanyingFileCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(DeleteAccompanyingFileCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var numberItemsChanged = await accompanyingFileRepository.DeleteAccompanyingFile(request.Id);
			return numberItemsChanged > -1
				? ReneeOperationResult<bool>.Success(true, Labels.DeleteAccompanyingFileSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileDeletingAccompanyingFile);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}