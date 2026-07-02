using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.OrganizeAndFinance.Presenter;

public class OrganizeAndFinanceSynthesisPresenter
{
	private OrganizeAndFinanceSynthesisViewModel? _synthesis;

	public OrganizeAndFinanceSynthesisPresenter FromQuery(
		GetOrganizeAndFinanceSynthesisQueryObjectResult queryObjectResult)
	{
		_synthesis = new OrganizeAndFinanceSynthesisViewModel
		{
			AccompanyingFileReference = queryObjectResult.AccompanyingFileReference,
			AreExistingHumidityAndVaporMigrationManagedAfterTreatment =
				queryObjectResult.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
			EstimatedAnnualEnergyConsumptionAfterWork = queryObjectResult.EstimatedAnnualEnergyConsumptionAfterWork,
			EstimatedEnergyClassJump = queryObjectResult.EstimatedEnergyClassJump,
			EstimatedEnergyDpeAfterWork = queryObjectResult.EstimatedEnergyDpeAfterWork,
			Id = queryObjectResult.Id,
			IsEmergencyWorks = queryObjectResult.IsEmergencyWorks,
			NextStepAndVigilancePoints = queryObjectResult.NextStepAndVigilancePoints,
			PreWorkPlanProjectType = queryObjectResult.PreWorkPlanProjectType,
			RenovationType = queryObjectResult.RenovationType,
			Stage = queryObjectResult.Stage,
			Status = queryObjectResult.Status,
			TreatedAirTightness = queryObjectResult.TreatedAirTightness,
			TreatedThermalBridge = queryObjectResult.TreatedThermalBridge,
			WorkPackageSummary = queryObjectResult.WorkPackageSummary,
			SolidarBuilder = queryObjectResult.SolidarBuilder,
			SecondSolidarBuilder = queryObjectResult.SecondSolidarBuilder,
			ThirdSolidarBuilder = queryObjectResult.ThirdSolidarBuilder,
			DiffuseCoordinator = queryObjectResult.DiffuseCoordinator,
			TargetedCoordinator = queryObjectResult.TargetedCoordinator,
			TerritorialBuilder = queryObjectResult.TerritorialBuilder,
			SecondTerritorialBuilder = queryObjectResult.SecondTerritorialBuilder,
			AnahFolderNumber = queryObjectResult.AnahFolderNumber,
			AnahFolderFilingDate = queryObjectResult.AnahFolderFilingDate,
			PreFinancingPlanViewModel = new PreFinancingPlanViewModel
			{
				UnderprivilegedHousingFoundation = queryObjectResult.UnderprivilegedHousingFoundation,
				AdaptationBonus = queryObjectResult.AdaptationBonus,
				BankLoanType = queryObjectResult.BankLoanType,
				ClassicBankLoan = queryObjectResult.ClassicBankLoan,
				CoOwnershipBonus = queryObjectResult.CoOwnershipBonus,
				DecentHousingBonus = queryObjectResult.DecentHousingBonus,
				Department = queryObjectResult.Department,
				DepartmentalHouseForDisabledPersons = queryObjectResult.DepartmentalHouseForDisabledPersons,
				EnergySavingCertificates = queryObjectResult.EnergySavingCertificates,
				ExitEnergySieveBonus = queryObjectResult.ExitEnergySieveBonus,
				FamilyAllowanceFund = queryObjectResult.FamilyAllowanceFund,
				GuidedPathwayBonus = queryObjectResult.GuidedPathwayBonus,
				LeroyMerlinFoundation = queryObjectResult.LeroyMerlinFoundation,
				HouseholdMaximumSavingAmountForRenovationProject =
					queryObjectResult.HouseholdMaximumSavingAmountForRenovationProject,
				MaximumAmountSupportFamilyMembersRenovationProject =
					queryObjectResult.MaximumAmountSupportFamilyMembersRenovationProject,
				Municipality = queryObjectResult.Municipality,
				PensionFund = queryObjectResult.PensionFund,
				PublicEstablishmentsIntercommunalCooperation =
					queryObjectResult.PublicEstablishmentsIntercommunalCooperation,
				Region = queryObjectResult.Region,
				SocialProtectionGroup = queryObjectResult.SocialProtectionGroup,
				StopEnergyExclusionFunds = queryObjectResult.StopEnergyExclusionFunds,
				WattForChangeFoundation = queryObjectResult.WattForChangeFoundation,
				FundingModes = queryObjectResult.FundingModes.Select(fm => new FundingModeViewModel
				{
					Name = fm.Label,
					Amount = fm.Value,
				}).ToList(),
			},
			InitialDpe = queryObjectResult.InitialDpeLabel
		};
		return this;
	}

	public OrganizeAndFinanceSynthesisViewModel Present()
	{
		return _synthesis!;
	}
}