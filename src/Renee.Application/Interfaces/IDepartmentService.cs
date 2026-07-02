using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IDepartmentService
{
	Task<ReneeOperationResult<IEnumerable<DepartmentDto>>> GetAllAsync();
}