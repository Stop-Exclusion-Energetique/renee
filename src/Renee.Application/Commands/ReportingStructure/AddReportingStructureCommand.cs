using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.ReportingStructure;

public record AddReportingStructureCommand(string? ReportingStructureName) : IRequest<ReneeOperationResult<Guid>>;
