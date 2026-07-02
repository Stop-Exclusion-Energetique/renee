using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Queries.User;
using Renee.UI.Components.Features.Admin.DeletedAccountManagement.Modal;
using Renee.UI.Components.Features.Admin.DeletedAccountManagement.Presenter;
using Renee.UI.Components.Features.Admin.DeletedAccountManagement.ViewModel;

namespace Renee.UI.Components.Features.Admin.DeletedAccountManagement;

public partial class DeletedAccountManagement
{
	[Inject] ISendEventQuery Sender { get; set; } = default!;
	[Inject] IModalService ModalService { get; set; } = default!;

	private List<DeletedAccountManagementViewModel> AccountDeletionRequests { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		await LoadAccountDeletionRequests();
	}

	private async Task ValidateAccountDeletionRequest(Guid userId)
	{
		var modal = ModalService.Show<DeletedAccountManagementModal>(
			new ModalParameters
			{
				{ nameof(DeletedAccountManagementModal.UserId), userId }
			},
			new ModalOptions { Position = ModalPosition.Middle, HideCloseButton = true });

		var result = await modal.Result;

		if (result.Confirmed)
		{
			await LoadAccountDeletionRequests();
		}
	}

	public async Task LoadAccountDeletionRequests()
	{
		AccountDeletionRequests = (new DeletedAccountManagementPresenter().FromQuery(
			(await Sender.Send(new GetAllUntrackedAccountDeletionRequestsForAdminQuery())).Value!)).Present();
		StateHasChanged();
	}
}