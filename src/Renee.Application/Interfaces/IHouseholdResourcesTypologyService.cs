using Renee.Application.DTOs.Occupant;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public interface IHouseholdResourcesTypologyService
{
	Task<ReneeOperationResult<IEnumerable<HouseholdResourcesTypologyDto>>> GetAllAsync();
}