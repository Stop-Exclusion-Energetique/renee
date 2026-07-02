using System.Globalization;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.List.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

namespace Renee.UI.Components.Features.AccompanyingFiles.List.Components;

public partial class AccompanyingFileCard
{
	[Parameter] public CreatedAccompanyingFilesByUserViewModel ViewModel { get; set; } = null!;
	[Parameter] public Guid UserId { get; set; }
	[Parameter] public string? UserRole { get; set; }

	[Parameter] public EventCallback OnAccompanyingFileDeleted { get; set; }
	[Parameter] public EventCallback OnStateChanged { get; set; }
	[Parameter] public FilterContext SelectedFilterContext { get; set; }
	[Parameter] public bool IsDeadlineDateReached { get; set; }
	[Parameter] public bool IsInTZEEProgram { get; set; }

	[Inject] private NavigationManager NavigationManager { get; set; } = null!;
	[Inject] private IModalService ModalService { get; set; } = null!;

	private bool IsUserSolidarBuilder => UserRole!.Equals(Constants.SolidarBuilderRole);
	private bool CanActOnFile => IsUserAllowedToModifyDataFolder() && !IsUserSolidarBuilder;

	private bool ShouldAccompanyingFileBeFiledToAnah => 
		ViewModel is { ShouldAccompanyingFileBeSubmittedToAnah: true } && 
		ViewModel is
		(
			{ Stage: AccompanyingFileStage.Identify, Status: not AccompanyingFileStatus.Aborted } or
			{ Stage: AccompanyingFileStage.OrganizingAndFinancing, Status: AccompanyingFileStatus.InProgress }
		);

	public async Task StageValidation(bool isValidationPopUp)
	{
		var modal = ModalService.Show<StageValidationModal>(
			new ModalParameters
			{
				{ nameof(StageValidationModal.AccompanyingFileId), ViewModel.Id },
				{ nameof(StageValidationModal.IsValidationModal), isValidationPopUp },
				{ nameof(StageValidationModal.UserId), UserId }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true });

		await modal.Result;

		if (OnStateChanged.HasDelegate) await OnStateChanged.InvokeAsync();
	}

	private string? GetAccompanyingFileCardBackground()
	{
		if (!IsUserAllowedToModifyDataFolder()) return null;

		return SelectedFilterContext == FilterContext.WithoutSolidarBuilders
			? "without-solidar-builders"
			: "affected-folder";
	}

	private bool IsUserAllowedToModifyDataFolder() =>
		(
			UserId == ViewModel.SolidarBuilder ||
			UserId == ViewModel.SecondSolidarBuilder ||
			UserId == ViewModel.DiffuseCoordinator ||
			UserId == ViewModel.TargetedCoordinator ||
			UserId == ViewModel.TerritorialBuilder ||
			UserId == ViewModel.SecondTerritorialBuilder ||
			UserId == ViewModel.ThirdSolidarBuilder
		) || UserRole == Constants.AdminRole || UserRole == Constants.StructuralReferentRole;

	private bool IsAccompanyingFileWaitingForValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForApproval;
	private bool IsAccompanyingFileWaitingForAbortValidation() => ViewModel.Status == AccompanyingFileStatus.WaitingForAbortion;

	private static readonly CultureInfo FrenchCulture = CultureInfo.GetCultureInfo("fr-FR");

	private string? StageFacturationTooltip
	{
		get
		{
			if (string.IsNullOrWhiteSpace(ViewModel.StageFacturationLabel))
				return null;

			var details = new List<string> { ViewModel.StageFacturationLabel };

			if (!string.IsNullOrWhiteSpace(ViewModel.InvoiceNumber) && ViewModel.InvoiceAmount.HasValue && ViewModel.InvoiceDateUtc.HasValue)
				details.Add(string.Format(
					Labels.FacturationToolTipMessage,
					ViewModel.InvoiceNumber,
					ViewModel.InvoiceAmount.Value.ToString("N2", FrenchCulture),
					ViewModel.InvoiceDateUtc.Value.ToString("dd/MM/yyyy", FrenchCulture)
				));

			return string.Join(" • ", details);
		}
	}

