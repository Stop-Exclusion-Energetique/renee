using System.Security.Claims;
using Blazored.Modal.Services;
using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.Result;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout.StageLayout;
using Renee.Application.DTOs.AI;
using Renee.Domain.ReneeError;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.BasePage;

public partial class RealiseAndFollow
{
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public ISendEventQuery QuerySender { get; set; } = null!;
	[Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
	[Inject] public IJSRuntime? JsRuntime { get; set; }
	[Inject] public IModalService ModalService { get; set; } = null!;
	[Inject] private UnsavedChangesGuard UnsavedChangesGuard { get; set; } = null!;

	[Parameter] public Guid AccompanyingFileId { get; set; }
    private string RedirectTabUrl => $"{Endpoints.RealiseAndFollowStage}/{AccompanyingFileId}";

    public RealiseAndFollowViewModel ViewModel { get; private set; } = new();

	public EditContext? EditContext { get; set; }

    [SupplyParameterFromQuery]
    public int SelectedTab { get; private set; }

	private bool _saveButtonIsActive;
	public bool SaveButtonIsActive
	{
		get => _saveButtonIsActive;
		set
		{
			_saveButtonIsActive = value;
			StateHasChanged();
		}
	}

	private string? UserRole;
	private Guid UserId;

	private List<ZeeSelectItem<AccompanyingTimeDuration?>> AccompanyingTimeDurations { get; } =
	[
		new(AccompanyingTimeDuration.LessThanTwoHours.GetDescription(), AccompanyingTimeDuration.LessThanTwoHours),
		new(AccompanyingTimeDuration.BetweenTwoAndFiveHours.GetDescription(), AccompanyingTimeDuration.BetweenTwoAndFiveHours),
		new(AccompanyingTimeDuration.BetweenFiveAndFourteenHours.GetDescription(), AccompanyingTimeDuration.BetweenFiveAndFourteenHours),
		new(AccompanyingTimeDuration.BetweenFourteenAndTwentyHours.GetDescription(), AccompanyingTimeDuration.BetweenFourteenAndTwentyHours),
		new(AccompanyingTimeDuration.BetweenTwentyAndTwentyEightHours.GetDescription(), AccompanyingTimeDuration.BetweenTwentyAndTwentyEightHours),
		new(AccompanyingTimeDuration.BetweenTwentyEightAndFourtyHours.GetDescription(), AccompanyingTimeDuration.BetweenTwentyEightAndFourtyHours),
		new(AccompanyingTimeDuration.BetweenFourtyAndSixtyHours.GetDescription(), AccompanyingTimeDuration.BetweenFourtyAndSixtyHours),
		new(AccompanyingTimeDuration.MoreThanSixtyHours.GetDescription(), AccompanyingTimeDuration.MoreThanSixtyHours)
	];

	private bool IsUserNotAllowedToEdit =>
		UserRole == Constants.StructuralReferentRole ||
		(UserRole == Constants.SolidarBuilderRole &&
		ViewModel.AccompanyingFileStatus == AccompanyingFileStatus.WaitingForApproval &&
		ViewModel.IsInTzeeProgram) || ViewModel.AccompanyingFileStatus == AccompanyingFileStatus.Finished ||
		ViewModel.AccompanyingFileStatus == AccompanyingFileStatus.Aborted;

    private bool IsSoliha { get; set; } = false;

	private bool IsLastTab => SelectedTab == 5;

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

		UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

		if (value != null)
			UserId = Guid.Parse(value);
		else
			throw new InvalidDataException("User Id not found in claims");

		if(UserRole is null)
		{
			NavigateToAccompanyingFileList();
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = Labels.Errors.ErrorWhileLoadingAccompanyingFile, Duration = 4000 });
			return;
		}

		UnsavedChangesGuard.UpdateCurrentUrl(NavigationManager.Uri);
		UnsavedChangesGuard.SetSaveHandler(Save);

		await LoadAccompanyingFileAsync();

		IsSoliha = ViewModel?.ReportingStructureName?.ToLowerInvariant().Contains(Labels.IsSolihaStructure, StringComparison.InvariantCultureIgnoreCase) == true && ViewModel?.IsImported == true;

		EditContext = new EditContext(ViewModel!);
		EditContext.OnFieldChanged += async (sender, args) => await HandleFieldChanged();
	}

	public async Task ChangeTab(int index)
	{
        await SaveWithoutValidation();
        SelectedTab = index;
		StateHasChanged();

		if (JsRuntime != null)
		{
			await JsRuntime.InvokeVoidAsync("scrollToTop");
		}
	}

    private string GetContextMenuRedirectUrl(int? index)
    {
        return index switch
        {
            0 or 1 or 2 or 3 or 4 => $"{RedirectTabUrl}?SelectedTab={index}",
            _ => $"{RedirectTabUrl}"
        };
    }

    public void NavigateToAccompanyingFileList()
	{
		NavigationManager.NavigateTo(Endpoints.UserCreatedAccompanyingFiles);
	}

	public async Task SaveWithoutValidation()
	{
		await Save();
	}

	public async Task SaveWithValidation()
	{
		if (EditContext!.Validate())
		{
			var result = await Save();

			if (result)
				NavigationManager.NavigateTo(
					$"{Endpoints.RealizeAndFollowSynthesis}/{AccompanyingFileId}");
		}
	}

	private async Task HandleFieldChanged()
	{
		if (SaveButtonIsActive) return;

		await UnsavedChangesGuard.SetUnsavedChangesState(true);
		SaveButtonIsActive = true;
	}

	private async Task<bool> Save()
	{
		try
		{
			var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

			var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

			if (Guid.TryParse(value, out var userId))
			{
				var projectCost = new UpdatedProjectCost(
					ViewModel.ProjectCostViewModel.AccompanyingCost,
					ViewModel.ProjectCostViewModel.HouseholdAutoFinancing,
					ViewModel.ProjectCostViewModel.WorkTotalCost);

				var workSummary = new UpdatedWorkSummary(
					ViewModel.WorkSummaryViewModel.IntermediateAirtightnessTestResult,
					ViewModel.WorkSummaryViewModel.WaterproofingTreatmentActions,
					ViewModel.WorkSummaryViewModel.HasEffectiveComplianceWithWorkRecommendations,
					ViewModel.WorkSummaryViewModel.ShouldChangeFinalEstimatedDpe,
					ViewModel.WorkSummaryViewModel.HasWorkEnablingHomeSupport,
					ViewModel.WorkSummaryViewModel.HasHousingAdaptationWorks,
					ViewModel.WorkSummaryViewModel.HasFinishingWorks,
					ViewModel.WorkSummaryViewModel.HasSafetyWorks,
					ViewModel.WorkSummaryViewModel.HasPreparationWorks,
					ViewModel.WorkSummaryViewModel.HasEmergencyWorks,
					ViewModel.WorkSummaryViewModel.HasUnsanitaryExit,
					(int?)ViewModel.WorkSummaryViewModel.TreatedAirTightness,
					(int?)ViewModel.WorkSummaryViewModel.TreatedThermalBridges,
					ViewModel.WorkSummaryViewModel.HasHumidityManagement);

				var evaluation = new UpdatedEvaluation(
					ViewModel.EvaluationsViewModel.WellBeingRating,
					ViewModel.EvaluationsViewModel.EducationalFrameworkRating,
					ViewModel.EvaluationsViewModel.FamilySatisfactionWithSupport,
					ViewModel.EvaluationsViewModel.IsBackToEmployment);

				var invoices = ViewModel.ProjectCostViewModel.Invoices
					.Select(i => new UpdatedInvoice(i.InvoiceTotalCost, i.LaborBilled, i.Id)).ToList();

				var updatePrefinancingPlan = new UpdatePreFinancingPlan(
				ViewModel.FinalFinancingPlanViewModel.GuidedPathwayBonus,
				ViewModel.FinalFinancingPlanViewModel.CoOwnershipBonus,
				ViewModel.FinalFinancingPlanViewModel.DecentHousingBonus,
				ViewModel.FinalFinancingPlanViewModel.AdaptationBonus,
				ViewModel.FinalFinancingPlanViewModel.ExitEnergySieveBonus,
				ViewModel.FinalFinancingPlanViewModel.Region,
				ViewModel.FinalFinancingPlanViewModel.Department,
				ViewModel.FinalFinancingPlanViewModel.PublicEstablishmentsIntercommunalCooperation,
				ViewModel.FinalFinancingPlanViewModel.Municipality,
				ViewModel.FinalFinancingPlanViewModel.BankLoanType,
				ViewModel.FinalFinancingPlanViewModel.ClassicBankLoan,
				ViewModel.FinalFinancingPlanViewModel.DepartmentalHouseForDisabledPersons,
				ViewModel.FinalFinancingPlanViewModel.EnergySavingCertificates,
				ViewModel.FinalFinancingPlanViewModel.HouseholdMaximumSavingAmountForRenovationProject,
				ViewModel.FinalFinancingPlanViewModel.FamilyAllowanceFund,
				ViewModel.FinalFinancingPlanViewModel.PensionFund,
				ViewModel.FinalFinancingPlanViewModel.UnderprivilegedHousingFoundation,
				ViewModel.FinalFinancingPlanViewModel.LeroyMerlinFoundation,
				ViewModel.FinalFinancingPlanViewModel.WattForChangeFoundation,
				ViewModel.FinalFinancingPlanViewModel.SocialProtectionGroup,
				ViewModel.FinalFinancingPlanViewModel.StopEnergyExclusionFunds,
				ViewModel.FinalFinancingPlanViewModel.FundingModes
					.Select(fm => new FundingModeDto(fm.Id, fm.Name, fm.Amount)).ToList(),
				ViewModel.FinalFinancingPlanViewModel.MaximumAmountSupportFamilyMembersRenovationProject);

				var updatedWorkParticipants = ViewModel.SiteSupervisionViewModel.WorkParticipants
					.Select(wp => new UpdatedWorkParticipant(
						wp.Id,
						wp.ParticipantType,
						wp.WorkTypesLabel,
						wp.ParticipantName,
						wp.ContactAdvisor,
						wp.StartDateOfWork,
						wp.EstimatedCompletionDate,
						wp.ActualEndDate,
						wp.Progress,
						wp.WorkQuality,
						wp.CommentOnWorkQuality,
						wp.WorkParticipantDifficulties,
						wp.CommentOnWorkParticipantDifficulties,
						wp.SpecificComments)).ToList();

				var updatedSiteSupervision = new UpdatedSiteSupervision(
					ViewModel.SiteSupervisionViewModel.OverallStartDate,
					ViewModel.SiteSupervisionViewModel.EstimatedOverallCompletionDate,
					ViewModel.SiteSupervisionViewModel.ActualOverallEndDate,
					ViewModel.SiteSupervisionViewModel.OverallProgress,
					ViewModel.SiteSupervisionViewModel.NextCoordinationMeetingScheduledFor,
					ViewModel.SiteSupervisionViewModel.OverallObservations,
					ViewModel.SiteSupervisionViewModel.PreSiteSupervisionMeetingDate,
					updatedWorkParticipants);

				var updatedHousingAfterWorkState = new UpdateHousingAfterWorkStateForRealizeAndFollowMilestone(
					ViewModel.WorkSummaryViewModel.FinalDpe,
					ViewModel.WorkSummaryViewModel.FinalDpeClassJump);

				var input = new SaveAccompanyingFileRealizeAndFollowCommandInput(
					projectCost,
					invoices,
					workSummary,
					evaluation,
					updatePrefinancingPlan,
					updatedSiteSupervision,
					updatedHousingAfterWorkState,
					ViewModel.ProjectEndViewModel.EndOfAccompanyingDate,
					ViewModel.ProjectEndViewModel.EndOfEncounterDate,
					ViewModel.ProjectEndViewModel.AccompanyingTimeDuration,
					ViewModel.SiteSupervisionViewModel.AnahGrantDate,
					AccompanyingFileId,
					userId);

				var result = await AccompanyingFileService.UpdateAccompanyingFileForRealizeAndFollowMilestone(input);

					if (result.IsSuccess && result.Value) StageNavigationStateService.NotifyStateChanged();

					NotificationService.Notify(new NotificationMessage
					{
						Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
						Summary = result.Message,
						Duration = 4000
					});

					return result.IsSuccess && result.Value;
			}

			return false;
		}
		catch (Exception) { return false; }
		finally
		{
			await UnsavedChangesGuard.SetUnsavedChangesState(false);
			SaveButtonIsActive = false;
		}
	}

	private async Task ShowAbortAccompanyingFileModal()
	{
		if (ModalService == null)
			return;

		var modalResult = await ModalService.Show<AbortAccompanyingFileModal>(
		new ModalParameters().Add(nameof(AbortAccompanyingFileModal.AccompanyingFileReference), ViewModel!.Reference),
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true, Size = ModalSize.ExtraLarge }).Result;

		if (modalResult.Cancelled)
			return;

		var saveResult = await Save();

		if (modalResult.Data is not AbortAccompanyingFileModalResult modalData || !saveResult)
			return;

		var abortCommandResult = await AccompanyingFileService.AbortAccompanyingFile(new AbortAccompanyingFileCommandInput(
			UserId,
			AccompanyingFileId,
			modalData.AbortReasonLabelId.GetValueOrDefault(),
			modalData.SolidarBuilderComment ?? string.Empty,
			modalData.IsBillingRequested ?? false,
			modalData.HasAttachment));

		if (abortCommandResult.IsSuccess)
			await AccompanyingFileService.SendAccompanyingFileAbortMails(AccompanyingFileId);

		NotificationService.Notify(new NotificationMessage
		{
			Severity = abortCommandResult.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = abortCommandResult.Message,
			Duration = 4000
		});

		if (abortCommandResult.IsSuccess)
			await LoadAccompanyingFileAsync();
	}

	private async Task LoadAccompanyingFileAsync()
	{
		(ViewModel, var errorMessage) = new RealiseAndFollowPresenter().FromQuery(
				await QuerySender.Send(new GetAccompanyingFileForRealiseAndFollowMilestoneQuery(AccompanyingFileId, UserId, UserRole ?? string.Empty)))
			.Present();

		if (errorMessage != null)
		{
			NavigateToAccompanyingFileList();
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = errorMessage, Duration = 4000 });
		}
	}

	private void WorkParticipantsHaveChanged()
	{
		if (!SaveButtonIsActive)
		{
			SaveButtonIsActive = true;
			StateHasChanged();
		}
	}

	public Task<ReneeOperationResult<AISynthesisResult>> AnalyzeDossierWithAIAsync()
	{
      var aiDossierSynthesisService = ServiceProvider.GetService(typeof(IAIDossierSynthesisService)) as IAIDossierSynthesisService;

		if (aiDossierSynthesisService is null)
			return Task.FromResult(ReneeOperationResult<AISynthesisResult>.Failure("Le service d'analyse IA n'est pas disponible."));

		return aiDossierSynthesisService.AnalyzeAsync(AccompanyingFileId);
	}
}
