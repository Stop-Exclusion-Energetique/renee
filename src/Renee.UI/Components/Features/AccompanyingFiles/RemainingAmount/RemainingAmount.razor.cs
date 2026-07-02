using System.Security.Claims;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.AccompanyingFiles.RemainingAmount.PopupComponent;
using Renee.UI.Components.Features.AccompanyingFiles.RemainingAmount.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.RemainingAmount;

public partial class RemainingAmount
{
	[CascadingParameter(Name = "ZeeSpinner")] public ZeeSpinner? ZeeSpinner { get; set; }
	[CascadingParameter] public IModalService Modal { get; set; } = default!;

	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;
	[Inject] public IAirtableService AirtableService { get; set; } = null!;
	[Inject] public IJSRuntime JSRuntime { get; set; } = null!;

	public RemainingAmountViewModel ViewModel { get; set; } = new();
	
	private EditContext? _editContext;
	private string errorMessage = string.Empty;
	private bool showButton = true;
	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
	}

	private async Task CreateRemainingFoundrequestForAirtableAsync()
	{
		if (_editContext!.Validate())
		{
			try
			{
				showButton = false;
				ZeeSpinner?.DisplayLoading();

				var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
				var userName = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
				var userEmail = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
				var userReportingStructure =
					authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.GroupSid)?.Value;
				var userIdClaims = authState.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
					?.Value;

				if (Guid.TryParse(userIdClaims, out var userId))
				{
					var response = await AirtableService.AddRecordAsync(
						userName,
						userEmail,
						userReportingStructure,
						ViewModel.Reference!,
						userId,
						(double)ViewModel.EstimatedRemainingAmount!);

					if (response.IsSuccess)
					{
						showButton = true;
						ViewModel.CleanViewModel();
						ZeeSpinner?.HideLoading();

						Modal.Show<LinkPopupComponent>(
							Labels.RemainingAmountPopupTitle,
							parameters: new ModalParameters()
								.Add(nameof(LinkPopupComponent.Link), response.ReplyUrl),
							new ModalOptions
							{
								DisableBackgroundCancel = false,
								HideCloseButton = false,
								Position = ModalPosition.Middle,
								Size = ModalSize.Medium
							}
						);

						await JSRuntime.InvokeVoidAsync("window.open", response.ReplyUrl, "_blank");
						return;
					}

					if (string.Equals(response.Message, GenerateFilesLabels.Errors.AccompanyingFileNotFound) ||
						string.Equals(response.Message, Labels.Errors.UserNotAllowedToCreateAirtableRecord) ||
						string.Equals(response.Message, Labels.Errors.AccompanyingFileIsNotInThirdStage))
					{
						showButton = true;
						ZeeSpinner?.HideLoading();

						errorMessage = response.Message!;
						StateHasChanged();
						return;
					}

					NotificationService.Notify(
						new NotificationMessage
						{
							Severity = NotificationSeverity.Error,
							Summary = Labels.Errors.ErrorWhileCreatingRemainingAmount,
							Detail = response.Message,
							Duration = 4000
						});
				}
				else
				{
					NotificationService.Notify(
						new NotificationMessage
						{
							Severity = NotificationSeverity.Error,
							Summary = Labels.Errors.ErrorWhileCreatingRemainingAmount,
							Detail = Labels.Errors.SystemError,
							Duration = 4000
						});
				}

				
				showButton = true;
				ZeeSpinner?.HideLoading();
				StateHasChanged();
			}
			catch (Exception)
			{
				NotificationService.Notify(
					new NotificationMessage
					{
						Severity = NotificationSeverity.Error,
						Summary = Labels.Errors.ErrorWhileCreatingRemainingAmount,
						Detail = Labels.Errors.SystemError,
						Duration = 4000
					});
				ZeeSpinner?.HideLoading();
			}
		}
	}
}