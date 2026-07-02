using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Interfaces;
using Renee.UI.Components.Features.ProfileManagement.Modal.ViewModel;

namespace Renee.UI.Components.Features.ProfileManagement.Modal;

public partial class DeleteAccountModal
{
	[Inject] public IUserService UserService { get; set; } = null!;

	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
	[Parameter] public Guid UserId { get; set; }

	public DeleteAccountModalViewModel ViewModel { get; set; } = new();
	private EditContext EditContext { get; set; } = default!;

	protected override void OnInitialized()
	{
		EditContext = new EditContext(ViewModel);
	}

	public async Task OnValueChanged()
	{
		if (ViewModel.IsAccountDeletionConfirmed == true)
		{
			var result = await UserService.UpdateUserAccountDeletionRequestState(UserId, true);
			if (result.IsSuccess && result.Value) 
			{
				await BlazoredModal.CloseAsync(ModalResult.Ok());
			}
		}

		if (ViewModel.IsAccountDeletionConfirmed == false)
		{
			await BlazoredModal.CloseAsync(ModalResult.Cancel());
		}
	}
}