using MediatR;
using Renee.Application.Commands.Administration;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Administration;

public class CreateAnahCategoryForSuplemntaryOccupantCommandHandler(
	IAnahCategorySuplementaryOccupantIncomeRepository anahCategorySuplementaryOccupantIncomeRepository,
	ITelemetryService telemetryService) : IRequestHandler<CreateAnahCategoryForSuplemntaryOccupantCommand, ReneeOperationResult<bool>>
{
	public async Task<ReneeOperationResult<bool>> Handle(CreateAnahCategoryForSuplemntaryOccupantCommand request, CancellationToken cancellationToken)
	{
		try
		{
			if(request.input is not {SuplementaryOccupantVerylowIncome: not null, SuplementaryOcupantLowIncome: not null, IsInIleDeFrance: not null, StartRuleDate: not null })
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.FillAllNeededFieldsForSupplementaryOccupants);

			var supplementaryOccupantIncome = await anahCategorySuplementaryOccupantIncomeRepository.GetAnahCategorySuplementaryOccupantIncomesAsync();

			if (supplementaryOccupantIncome
				.FirstOrDefault(a =>
					a.StartRuleDate!.Value.Year == request.input.StartRuleDate.Value.Year &&
					a.IsInIleDeFrance == request.input.IsInIleDeFrance
				) is not null)
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.SupplementaryOccupantRulesWithSameDateAlreadyExists);

			var result = await anahCategorySuplementaryOccupantIncomeRepository.AddNewAnahCategorySuplementaryOccupantIncome(request.input);

			if (result > -1)
				return ReneeOperationResult<bool>.Success(true, Labels.CreateSupplementaryOccupantRuleSuccess);
			else
				return ReneeOperationResult<bool>.Failure(AnahTableLabels.Errors.ErrorWhileAddingNewSupplementaryOccupantRule);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}
