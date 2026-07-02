using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record UpdateAnahCategoryForSuplemntaryOccupantCommand(AnahCategorySuplementaryOccupantIncome input) : IRequest<ReneeOperationResult<bool>>;

