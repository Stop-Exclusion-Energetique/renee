using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.User;

public record ChangeUserReportingStructureCommand(Guid UserId, Guid ReportingStructureId) : IRequest<ReneeOperationResult<bool>>;
