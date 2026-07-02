using Renee.Application.Helpers;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.RealizeAndFollow.Presenter;

public class CreateRealiseAndFollowSynthesisPresenter
{
	private CreateRealiseAndFollowSynthesisViewModel? _synthesis;

	public CreateRealiseAndFollowSynthesisPresenter FromQuery(
		GetRealiseAndFollowSynthesisQueryObjectResult queryObjectResult)
	{
		_synthesis = new CreateRealiseAndFollowSynthesisViewModel
		{
			Id = queryObjectResult.Id,
			AccompaniyingFileReference = queryObjectResult.AccompaniyingFileReference,
			Stage = queryObjectResult.Stage,
			Status = queryObjectResult.Status,
			AccompanyingCost = queryObjectResult.AccompanyingCost,
			HouseholdAutoFinancing = queryObjectResult.HouseholdAutoFinancing,
			InvoicesSummary = queryObjectResult.InvoicesSummary,
			IntermediateAirtightnessTestResult = queryObjectResult.IntermediateAirtightnessTestResult,
			WaterproofingTreatmentActions = queryObjectResult.WaterproofingTreatmentActions,
			HasEffectiveComplianceWithWorkRecommendations =
				queryObjectResult.HasEffectiveComplianceWithWorkRecommendations,
			HasWorkEnablingHomeSupport = queryObjectResult.HasWorkEnablingHomeSupport,
			WellBeingRating = queryObjectResult.WellBeingRating,
			EducationalFrameworkRating = queryObjectResult.EducationalFrameworkRating,
			FamilySatisfactionWithSupport = queryObjectResult.FamilySatisfactionWithSupport,
			IsBackToEmployment = queryObjectResult.IsBackToEmployment,
			EndOfAccompanyingDate = queryObjectResult.EndOfAccompanyingDate,
			AccompanyingTime = queryObjectResult.AccompanyingTime,
			EndOfEncounterDate = queryObjectResult.EndOfEncounterDate,
			IsInTzeeProgram = queryObjectResult.IsInTzeeProgram,
			SolidarBuilder = queryObjectResult.SolidarBuilder,
			SecondSolidarBuilder = queryObjectResult.SecondSolidarBuilder,
			ThirdSolidarBuilder = queryObjectResult.ThirdSolidarBuilder,
			DiffuseCoordinator = queryObjectResult.DiffuseCoordinator,
			TargetedCoordinator = queryObjectResult.TargetedCoordinator,
			TerritorialBuilder = queryObjectResult.TerritorialBuilder,
			SecondTerritorialBuilder = queryObjectResult.SecondTerritorialBuilder,
			AccompanyingTimeDuration = queryObjectResult.AccompanyingTimeDuration.GetDescription()
		};

		return this;
	}

	public CreateRealiseAndFollowSynthesisViewModel Present()
	{
		return _synthesis!;
	}
}