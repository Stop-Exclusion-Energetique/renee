using System.Linq.Expressions;
using MediatR;
using Renee.Application.DTOs.Administration;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Administration;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Administration;

public class GetAllAnahCategoryQueryHandler(
	IAnahCategoryRepository anahCategoryRepository,
	IAnahCategorySuplementaryOccupantIncomeRepository anahCategorySuplementaryOccupantIncomeRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetAllAnahCategoryQuery, ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>>
{
	public async Task<ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>> Handle(
		GetAllAnahCategoryQuery request,
		CancellationToken cancellationToken)
	{
		try
		{
			var anahCategories = await anahCategoryRepository.GetAllAnahCategoriesAsync();
			var suplementaryOccupantIncome = await anahCategorySuplementaryOccupantIncomeRepository.GetAnahCategorySuplementaryOccupantIncomesAsync();

			if (anahCategories == null || suplementaryOccupantIncome == null)
				return ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>.Failure(Labels.Errors.ErrorWhileRetrievingAnahCategories);

			var result = new GetAllAnahCategoryQueryObjectResult(
										anahCategories
											.Select(
												ac => new AnahCategoryDto(
													ac.Id,
													new AnahCategoryData(
														ac.PeopleNumber,
														ac.VeryLowIncomeHouseholdsAmount,
														ac.LowIncomeHouseholdsAmount,
														ac.Year,
														ac.AnahRuleDebutDate,
														ac.AnahRuleEndDate),
													ac.IsInIleDeFrance
											)
										)
									.ToList(), 
									suplementaryOccupantIncome
								);

			return ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<GetAllAnahCategoryQueryObjectResult>.Failure(Labels.Errors.ErrorWhileRetrievingAnahCategories);
		}
	}
}