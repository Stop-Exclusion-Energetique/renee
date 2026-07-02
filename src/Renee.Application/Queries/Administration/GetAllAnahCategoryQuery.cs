using MediatR;
using Renee.Application.DTOs.Administration;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Administration;

public sealed class GetAllAnahCategoryQuery : IRequest<ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>>;

public record GetAllAnahCategoryQueryObjectResult(List<AnahCategoryDto> AnahCategories, List<AnahCategorySuplementaryOccupantIncome> SupplementaryOccupantIncomes)
{}