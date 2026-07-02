using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Administration;

public class UpdateAnahCategoryCommandHandler(
	IAnahCategoryRepository anahCategoryRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<UpdateAnahCategoryCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(UpdateAnahCategoryCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if (request.AnahCategoryDto is not { PeopleNumber: not null, LowIncomeHouseholdsAmount: not null, VeryLowIncomeHouseholdsAmount: not null, RuleStartDate: not null }) 
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.FillAllNeededFields);

			if (request.AnahCategoryDto.PeopleNumber > 5)
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.PeopleInHouseholdIsBiggerThanFive);

			var anahCategories = await anahCategoryRepository.GetAllAnahCategoriesAsync();

			if (anahCategories
				.FirstOrDefault(a =>
					a.PeopleNumber == request.AnahCategoryDto.PeopleNumber &&
					a.AnahRuleDebutDate!.Value.Year == request.AnahCategoryDto.RuleStartDate.Value.Year &&
					a.IsInIleDeFrance == request.AnahCategoryDto.IsInIleDeFrance) is not null)
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.AnahWithSameNumberOfPeopleAndAnahYearDebutRule);

			var entity = new AnahCategory
			{
				Id = request.AnahCategoryDto.Id,
				PeopleNumber = request.AnahCategoryDto.PeopleNumber.Value,
				IsInIleDeFrance = request.AnahCategoryDto.IsInIleDeFrance,
				LowIncomeHouseholdsAmount = request.AnahCategoryDto.LowIncomeHouseholdsAmount,
				VeryLowIncomeHouseholdsAmount = request.AnahCategoryDto.VeryLowIncomeHouseholdsAmount,
				AnahRuleDebutDate = request.AnahCategoryDto.RuleStartDate,
				AnahRuleEndDate = request.AnahCategoryDto.RuleEndDate,
				LastUpdateById = request.ConnectedUserId,
				LastUpdateDatetimeUtc = DateTime.Now
			};

			var numberItemsChanged = await anahCategoryRepository.UpdateAnahCategoryAsync(entity);

			if(numberItemsChanged > -1)
				return ReneeOperationResult<bool>.Success(true, Labels.UpdateAnahCategorySuccess);

			else
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.ErrorWhileCreatingAnahCategory);
		}
		catch (Exception ex) 
		{ 
			await telemetryService.TrackExceptionAsync(ex, new CancellationToken());
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}