using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Task;
using Renee.Application.Queries.Territory;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Billing.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.Components.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Tasks.Component.Modal;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu;

public partial class AccompanyingFileMenu
{
	public AccompanyingFileMenuViewModel? ViewModel { get; set; }

	[Parameter] public Guid AccompanyingFileId { get; set; } = Guid.Empty;

	[Inject] public IModalService ModalService { get; set; } = null!;

	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

	[Inject] public ITaskService TaskService { get; set; } = null!;

	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public ISendEventQuery Sender { get; set; } = null!;
	[Inject] public IAccompanyingFileService AccompanyingFileService { get; set; } = null!;
	[Inject] private IJSRuntime JSRuntime { get; set; } = null!;
	[Inject] public IFileService FileService { get; set; } = null!;

	private List<ZeeSelectItem<Guid?>>? Territory { get; set; }

	private Guid _userId;
	private string _userRole = string.Empty;
	private EditContext _targetInformationEditContext = null!;
	private EditContext _billingLogEditContext = null!;
	private bool _showSubmitButtonForTargetInformation;
	private bool _showSubmitButtonForFirstFacturationData;
	private bool _showSubmitButtonForSecondFacturationData;
	private bool _showSubmitButtonForThirdFacturationData;
	private string _optionalDocumentBlobName = string.Empty;
	private readonly List<string> _errorMessages = [];

