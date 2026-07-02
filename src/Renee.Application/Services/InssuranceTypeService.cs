using MediatR;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.Interfaces;
using Renee.Application.Queries.PreWorkPlan;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public class InsuranceTypeService(IMediator mediator) : IInsuranceTypeService
{
	public async Task<ReneeOperationResult<IEnumerable<InsuranceTypeDto>>> GetAllInsuranceTypes() =>
		await mediator.Send(new GetAllInsuranceTypesQuery());
}