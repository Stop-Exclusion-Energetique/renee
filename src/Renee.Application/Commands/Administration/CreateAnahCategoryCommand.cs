using MediatR;
using Renee.Application.DTOs.Administration;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record CreateAnahCategoryCommand(AnahCategoryDto AnahCategoryDto, Guid ConnectedUserId) : IRequest<ReneeOperationResult<bool>>;