	protected override async Task OnInitializedAsync()
	{
		var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;
		var userIdClaims = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var role = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
		var territoriesResult = await Sender.Send(new GetAllTerritoriesQuery());
		if (territoriesResult.IsSuccess && territoriesResult.Value is not null)
			Territory = territoriesResult.Value.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId)).ToList();

		if (Guid.TryParse(userIdClaims, out var userId) && role != null)
		{
			_userId = userId;
			_userRole = role;
		}

		await TaskService.ActivatePendingTasks(AccompanyingFileId);

		var menuQueryResult = await SendEventQuery.Send(
			new GetAllAccompanyingFileTaskAndSupportTeamQuery
			{
				AssociatedResourceId = AccompanyingFileId,
				UserId = _userId
			});

		if (!menuQueryResult.IsSuccess || menuQueryResult.Value is null)
		{
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = menuQueryResult.Message, Duration = 4000 });
			return;
		}

		ViewModel = new AccompanyingFileMenuPresenter().FromQuery(menuQueryResult.Value).Present();

		_optionalDocumentBlobName = $"{ViewModel.AccompanyingFileReference}_optionnel";

		await LoadDocumentsAsync();

		_targetInformationEditContext = new EditContext(ViewModel.TargetInformation ?? new AccompanyingFileTargetDetailsViewModel());
		_billingLogEditContext = new EditContext(ViewModel.BillingLog ?? new BillingLogViewModel());
	}

	public async Task DeleteTask(Guid taskId)
	{
		await TaskService.DeleteTask(taskId, _userId);

		await LoadData();
	}

	public async Task ShowAddTaskModal()
	{
		var modal = ModalService.Show<AddTaskModal>(
			new ModalParameters
			{
				{ nameof(AddTaskModal.AccompanyingFileId), AccompanyingFileId },
				{ nameof(AddTaskModal.UserId), _userId },
				{ nameof(AddTaskModal.IsUpdateModal), false },
				{ nameof(AddTaskModal.TaskId), null }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });

		var result = await modal.Result;
		if (result.Confirmed) await LoadData();
	}

	public async Task ShowUpdateSupportTeamMemberModal(string supportTeamMemberRole)
	{
		var modal = ModalService.Show<UpdateSupportTeamMemberModal>(
			new ModalParameters
			{
				{ nameof(UpdateSupportTeamMemberModal.ConnectedUserId), _userId },
				{ nameof(UpdateSupportTeamMemberModal.AssociatedResourceId), AccompanyingFileId },
				{ nameof(UpdateSupportTeamMemberModal.SupportTeamMemberRole), supportTeamMemberRole },
				{ nameof(UpdateSupportTeamMemberModal.IsCoproperty), false }
			},
			new ModalOptions
			{
				Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true, Size = ModalSize.Large
			});

		var result = await modal.Result;
        if (result.Confirmed) await LoadData();

    }

	public async Task ShowUpdateTaskModal(Guid taskId)
	{
		var modal = ModalService.Show<AddTaskModal>(
			new ModalParameters
			{
				{ nameof(AddTaskModal.AccompanyingFileId), AccompanyingFileId },
				{ nameof(AddTaskModal.UserId), _userId },
				{ nameof(AddTaskModal.IsUpdateModal), true },
				{ nameof(AddTaskModal.TaskId), taskId }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true });

		var result = await modal.Result;
		if (result.Confirmed) await LoadData();
	}

	private async Task LoadData()
	{
		await TaskService.ActivatePendingTasks(AccompanyingFileId);

		var result = await SendEventQuery.Send(
			new GetAllAccompanyingFileTaskAndSupportTeamQuery
			{
				AssociatedResourceId = AccompanyingFileId,
				UserId = _userId
			});

		if (!result.IsSuccess || result.Value is null)
		{
			NotificationService.Notify(new NotificationMessage { Severity = NotificationSeverity.Error, Summary = result.Message, Duration = 4000 });
			return;
		}

		ViewModel = new AccompanyingFileMenuPresenter().FromQuery(result.Value).Present();

		StateHasChanged();
	}

	private void HandleTargetInformationDataChange()
	{
		_showSubmitButtonForTargetInformation = true;
	}

	private void HandleFacturationDataChange(FacturationStage stage)
	{
		switch (stage)
		{
			case FacturationStage.First:
				_showSubmitButtonForFirstFacturationData = true;
				break;
			case FacturationStage.Second:
				_showSubmitButtonForSecondFacturationData = true;
				break;
			case FacturationStage.Third:
				_showSubmitButtonForThirdFacturationData = true;
				break;
		}
	}

	private async Task SubmitTargetInformation()
	{
		var result = await AccompanyingFileService.UpdateAccompanyingFileTargetInformations(
			new UpdateAccompanyingFileTargetInformationsCommandInput(
				AccompanyingFileId,
				ViewModel!.TargetInformation!.GeographicalHousingAreaTypology!.Value,
				ViewModel.TargetInformation.AccompanyingType,
				ViewModel.TargetInformation.AccompanyingTerritory
			)
		);

		_showSubmitButtonForTargetInformation = false;

		NotificationService.Notify(new NotificationMessage
		{
			Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = result.Message,
			Duration = 4000
		});


		if (result.IsSuccess)
			await LoadData();
	}

	private async Task SubmitFacturationData()
	{
		var billingLog = ViewModel?.BillingLog;

		if (billingLog == null)
			return;

		var result = await AccompanyingFileService.UpdateAccompanyingFileFacturationInformations(
			new UpdateAccompanyingFileFacturationInformationsCommandInput(
				AccompanyingFileId,
				billingLog.BilledJalon1,
				billingLog.AmountBilledFirstStage,
				billingLog.FundraisingLauchDateForFirstStage,
				billingLog.BillingCallNumberFirstStage,
				billingLog.InvoiceNumberFirstStage,
				billingLog.BillingDateFirstStage,
				billingLog.BilledJalon2,
				billingLog.AmountBilledSecondStage,
				billingLog.FundraisingLauchDateForSecondStage,
				billingLog.BillingCallNumberSecondStage,
				billingLog.InvoiceNumberSecondStage,
				billingLog.BillingDateSecondStage,
				billingLog.BilledJalon3,
				billingLog.AmountBilledThirdStage,
				billingLog.FundraisingLauchDateForThirdStage,
				billingLog.BillingCallNumberThirdStage,
				billingLog.InvoiceNumberThirdStage,
				billingLog.BillingDateThirdStage)
		);

		ResetFacturationSubmitButtons();

		NotificationService.Notify(new NotificationMessage
		{
			Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = result.Message,
			Duration = 4000
		});

		if (result.IsSuccess)
			await LoadData();
	}

	private void ResetFacturationSubmitButtons()
	{
		_showSubmitButtonForFirstFacturationData = false;
		_showSubmitButtonForSecondFacturationData = false;
		_showSubmitButtonForThirdFacturationData = false;
	}

	public async Task ViewFile(string fileName, byte[]? fileData)
	{
		if (fileData != null)
		{
			var base64 = Convert.ToBase64String(fileData);

			var mimeType = "application/octet-stream";
			if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) mimeType = "application/pdf";
			if (fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
			if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) mimeType = "image/jpeg";
			if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) mimeType = "image/png";
			if (fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)) mimeType = "image/webp";

			await JSRuntime.InvokeVoidAsync("viewFile", base64, mimeType, fileName);
		}
	}

	private bool FacturationFieldShouldBeDisabled() => !(_userRole == Constants.AdminRole ||
		_userRole == Constants.TerritorialBuilderRole ||
		_userRole == Constants.DiffuseCoordinatorRole ||
		_userRole == Constants.TargetedCoordinatorRole);

	public async Task OnChangeUpload(InputFileChangeEventArgs e)
	{
		_errorMessages.Clear();
		var quota = 10;
		var remainingSlots = quota - ViewModel?.Documents.Count(d => d.IsOptionnal) ?? 0;

		if (remainingSlots <= 0)
		{
			_errorMessages.Add("Le quota de 10 documents optionnels est déjà atteint.");
			return;
		}

		var files = e.GetMultipleFiles(int.MaxValue);
		if (files.Count > remainingSlots)
		{
			_errorMessages.Add(
				$"Vous avez sélectionné {files.Count} fichiers, " +
				$"mais il ne reste que {remainingSlots} emplacement(s) disponible(s). " +
				$"Seuls les {remainingSlots} premiers seront traités."
			);
		}

		var filesToProcess = files.Take(remainingSlots);

		foreach (var file in filesToProcess) 
		{
			if (file is null)
				return;

			if (file.Size > Constants.MaxFileSize)
			{
				_errorMessages.Add(string.Format("Le fichier {0} dépasse la taille maximale de 10 Mo.", file.Name));
				continue;
			}

			var fileExtension = GetFileExtension(file.Name).ToLowerInvariant();

			var allowedExtensions = new HashSet<string> { "pdf", "png", "jpeg", "jpg", "webp" };
			if (!allowedExtensions.Contains(fileExtension))
			{
				_errorMessages.Add(string.Format("Le fichier {0} n’est pas dans un format valide. Veuillez sélectionner un fichier au format PDF, PNG, JPG, JPEG, WEBP.", file.Name));
				continue;
			}

			var memoryStreamFile = new MemoryStream();
			var fileName = $"{_optionalDocumentBlobName}_{file.Name}";
			await file.OpenReadStream(Constants.MaxFileSize).CopyToAsync(memoryStreamFile);

			memoryStreamFile.Position = 0;

			await FileService.UploadFileAsync(fileName, fileName, memoryStreamFile, _userId);
		}

		await LoadDocumentsAsync();
	}

	private async Task OnClickDeleteFile(string fileName)
	{
		var modalResult =
			await ModalService.Show<FileDeletionModal>(
				new ModalParameters
				{
					{ nameof(FileDeletionModal.FileName), fileName }
				},
				new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true, HideHeader = true }
			).Result;

		if (modalResult.Cancelled)
			return;

		await LoadDocumentsAsync();
	}

	private static string GetFileExtension(string fileName) =>
		fileName.Split('.')[^1];

	private async Task LoadDocumentsAsync()
	{
		if (ViewModel == null)
			return;

		var docs = (await FileService.GetAllDocumentsAsync(ViewModel.AccompanyingFileReference))
			.Select(d => new DocumentViewModel
			{
				Name = d.Name,
				Stream = d.FileStream,
				UploadedAt = d.UploadedAt,
				IsOptionnal = d.IsOptionnal,
				Data = d.Data,
				AccompanyingFileStage = d.AccompanyingFileStage,
				Author = d.Author
			})
			.ToList();

		ViewModel.Documents = docs;
	}

	private async Task OnViewFileRequested((string FileName, byte[]? Data) fileInfo)
	{
		await ViewFile(fileInfo.FileName, fileInfo.Data);
	}
}