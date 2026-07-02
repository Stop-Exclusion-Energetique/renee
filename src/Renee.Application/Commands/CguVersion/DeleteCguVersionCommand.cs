using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.CguVersion;

public record DeleteCguVersionCommand(Guid CguId): IRequest<ReneeOperationResult<bool>>;
