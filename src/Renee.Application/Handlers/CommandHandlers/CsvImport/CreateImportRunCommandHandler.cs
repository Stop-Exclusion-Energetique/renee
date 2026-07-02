using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.CsvImport;

public class CreateImportRunCommandHandler(
	IImportRunRepository importRunRepository,
	ITelemetryService telemetryService) : IRequestHandler<CreateImportRunCommandInput, ReneeOperationResult<Guid?>>
{
	public async Task<ReneeOperationResult<Guid?>> Handle(CreateImportRunCommandInput request, CancellationToken cancellationToken)
	{
		try
		{
			var importRun = new ImportRun { StartedAt = DateTime.Now };

			var result = await importRunRepository.AddImportRun(importRun);

			return result > 0
				? ReneeOperationResult<Guid?>.Success(importRun.Id)
				: ReneeOperationResult<Guid?>.Failure(Labels.Errors.ErrorWhileCreatingImportRun);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<Guid?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}