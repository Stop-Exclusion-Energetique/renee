using MediatR;
using Renee.Application.DTOs.PreWorkPlan;
using Renee.Application.Interfaces;
using Renee.Application.Queries.PreWorkPlan;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.PreWorkPlan;

public class GetAllInsuranceTypesQueryHandler(
	IInsuranceTypeRepository preWorkPlanInsuranceTypeRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllInsuranceTypesQuery, ReneeOperationResult<IEnumerable<InsuranceTypeDto>>>
{
	public async Task<ReneeOperationResult<IEnumerable<InsuranceTypeDto>>> Handle(
		GetAllInsuranceTypesQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var result = (await preWorkPlanInsuranceTypeRepository.GetAllAsync()).Select(it => new InsuranceTypeDto(it.Id, it.Label));
			return ReneeOperationResult<IEnumerable<InsuranceTypeDto>>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<IEnumerable<InsuranceTypeDto>>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}