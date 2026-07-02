using MediatR;
using Renee.Application.DTOs.CguVersion;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.CguVersion;

public record CreateCguVersionCommand(CguVersionDto CguVersionDto) : IRequest<ReneeOperationResult<bool>>;
