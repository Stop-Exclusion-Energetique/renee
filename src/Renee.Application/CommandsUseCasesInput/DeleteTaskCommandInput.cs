using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record DeleteTaskCommandInput(Guid TaskId) : IRequest<ReneeOperationResult<int>>;