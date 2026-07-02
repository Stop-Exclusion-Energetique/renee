using MediatR;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Commands.Administration;

public record CreateAnahCategoryForSuplemntaryOccupantCommand(AnahCategorySuplementaryOccupantIncome input) : IRequest<ReneeOperationResult<bool>>;
