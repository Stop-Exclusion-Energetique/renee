using Blazored.Modal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Territory;
using Renee.UI.Components.Features.ProfileManagement.Modal.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.ProfileManagement.Modal;

public partial class UpdateProfileManagementModal
{
	[Parameter] public Guid UserId { get; set; }
	public EditContext? EditContext { get; set; }

	public UpdateProfileManagementViewModel UpdateProfileManagementViewModel { get; set; } = new();

	[Inject] public IUserService? UserService { get; set; }
	[Inject] public ISendEventQuery Sender { get; set; } = default!;

	[CascadingParameter] public BlazoredModalInstance ModalInstance { get; set; } = default!;
	private List<ZeeSelectItem<Guid?>>? Territories { get; set; }
	private UserDto? _user;

	protected override async Task OnInitializedAsync()
	{
		var territoriesResult = await Sender.Send(new GetAllTerritoriesQuery());
		if (territoriesResult.IsSuccess && territoriesResult.Value is not null)
			Territories = territoriesResult.Value.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId)).OrderBy(x => x.Label).ToList();

		if (UserService != null)
		{
			var userResult = await UserService!.GetRegisteredUserById(UserId);
			if (userResult.IsSuccess && userResult.Value is not null)
			{
				var user = userResult.Value;
				UpdateProfileManagementViewModel = new UpdateProfileManagementViewModel
				{
					TerritoryId = user.TerritoryId,
					Email = user.Email,
					LastName = user.LastName,
					FirstName = user.FirstName,
					PhoneNumber = user.PhoneNumber,
					SiretNumber = user.SiretNumber
				};

				EditContext = new EditContext(UpdateProfileManagementViewModel);
				_user = user;
			}
		}
	}

	public async Task Cancel() => await ModalInstance.CloseAsync();

	private async Task HandleValidSubmit()
	{
		if (UserService is null || _user is null) return;

		var userDto = new UserDto(
			UserId,
			new UserPersonnalInformations(
				UpdateProfileManagementViewModel.Email,
				UpdateProfileManagementViewModel.FirstName,
				UpdateProfileManagementViewModel.LastName,
				UpdateProfileManagementViewModel.PhoneNumber,
				UpdateProfileManagementViewModel.SiretNumber),
				UpdateProfileManagementViewModel.TerritoryId,
				null
			);

		if (UpdateProfileManagementViewModel.Email != _user.Email && _user.Email != null)
		{
			var adUpdateResult = await UserService.UpdateUserAD(userDto, _user.Email);
			if (!adUpdateResult.IsSuccess || !adUpdateResult.Value) return;
		}

		var result = await UserService.UpdateUser(userDto);
		if (result.IsSuccess && result.Value) await ModalInstance.CloseAsync();

		NotificationService.Notify(new NotificationMessage
		{
			Severity = result.IsSuccess ? NotificationSeverity.Success : NotificationSeverity.Error,
			Summary = result.Message,
			Duration = 4000,
		});
	}
}