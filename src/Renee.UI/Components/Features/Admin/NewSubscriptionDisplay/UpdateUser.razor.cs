using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.UI.Components.Features.Admin.NewSubscriptionDisplay.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.Admin.NewSubscriptionDisplay;

public partial class UpdateUser
{
	private EditContext? _editContext;
	private List<ZeeSelectItem<Guid?>>? Roles { get; set; }
	private List<ZeeSelectItem<Guid?>>? ReportingStructures { get; set; }


	[Parameter] public UserDetailsViewModel UserDetailsViewModel { get; set; } = new ();
	[CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; } = default!;
	
	[Inject] public IUserService? UserService { get; set; }
	[Inject] public ISendEventQuery sendEventQuery { get; set; } = default!;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(UserDetailsViewModel);

		await LoadData();
	}

	public async Task OnValidSubmit()
	{
		if(_editContext!.Validate() && UserService is not null)
		{
			var result = await UserService.UpdateUserByAdmin(
					new UpdateUserCommandInput(
						UserDetailsViewModel.UserId,
						UserDetailsViewModel.LastName!,
						UserDetailsViewModel.FirstName!,
						(Guid)UserDetailsViewModel.UserRoleId!,
						(Guid)UserDetailsViewModel.ReportingStructureId!
					)
				);

			if (result.IsSuccess)
			{
				await ModalInstance.CloseAsync(ModalResult.Ok());
			}

			NotificationService.Notify(new NotificationMessage
			{
				Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
				Summary = result.Message,
				Duration = 4000
			});
		}
	}

	private async Task LoadData()
	{
		var result = await sendEventQuery.Send(new GetInitialisationDataForUserCreationFormQuery());

		if (result.IsSuccess)
		{
			ReportingStructures = result.Value!.ReportingStructures.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id)).ToList(); ;
			Roles = result.Value.Roles.Select(x => new ZeeSelectItem<Guid?>(x.LongName, x.Id)).ToList(); ;
		}
	}
}
