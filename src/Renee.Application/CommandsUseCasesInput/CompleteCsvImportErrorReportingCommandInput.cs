using MediatR;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record CompleteCsvImportErrorReportingCommandInput(
	string? OperatorName,
	ImportCsvResultStatus ImportCsvResultStatus,
	Guid ImportRunId,
	List<LineErrorReport> LineErrorReports) : IRequest<ReneeOperationResult<bool>>;