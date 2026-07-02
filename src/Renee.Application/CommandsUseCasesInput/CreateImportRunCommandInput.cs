using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateImportRunCommandInput() : IRequest<ReneeOperationResult<Guid?>>;