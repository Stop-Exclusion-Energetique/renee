using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.PreFinancingPlan;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.Result;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals;
using Renee.UI.Components.FormComponents;
using Renee.UI.Components.Layout.StageLayout;
using System.Security.Claims;
using Renee.Application.DTOs.AI;
using Renee.Domain.ReneeError;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage;

public partial class OrganizeAndFinance
{
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
    [Inject] private IServiceProvider ServiceProvider { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
	[Inject] public IJSRuntime? JsRuntime { get; set; }
	[Inject] private IModalService ModalService { get; set; } = null!;
	[Inject] private UnsavedChangesGuard UnsavedChangesGuard { get; set; } = null!;

	[Parameter] public Guid AccompanyingFileId { get; set; }

	private string RedirectTabUrl => $"{Endpoints.OrganizeAndFinanceStage}/{AccompanyingFileId}";

    public OrganizeAndFinanceViewModel? ViewModel { get; private set; } = new();

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

	public string UserRole { get; set; } = string.Empty;

	private Guid UserId { get; set; }

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
		UserRole == Constants.StructuralReferentRole || (UserRole == Constants.SolidarBuilderRole &&
		(ViewModel?.AccompanyingFileStage > AccompanyingFileStage.OrganizingAndFinancing || ViewModel?.AccompanyingFileStatus >= AccompanyingFileStatus.WaitingForApproval) &&
		ViewModel.IsInTzeeProgram);

    private bool IsSoliha { get; set; } = false;

	private bool IsLastTab => SelectedTab == 3;

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

		var value = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		UserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

		if (value != null)
			UserId = Guid.Parse(value);
		else
			throw new InvalidDataException("User Id not found in claims");

		UnsavedChangesGuard.UpdateCurrentUrl(NavigationManager.Uri);
		UnsavedChangesGuard.SetSaveHandler(SaveData);

		await LoadAccompanyingFileAsync();

		IsSoliha = ViewModel?.ReportingStructureName?.ToLowerInvariant().Contains(Labels.IsSolihaStructure, StringComparison.InvariantCultureIgnoreCase) == true && ViewModel?.IsImported == true;

		EditContext = new EditContext(ViewModel!);
		EditContext.OnFieldChanged += async (sender, args) => await HandleFieldChanged();
	}

