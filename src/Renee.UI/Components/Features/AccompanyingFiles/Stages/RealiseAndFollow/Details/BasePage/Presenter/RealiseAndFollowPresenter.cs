using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain.ReneeError;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectCost.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.Presenter;

public class RealiseAndFollowPresenter
{
	private RealiseAndFollowViewModel? _viewModel;
	private string? _errorMessage;

	public RealiseAndFollowPresenter FromQuery(
		ReneeOperationResult<GetAccompanyingFileForRealizeAndFollowMilestoneQueryObjectResult> queryObjectResult)
	{
		if (queryObjectResult.IsSuccess)
		{
			var resultValue = queryObjectResult.Value;

			_viewModel = new RealiseAndFollowViewModel
			{
				AccompanyingFileStatus = resultValue!.Status,
				Reference = resultValue.Reference,
				IsInTzeeProgram = resultValue.IsInTzeeProgram,
                IsImported = resultValue.IsImported,
                ReportingStructureName = resultValue.ReportingStructureName,
                ProjectCostViewModel =
				{
					AccompanyingCost = resultValue.AccompanyingCost,
					HouseholdAutoFinancing = resultValue.HouseholdAutoFinancing,
					Invoices =
						resultValue.Invoices.Select(
							invoice => new InvoiceViewModel
							{
								Id = invoice.Id,
								InvoiceTotalCost = invoice.TotalCost,
								LaborBilled = invoice.BilledWorkForce
						}).ToList()
				},
				WorkSummaryViewModel =
				{
					IntermediateAirtightnessTestResult = resultValue.IntermediateAirtightnessTestResult,
					WaterproofingTreatmentActions = resultValue.WaterproofingTreatmentActions,
					HasEffectiveComplianceWithWorkRecommendations =
						resultValue.HasEffectiveComplianceWithWorkRecommendations,
					HasWorkEnablingHomeSupport = resultValue.HasWorkEnablingHomeSupport,
					HasHousingAdaptationWorks = resultValue.HasHousingAdaptationWorks,
					HasFinishingWorks = resultValue.HasFinishingWorks,
					HasSafetyWorks = resultValue.HasSafetyWorks,
					HasPreparationWorks = resultValue.HasPreparationWorks,
					HasEmergencyWorks = resultValue.HasEmergencyWorks,
					HasUnsanitaryExit = resultValue.HasUnsanitaryExit,
					TreatedAirTightness = resultValue.TreatedAirTightness,
					TreatedThermalBridges = resultValue.TreatedThermalBridges,
					HasHumidityManagement = resultValue.HasHumidityManagement,
					ShouldChangeFinalEstimatedDpe = resultValue.ShouldChangeFinalEstimatedDpe,
					InitialDpe = resultValue.InitialDpe,
					EstimatedDpe = resultValue.EstimatedDpe,
					FinalDpe = resultValue.FinalDpe,
					FinalDpeClassJump = resultValue.FinalDpeClassJump
				},
				EvaluationsViewModel =
				{
					WellBeingRating = resultValue.WellBeingRating,
					EducationalFrameworkRating = resultValue.EducationalFrameworkRating,
					FamilySatisfactionWithSupport = resultValue.FamilySatisfactionWithSupport,
					IsBackToEmployment = resultValue.IsBackToEmployment
				},
				ProjectEndViewModel =
				{
					EndOfAccompanyingDate = resultValue.EndOfAccompanyingDate,
					EndOfEncounterDate = resultValue.EndOfEncounterDate,
					StartOfAccompanyingDate = resultValue.StartOfAccompanyingDate,
					AccompanyingTimeDuration = resultValue.AccompanyingTimeDuration
				},
				FinalFinancingPlanViewModel = new PreFinancingPlanViewModel
				{
					UnderprivilegedHousingFoundation = resultValue.UnderprivilegedHousingFoundation,
					AdaptationBonus = resultValue.AdaptationBonus,
					BankLoanType = resultValue.BankLoanType,
					ClassicBankLoan = resultValue.ClassicBankLoan,
					CoOwnershipBonus = resultValue.CoOwnershipBonus,
					DecentHousingBonus = resultValue.DecentHousingBonus,
					Department = resultValue.Department,
					DepartmentalHouseForDisabledPersons = resultValue.DepartmentalHouseForDisabledPersons,
					EnergySavingCertificates = resultValue.EnergySavingCertificates,
					ExitEnergySieveBonus = resultValue.ExitEnergySieveBonus,
					FamilyAllowanceFund = resultValue.FamilyAllowanceFund,
					GuidedPathwayBonus = resultValue.GuidedPathwayBonus,
					LeroyMerlinFoundation = resultValue.LeroyMerlinFoundation,
					HouseholdMaximumSavingAmountForRenovationProject =
						resultValue.HouseholdMaximumSavingAmountForRenovationProject,
					MaximumAmountSupportFamilyMembersRenovationProject =
						resultValue.MaximumAmountSupportFamilyMembersRenovationProject,
					Municipality = resultValue.Municipality,
					PensionFund = resultValue.PensionFund,
					PublicEstablishmentsIntercommunalCooperation =
						resultValue.PublicEstablishmentsIntercommunalCooperation,
					Region = resultValue.Region,
					SocialProtectionGroup = resultValue.SocialProtectionGroup,
					StopEnergyExclusionFunds = resultValue.StopEnergyExclusionFunds,
					WattForChangeFoundation = resultValue.WattForChangeFoundation,
					FundingModes =
						resultValue.FundingModes?.Select(
								fm => new FundingModeViewModel { Id = fm.Id, Name = fm.Label, Amount = fm.Value })
							.ToList() ?? []
				},
				SiteSupervisionViewModel = new SiteSupervisionViewModel
				{
					OverallStartDate = resultValue.OverallStartDate,
					EstimatedOverallCompletionDate = resultValue.EstimatedOverallCompletionDate,
					ActualOverallEndDate = resultValue.ActualOverallEndDate,
					OverallProgress = resultValue.OverallProgress,
					NextCoordinationMeetingScheduledFor = resultValue.NextCoordinationMeetingScheduledFor,
					OverallObservations = resultValue.OverallObservations,
					PreSiteSupervisionMeetingDate = resultValue.PreSiteSupervisionMeetingDate,
					WorkParticipants = [.. resultValue.WorkParticipants.Select(
						wp => new WorkParticipantViewModel
						{
							Id = wp.Id,
							ParticipantType = wp.ParticipantType,
							WorkTypesLabel = wp.WorkTypesLabel,
							ParticipantName = wp.ParticipantName,
							ContactAdvisor = wp.ContactAdvisor,
							StartDateOfWork = wp.StartDateOfWork,
							EstimatedCompletionDate = wp.EstimatedCompletionDate,
							ActualEndDate = wp.ActualEndDate,
							Progress = wp.Progress,
							WorkQuality = wp.WorkQuality,
							CommentOnWorkQuality = wp.CommentOnWorkQuality,
							WorkParticipantDifficulties = wp.WorkParticipantDifficulties,
							CommentOnWorkParticipantDifficulties = wp.CommentOnWorkParticipantDifficulties,
							SpecificComments = wp.SpecificComments
						})],
					AnahGrantDate = resultValue.AnahGrantDate
				}
			};
		}

		if(!queryObjectResult.IsSuccess)
			_errorMessage = queryObjectResult.Message;

		return this;
	}

	public (RealiseAndFollowViewModel, string?) Present()
	{
		if (_viewModel!.ProjectCostViewModel.Invoices.Count == 0)
			_viewModel.ProjectCostViewModel.Invoices.Add(new InvoiceViewModel());
		return (_viewModel!, _errorMessage);
	}
}