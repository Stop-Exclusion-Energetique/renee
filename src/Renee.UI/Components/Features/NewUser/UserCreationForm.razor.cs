using System.Linq.Expressions;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Commands.User;
using Renee.Application.DTOs.ReportingStructure;
using Renee.Application.DTOs.User;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.UI.Components.DisplayComponents;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.NewUser;

public partial class UserCreationForm
{
	public UserViewModel UserViewModel { get; } = new();
	[Inject] public NavigationManager? NavigationManager { get; set; }
	[Inject] public IUserValidationService? UserValidationService { get; set; }
	[Inject] public IUserService? UserService { get; set; }
	[Inject] public IModalService? ModalService { get; set; }

	[Inject] ICguVersionService CguVersionService { get; set; } = null!;
	[Inject] public IFileViewerService FileViewer { get; set; } = null!;

	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

	[CascadingParameter(Name = "ZeeSpinner")]
	public ZeeSpinner? ZeeSpinner { get; set; }
	private EditContext? _editContext;

	private List<ZeeSelectItem<Guid?>>? Roles { get; set; }

	public List<ZeeSelectItem<Guid?>>? ReportingStructures { get; set; }

	private List<ZeeSelectItem<Guid?>>? Territories { get; set; }

	private string? LabelCGU { get; set; }
	private string? VersionCGU { get; set; }

	private bool IsMailSendWithSuccess { get; set; }

	private string? RandomCode { get; set; }
	private bool IsCorrectPassword { get; set; } = true;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(UserViewModel);

		await LoadFormData();
		ZeeSpinner?.HideLoading();

		var cguVersionResult = await CguVersionService.GetLatestVersionAsync();
		if (cguVersionResult.IsSuccess)
		{
			LabelCGU = cguVersionResult.Value?.Label;
			VersionCGU = cguVersionResult.Value?.Version;
		}
	}

	public void OnNewReportingStructureSelected()
	{
		UserViewModel.SelectedReportingStructureName = ReportingStructures
			?.Find(es => es.Value.Equals(UserViewModel.SelectedReportingStructureId))?.Label;
	}

	private static FieldIdentifier GetFieldIdentifierForProperty<TValue>(Expression<Func<TValue>> expression) =>
		FieldIdentifier.Create(expression);

	private async Task OnEmailInputChange()
	{
		if (_editContext is null || string.IsNullOrEmpty(UserViewModel.Email)) return;

		try
		{
			if (UserService != null && UserValidationService != null)
			{
				var isEmailAlreadyUsed = (await UserService.VerifyDuplicatedEmailAsync(UserViewModel.Email)).Value |
										 (await UserValidationService.VerifyDuplicatedEmailAsync(UserViewModel.Email)).Value;

				if (isEmailAlreadyUsed == true) ShowZeeErrorModal(Labels.Errors.UserDuplicateEmail);
			}
		}
		catch
		{
			// ignored
		}
	}

	private async Task OnSubmit()
	{
		if (_editContext is null) return;
		ZeeSpinner?.DisplayLoading();
		if (UserViewModel.Password != RandomCode)
		{
			IsCorrectPassword = false;
			ZeeSpinner?.HideLoading();
			return;
		}

		IsCorrectPassword = true;

		if (!_editContext.Validate()) return;
		if (_editContext.Model is not UserViewModel vm) return;

		if (vm is { AskedRole: not null })
		{
			var userDto = new UserDto(Guid.NewGuid(), DateTime.UtcNow, vm.LastName, vm.FirstName, vm.Email);

			if (UserViewModel.SelectedReportingStructureName != null)
			{
				userDto = userDto.SetDataForSignIn(
					vm.AskedRole.Value,
					vm.Function,
					new ReportingStructureDto(
						vm.SelectedReportingStructureId ?? Guid.NewGuid(),
						UserViewModel.SelectedReportingStructureName,
						null),
					vm.OtherReportingStructure,
					vm.TerritoryId);
				userDto = userDto.SetPersonnalInformations(
					vm.PhoneNumber,
					vm.SiretNumber);

				userDto = userDto.SetCguVersion(VersionCGU);

				var userCreateWithSuccess = UserValidationService != null &&
											await UserValidationService.CreateUser(new CreateUserCommand(userDto));

				if (userCreateWithSuccess) NavigationManager?.NavigateTo(Endpoints.Validation);
			}
		}

		ZeeSpinner?.HideLoading();
	}

	private async Task SendValidationEmail()
	{
		if (_editContext is not null && _editContext.Validate())
		{
			RandomCode = StringHelper.GenerateRandomString();
			if (UserValidationService != null)
				IsMailSendWithSuccess = await UserValidationService.SendConfirmation(UserViewModel.Email, RandomCode);
		}
	}

	private void ShowZeeErrorModal(string message)
	{
		var parameters = new ModalParameters { { "Message", message } };
		var modalOptions = new ModalOptions { Position = ModalPosition.Middle };
		ModalService?.Show<ZeeErrorModal>("", parameters, modalOptions);
	}

	private async Task LoadFormData()
	{
		var result = await SendEventQuery.Send(new GetInitialisationDataForUserCreationFormQuery());

		if (result.IsSuccess)
		{
			Roles = [.. result.Value!.Roles.Select(x => new ZeeSelectItem<Guid?>(x.LongName, x.Id))];
			Territories = [.. result.Value.Territories
				.Select(x => new ZeeSelectItem<Guid?>(x.TerritoryName, x.TerritoryId)).OrderBy(x => x.Label)];
			ReportingStructures = [.. result.Value.ReportingStructures.Select(x => new ZeeSelectItem<Guid?>(x.Name, x.Id))];
		}
	}
}