	private async Task OnDeleteAccompanyingFile()
	{
		var modal = ModalService.Show<DeleteAccompanyingFileModal>(
			new ModalParameters {
				{ nameof(DeleteAccompanyingFileModal.AccompanyingFileId), ViewModel.Id },
				{ nameof(DeleteAccompanyingFileModal.CanNotBedeleted), CanNotBeDeleted() },
				{ nameof(DeleteAccompanyingFileModal.NoDeletionMessage), GetNoDeletionMessage() }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
		);

		var result = await modal.Result;
		if (result.Confirmed) await OnAccompanyingFileDeleted.InvokeAsync();
	}

	private bool CanNotBeDeleted() => IsUserSolidarBuilder && (ViewModel.Stage > AccompanyingFileStage.Identify || !string.IsNullOrWhiteSpace(ViewModel.StageFacturationLabel));
	private string? GetNoDeletionMessage()
	{
		if (!CanNotBeDeleted()) return null;

		if(ViewModel.Stage > AccompanyingFileStage.Identify && !string.IsNullOrWhiteSpace(ViewModel.StageFacturationLabel))
			return string.Format(
					Labels.CannotDeleteAccompanyingFileBecauseAStageIsValidatedAndItHasBillingInformation,
					ViewModel.StageFacturationLabel,
					ViewModel.InvoiceNumber,
					ViewModel.InvoiceDateUtc?.ToString("dd/MM/yyyy", FrenchCulture)
				);

		if (ViewModel.Stage > AccompanyingFileStage.Identify)
			return Labels.CannotDeleteAccompanyingFileBecauseAStageIsValidated;

		if (!string.IsNullOrWhiteSpace(ViewModel.StageFacturationLabel))
			return string.Format(
				Labels.CannotDeleteAccompanyingFileBecauseItHasBillingInformation,
				ViewModel.StageFacturationLabel,
				ViewModel.InvoiceNumber,
				ViewModel.InvoiceDateUtc?.ToString("dd/MM/yyyy", FrenchCulture)
			);

		return null;
	}

	private async Task OnEdit()
	{
		if (IsDeadlineDateReached && IsInTZEEProgram)
		{
			var modal = ModalService.Show<DeadlineReachedModal>(
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true }
			);

			await modal.Result;
		}
		else
		{
			NavigationManager.NavigateTo(ViewModel.EditUrl);
		}
	}

	private bool ShownSynthesisButton()
	{
		switch (ViewModel.Stage)
		{
			case > 0:
			case AccompanyingFileStage.Identify when ViewModel.Status != AccompanyingFileStatus.InProgress && ViewModel.Status != AccompanyingFileStatus.Rejected:
				return true;
			default: return false;
		}
	}

	private void AnalyzeToValidate()
	{
		NavigationManager.NavigateTo(ViewModel.SynthesisUrl);
	}
	
	private async Task ShowAbortRequestDetails()
	{
		var modal = ModalService.Show<AbortAccompanyingFileModalValidation>(
			new ModalParameters
			{
				{ nameof(AbortAccompanyingFileModalValidation.AccompanyingFileId), ViewModel.Id },
				{ nameof(AbortAccompanyingFileModalValidation.AbortReasonLabelId), ViewModel.AbortReasonLabelId },
				{ nameof(AbortAccompanyingFileModalValidation.AccompanyingFileReference), ViewModel.Reference },
				{ nameof(AbortAccompanyingFileModalValidation.IsBillingRequested), ViewModel.IsBillingRequested },
				{ nameof(AbortAccompanyingFileModalValidation.SolidarBuilderAbortRequestDetails), ViewModel.SolidarBuilderAbortRequestDetails }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = false, HideHeader = true, Size = ModalSize.Large }
		);

		var result = await modal.Result;

		if (result.Confirmed && OnStateChanged.HasDelegate)
			await OnStateChanged.InvokeAsync();
	}
}
