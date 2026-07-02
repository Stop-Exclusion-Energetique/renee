using Renee.Application.Abstraction.Query;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Occupant;

public class GetAllHouseholdHeatingEnergyQuery : IQuery<ReneeOperationResult<List<HouseholdHeatingEnergyLabel>>>
{}
