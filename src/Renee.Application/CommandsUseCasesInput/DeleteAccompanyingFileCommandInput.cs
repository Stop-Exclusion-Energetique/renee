using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record DeleteAccompanyingFileCommandInput(Guid Id) : IRequest<ReneeOperationResult<bool>>;