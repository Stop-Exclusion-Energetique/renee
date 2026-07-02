using MediatR;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Occupant;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Occupant;

public class GetFinancialAidFromAnahQueryHandler(
	IAnahCategoryRepository anahCategoryRepository,
	IAnahCategorySuplementaryOccupantIncomeRepository anahCategorySuplementaryOccupantIncomeRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<GetFinancialAidFromAnahQuery, ReneeOperationResult<string?>>
{
	public async Task<ReneeOperationResult<string?>> Handle(GetFinancialAidFromAnahQuery request, CancellationToken cancellationToken)
	{
		try
		{
			if(request is { PeopleNumber: null, Resources: null , IsInIleDeFrance: null })
				return ReneeOperationResult<string?>.Failure(Labels.Errors.NoData);

			var allAnahCategories = await anahCategoryRepository.GetAllAnahCategoriesAsync();
			var allSupplementaryOccupantIncomeCategories = await anahCategorySuplementaryOccupantIncomeRepository.GetAnahCategorySuplementaryOccupantIncomesAsync();

			if ((allAnahCategories is null or { Count: 0 }) || (allSupplementaryOccupantIncomeCategories is null or { Count: 0 }) ) 
				return ReneeOperationResult<string?>.Success(Labels.NoAid);

			var result = GetAnahCategory(request.PeopleNumber, request.IsInIleDeFrance, request.Resources, request.StartOfAccompanyingDate, allAnahCategories, allSupplementaryOccupantIncomeCategories);
			return ReneeOperationResult<string?>.Success(result);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<string?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static string GetAnahCategory(int? nummberOfPeopleInHousehold, bool? IsInIleDeFrance, double? resources, DateTime? accompanyingStartDate, List<AnahCategory> anahCategories, List<AnahCategorySuplementaryOccupantIncome> anahSupplementaryOccupantCategpry)
	{
		if(nummberOfPeopleInHousehold > 5)
		{
			var numberOfSupplementaryOccupant = nummberOfPeopleInHousehold - 5;

			var anahCategory = anahCategories
							.Find(
								anah => anah.IsInIleDeFrance == IsInIleDeFrance &&
										anah.AnahRuleDebutDate!.Value.Year == accompanyingStartDate!.Value.Year &&
										anah.PeopleNumber == 5
							);

			var supplementaryOccupantRules = anahSupplementaryOccupantCategpry
											.Find(
												anah => anah.IsInIleDeFrance == IsInIleDeFrance &&
														anah.StartRuleDate!.Value.Year == accompanyingStartDate!.Value.Year
											);

			if (anahCategory is null || supplementaryOccupantRules is null)
				return Labels.NoAid;

			var veryLowIncome = anahCategory.VeryLowIncomeHouseholdsAmount + (numberOfSupplementaryOccupant * supplementaryOccupantRules.SuplementaryOccupantVerylowIncome);
			var LowIncome = anahCategory.LowIncomeHouseholdsAmount + (numberOfSupplementaryOccupant * supplementaryOccupantRules.SuplementaryOcupantLowIncome);

			return GetAnahCategoryLabel(LowIncome, veryLowIncome, resources);
		}
		else
		{
			var anahCategory = anahCategories
							.Find(
								anah => anah.IsInIleDeFrance == IsInIleDeFrance &&
										anah.AnahRuleDebutDate!.Value.Year == accompanyingStartDate!.Value.Year &&
										anah.PeopleNumber == nummberOfPeopleInHousehold
							);

			if (anahCategory is null)
				return Labels.NoAid;

			return GetAnahCategoryLabel(anahCategory.LowIncomeHouseholdsAmount, anahCategory.VeryLowIncomeHouseholdsAmount, resources);
		}

		string GetAnahCategoryLabel (double? lowIncome, double? veryLowIncome, double? resources)
			=> resources switch
			{
				double n when veryLowIncome >= n => Labels.VeryLowIncomeHouseholdsAmount,
				double n when (veryLowIncome < n ) && (n <= lowIncome) => Labels.LowIncomeHouseholdsAmount,
				_ => Labels.NoAid
			};
	}
}