using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IInsuranceTypeRepository
{
	Task<IEnumerable<InsuranceType>> GetAllAsync();
}