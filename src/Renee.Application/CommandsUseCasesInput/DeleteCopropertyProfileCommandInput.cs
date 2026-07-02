using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record DeleteCopropertyProfileCommandInput(Guid Id) : IRequest<ReneeOperationResult<bool>>;
