using Renee.Application.DTOs.Administration;
using Renee.Application.Queries.Administration;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces.Administration;

public interface IAnahCategoryService
{
	Task<ReneeOperationResult<bool>> CreateAnahCategory(AnahCategoryDto dto, Guid connectedUserId);
	Task<ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>> GetAllAnahCategories();
	Task<ReneeOperationResult<bool>> UpdateAnahCategory(AnahCategoryDto dto, Guid connectedUserId);

	Task<ReneeOperationResult<bool>> CreateAnahCategoryForSuplemntaryOccupant(AnahCategorySuplementaryOccupantIncome input);
	Task<ReneeOperationResult<bool>> UpdateAnahCategoryForSuplemntaryOccupant(AnahCategorySuplementaryOccupantIncome input);

}