	public async Task ChangeTab(int index)
	{
		if (SelectedTab == 2 && index > SelectedTab &&
			(ViewModel?.PreFinancingPlanViewModel.PreFinancingPlanBalancing < 0 ||
			ViewModel?.PreFinancingPlanViewModel.PreFinancingPlanBalancing > 0))
		{
			var modalResult = await ModalService.Show<FinancingDifferentialModal>(
				new ModalParameters
				{
					{ nameof(FinancingDifferentialModal.ModalText), Labels.PreFinancingPlanValidationWarning },
					{ nameof(FinancingDifferentialModal.Differential), ViewModel?.PreFinancingPlanViewModel.PreFinancingPlanBalancing }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }).Result;

			if (modalResult.Cancelled)
				return;
		}

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
            0 or 1 or 2 or 3 => $"{RedirectTabUrl}?SelectedTab={index}",
            _ => $"{RedirectTabUrl}"
        };
    }

    private async Task HandleFieldChanged()
	{
		if (SaveButtonIsActive) return;

		await UnsavedChangesGuard.SetUnsavedChangesState(true);
		SaveButtonIsActive = true;
	}

	private void WorkPackagesHaveChanged()
	{
		if (!SaveButtonIsActive)
		{
			SaveButtonIsActive = true;
		}
	}

	private void NavigateToAccompanyingFileList()
	{
		NavigationManager.NavigateTo(Endpoints.UserCreatedAccompanyingFiles);
	}

	private async Task<bool> SaveData()
	{
		try
		{
			var updateHousing = new UpdateHousing(
				ViewModel!.InitialHousingStateViewModel.RoomCounter,
				ViewModel.InitialHousingStateViewModel.DoorCounter,
				ViewModel.InitialHousingStateViewModel.WindowCounter,
				ViewModel.InitialHousingStateViewModel.PatioDoorCounter,
				ViewModel.InitialHousingStateViewModel.RoofWindowCounter,
				ViewModel.InitialHousingStateViewModel.BayWindowCounter,
				ViewModel.InitialHousingStateViewModel.CeilingHeight,
				ViewModel.InitialHousingStateViewModel.SunExposure);

			var updateHousingInitialState = new UpdatedHousingInitialStateForOrganizeAndFinanceMilestone(
				ViewModel.InitialHousingStateViewModel.HasPestOrMold,
				ViewModel.InitialHousingStateViewModel.HasFaultyElectricalSystem,
				ViewModel.InitialHousingStateViewModel.HasVentilationSystem,
				ViewModel.InitialHousingStateViewModel.HasHeatingSystem,
				ViewModel.InitialHousingStateViewModel.HasHotWaterProduction,
				ViewModel.InitialHousingStateViewModel.HasOpenings,
				ViewModel.InitialHousingStateViewModel.HasInsulation,
				ViewModel.InitialHousingStateViewModel.HasHousingCover,
				ViewModel.InitialHousingStateViewModel.DisordersObservedCommentary,
				ViewModel.PreWorkPlanTabViewModel.EstimatedEnergyDpeBeforeWork);

			var updateHousingAfterWorkState = new UpdateHousingAfterWorkState(
				ViewModel.PreWorkPlanTabViewModel.EstimatedAnnualEnergyConsumptionAfterWork,
				ViewModel.PreWorkPlanTabViewModel.EstimatedAnnualGhgEmissionsAfterWork,
				ViewModel.PreWorkPlanTabViewModel.EstimatedEnergyDpeAfterWork,
				ViewModel.PreWorkPlanTabViewModel.EstimatedEnergyGesAfterWork,
				ViewModel.PreWorkPlanTabViewModel.EstimatedEnergyClassJump);

			var updatePreWorkPlan = new UpdatePreWorkPlan(
				ViewModel.PreWorkPlanTabViewModel.RenovationType,
				ViewModel.PreWorkPlanTabViewModel.NextStepAndVigilancePoints,
				ViewModel.PreWorkPlanTabViewModel.NeedTemporaryRehousingSolution,
				ViewModel.PreWorkPlanTabViewModel.InterestInPossibleSupportedSelfRehabilitationAra,
				ViewModel.PreWorkPlanTabViewModel.AraOpeningStatementSent,
				ViewModel.PreWorkPlanTabViewModel.IsEmergencyWorks,
				ViewModel.PreWorkPlanTabViewModel.IsEnergeticsRenovationWorks,
				ViewModel.PreWorkPlanTabViewModel.IsInducedWorks,
				ViewModel.PreWorkPlanTabViewModel.IsSafetyAndHealthWorks,
				ViewModel.PreWorkPlanTabViewModel.TreatedAirTightness,
				ViewModel.PreWorkPlanTabViewModel.TreatedThermalBridge,
				ViewModel.PreWorkPlanTabViewModel.AreExistingHumidityAndVaporMigrationManagedAfterTreatment,
				ViewModel.PreWorkPlanTabViewModel.IsRgeLabelUpToDate,
				ViewModel.SupportedSelfRehabilitationViewModel.IsFamilyReadyForSupportedSelfRehabilitationApproach,
				ViewModel.SupportedSelfRehabilitationViewModel.FamilyPhysicalCapabilitiesHaveBeenTakenIntoAccount,
				ViewModel.SupportedSelfRehabilitationViewModel.FamilyCanMobilizeSocialCircleOnConstructionSite,
				ViewModel.SupportedSelfRehabilitationViewModel.WorkDetails,
				ViewModel.SupportedSelfRehabilitationViewModel
					.FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite,
				ViewModel.PreWorkPlanTabViewModel.PreWorkPlanProjectTypes ?? [],
				ViewModel.PreWorkPlanTabViewModel.PreWorkPlanInsuranceTypes ?? []);

			var updatePrefinancingPlan = new UpdatePreFinancingPlan(
				ViewModel.PreFinancingPlanViewModel.GuidedPathwayBonus,
				ViewModel.PreFinancingPlanViewModel.CoOwnershipBonus,
				ViewModel.PreFinancingPlanViewModel.DecentHousingBonus,
				ViewModel.PreFinancingPlanViewModel.AdaptationBonus,
				ViewModel.PreFinancingPlanViewModel.ExitEnergySieveBonus,
				ViewModel.PreFinancingPlanViewModel.Region,
				ViewModel.PreFinancingPlanViewModel.Department,
				ViewModel.PreFinancingPlanViewModel.PublicEstablishmentsIntercommunalCooperation,
				ViewModel.PreFinancingPlanViewModel.Municipality,
				ViewModel.PreFinancingPlanViewModel.BankLoanType,
				ViewModel.PreFinancingPlanViewModel.ClassicBankLoan,
				ViewModel.PreFinancingPlanViewModel.DepartmentalHouseForDisabledPersons,
				ViewModel.PreFinancingPlanViewModel.EnergySavingCertificates,
				ViewModel.PreFinancingPlanViewModel.HouseholdMaximumSavingAmountForRenovationProject,
				ViewModel.PreFinancingPlanViewModel.FamilyAllowanceFund,
				ViewModel.PreFinancingPlanViewModel.PensionFund,
				ViewModel.PreFinancingPlanViewModel.UnderprivilegedHousingFoundation,
				ViewModel.PreFinancingPlanViewModel.LeroyMerlinFoundation,
				ViewModel.PreFinancingPlanViewModel.WattForChangeFoundation,
				ViewModel.PreFinancingPlanViewModel.SocialProtectionGroup,
				ViewModel.PreFinancingPlanViewModel.StopEnergyExclusionFunds,
				ViewModel.PreFinancingPlanViewModel.FundingModes
					.Select(fm => new FundingModeDto(fm.Id, fm.Name, fm.Amount)).ToList(),
				ViewModel.PreFinancingPlanViewModel.MaximumAmountSupportFamilyMembersRenovationProject);

			var updatedWorkPackage = ViewModel.PreWorkPlanTabViewModel.WorkPackages.Select(
					wp => new UpdateWorkPackage(
						wp.Id ?? Guid.Empty,
						wp.EnergeticsEffectOfWorks,
						wp.WorkTypes.Select(wt => new UpdateWorkTypeCost(wt.Id, wt.Price, wt.Description)).ToList()))
				.ToList();

			var updatedFundingModes = ViewModel.PreFinancingPlanViewModel.FundingModes
				.Select(fm => new UpdateFundingModes(fm.Id, fm.Name ?? string.Empty, fm.Amount ?? 0)).ToList();

			var result = await AccompanyingFileService.UpdateAccompanyingFileForOrganizeAndFinanceMilestone(
				new SaveAccompanyingFileOrganizeAndFinanceMilestoneCommandInput(
					AccompanyingFileId,
					ViewModel.SupportedSelfRehabilitationViewModel.AccompanyingTimeDuration,
					updateHousing,
					updateHousingInitialState,
					updateHousingAfterWorkState,
					updatePreWorkPlan,
					updatePrefinancingPlan,
					updatedWorkPackage,
					updatedFundingModes,
					UserId,
					ViewModel.SupportedSelfRehabilitationViewModel.AnahFolderNumber,
					ViewModel.SupportedSelfRehabilitationViewModel.AnahFolderFilingDate));

			if (result.IsSuccess && result.Value) StageNavigationStateService.NotifyStateChanged();

			NotificationService.Notify(new NotificationMessage
			{
				Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
				Summary = result.Message,
				Duration = 4000
			});

			return result.IsSuccess && result.Value;
		}
		catch (Exception) { return false; }
		finally
		{
			await UnsavedChangesGuard.SetUnsavedChangesState(false);
			SaveButtonIsActive = false;
		}
	}

	private async Task SaveWithoutValidation()
	{
		try { await SaveData(); }
		catch (Exception)
		{
			// ignored
		}
	}

	private async Task SubmitWithValidation()
	{
		var formIsValid = ValidateEditContext();

		if (!formIsValid) return;

		if (ViewModel?.PreFinancingPlanViewModel.AbsoluteValueGapBetweenWorkPackagesAndPreFinancingPlan > 0)
		{
			var modalResult = await ModalService.Show<FinancingDifferentialModal>(
				new ModalParameters
				{
					{ nameof(FinancingDifferentialModal.ModalText), Labels.FinancingDifferentialModalText },
					{ nameof(FinancingDifferentialModal.Differential), ViewModel?.PreFinancingPlanViewModel.AbsoluteValueGapBetweenWorkPackagesAndPreFinancingPlan }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }).Result;

			if (modalResult.Cancelled)
				return;
		}

		var result = await SaveData();

		if (result)
			NavigationManager.NavigateTo(
				$"{Endpoints.OrganizeAndFinanceSynthesis}/{AccompanyingFileId}");
	}

	private bool ValidateEditContext()
	{
		return EditContext!.Validate();
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

		var saveResult = await SaveData();

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
		(ViewModel, var errorMessage) = new OrganizeAndFinancePresenter().FromQuery(
				await AccompanyingFileService.GetAccompanyingFileForOrganizeAndFinanceMilestone(AccompanyingFileId, UserId, UserRole))
			.Present();

		if (errorMessage != null)
		{
			NavigateToAccompanyingFileList();
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = errorMessage, Duration = 4000 });
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
