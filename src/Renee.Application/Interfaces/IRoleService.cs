using Renee.Application.DTOs.User;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IRoleService
{
	Task<ReneeOperationResult<IEnumerable<RoleDto>>> GetAllAsync();
}