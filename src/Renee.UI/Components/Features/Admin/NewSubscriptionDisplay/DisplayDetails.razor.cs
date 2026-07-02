using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.Territory;
using Renee.Application.DTOs.User;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.Features.Admin.NewSubscriptionDisplay.Component;

namespace Renee.UI.Components.Features.Admin.NewSubscriptionDisplay;

public partial class DisplayDetails
{
	[Parameter] public string? Id { get; set; }

	[CascadingParameter(Name = "ZeeSpinner")]
	public ZeeSpinner? ZeeSpinner { get; set; }
	[Inject] private NavigationManager? NavigationManager { get; set; }
	[Inject] private IModalService? ModalService { get; set; }
	[Inject] private IUserValidationService? UserValidationService { get; set; }
	[Inject] public ISendEventQuery sendEventQuery { get; set; } = default!;

	private UserDto? _user;

	private List<RoleDto>? Roles { get; set; }
	private List<TerritoryQueryObjectResult> Territories { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		if (Guid.TryParse(Id, out var id) && UserValidationService != null)
		{
			var userResult = await UserValidationService.GetUserById(id);
			if (userResult.IsSuccess)
				_user = userResult.Value;
		}

		await LoadData();
	}

	private async Task ConfirmNewUser()
	{
		ZeeSpinner?.DisplayLoading();
		if (_user is null) return;
		var isCreated = NavigationManager?.BaseUri != null &&
		                UserValidationService is not null &&
		                await UserValidationService.RegisterUserInApplication(
			                new RegisterUserCommand(_user), NavigationManager.BaseUri);
		if (isCreated) NavigationManager!.NavigateTo(Endpoints.NewSubscriptionDisplay, true);
		ZeeSpinner?.HideLoading();
	}

	private void DeleteConfirmation()
	{
		if (_user?.Email == null) return;
		var parameters = new ModalParameters { { "Id", _user.Id }, { "Email", _user.Email } };
		var modalOptions = new ModalOptions { Position = ModalPosition.Middle };
		ModalService?.Show<DeleteSubscriptionConfirmationModal>("", parameters, modalOptions);
	}

	private async Task LoadData()
	{
		var result = await sendEventQuery.Send(new GetInitialisationDataForUserCreationFormQuery());

		if (result.IsSuccess)
		{
			Territories = result.Value!.Territories;
			Roles = result.Value.Roles;
		}
	}
}