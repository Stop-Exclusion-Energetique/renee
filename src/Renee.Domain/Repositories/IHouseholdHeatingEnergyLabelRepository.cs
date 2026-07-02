using Renee.Domain.Entity;

namespace Renee.Domain.Repositories;

public interface IHouseholdHeatingEnergyLabelRepository
{
	Task<List<HouseholdHeatingEnergyLabel>> GetAllHouseHoldHeatingEnergyLabelAsync();
}
