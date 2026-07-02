using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.CsvImport;

public class CompleteCsvImportErrorReportingCommandHandler(
	IImportRunRepository importRunRepository,
	IImportErrorRepository importErrorRepository,
	ITelemetryService telemetryService) : IRequestHandler<CompleteCsvImportErrorReportingCommandInput, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(
		CompleteCsvImportErrorReportingCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			var importRun = await importRunRepository.GetImportRunById(request.ImportRunId);
			if (importRun is null)
				return ReneeOperationResult<bool>.Failure(Labels.Errors.ImportRunNotFound);

			importRun.Operator = request.OperatorName;
			importRun.ImportStatus = (int?)request.ImportCsvResultStatus;
			importRun.EndedAt = DateTime.UtcNow;

			await importRunRepository.UpdateImportRun(importRun);

			if (request.LineErrorReports.Count == 0)
				return ReneeOperationResult<bool>.Success(true, Labels.ImportCsvSuccess);

			var importErrors = request.LineErrorReports.SelectMany(lineReport =>
				lineReport.GetErrors().Select(errorMessage => new ImportError
				{
					ImportRunId = request.ImportRunId,
					LineNumber = lineReport.LineNumber,
					ErrorMessage = errorMessage
				})).ToList();

			var result = await importErrorRepository.AddImportErrors(importErrors);

			return result > 0
				? ReneeOperationResult<bool>.Success(true, Labels.ImportCsvSuccess)
				: ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileSavingImportErrors);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}