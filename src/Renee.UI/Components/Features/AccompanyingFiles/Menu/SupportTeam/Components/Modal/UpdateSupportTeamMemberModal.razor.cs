using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Application.Queries.User;
using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.Components.Modal.ViewModel;
using Renee.UI.Components.FormComponents;
using System.Security.Claims;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.Components.Modal;

public partial class UpdateSupportTeamMemberModal
{
	[Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;
	[Inject] public ISupportTeamService SupportTeamService { get; set; } = null!;
	[Inject] public AuthenticationStateProvider AuthenticationStateProvider { get; set; } = null!;

	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
	[Parameter] public Guid AssociatedResourceId { get; set; }
	[Parameter] public Guid ConnectedUserId { get; set; }
	[Parameter] public string? SupportTeamMemberRole { get; set; }
	[Parameter] public bool? IsCoproperty { get; set; }

	public UpdateSupportTeamMemberViewModel ViewModel { get; set; } = new();
	public EditContext? EditContext { get; set; }
	private List<ZeeSelectItem<Guid?>> Users { get; set; } = [];

	protected override async Task OnInitializedAsync()
	{
		EditContext = new EditContext(ViewModel);

		if (SupportTeamMemberRole != null) 
		{
			var claims = (await AuthenticationStateProvider.GetAuthenticationStateAsync()).User.Claims;

			var connectedUserRole = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? string.Empty;

			var usersResult = await SendEventQuery
							.Send(
								new GetAllUsersBasedOnRoleQuery(
									ConnectedUserId, 
									GetRealRoleName(SupportTeamMemberRole), 
									IsUserTeritorialBuilderOrCoordinator(connectedUserRole)
								)
							);
			if (usersResult.IsSuccess && usersResult.Value is not null)
				Users = usersResult.Value.Select(qr => new ZeeSelectItem<Guid?>(qr.FullName, qr.Id)).ToList();
		}
	}

	private async Task Close() => await BlazoredModal.CloseAsync(ModalResult.Cancel());

	private async Task HandleSubmit()
	{
		var commandInput = new UpdateSupportTeamMemberCommandInput(
			AssociatedResourceId,
			SupportTeamMemberRole ?? string.Empty,
			ViewModel.AssignedTo,
			IsCoproperty ?? false);

		var result = await SupportTeamService.UpdateSupportTeam(commandInput);
		if (result.IsSuccess && result.Value != -1) 
			await BlazoredModal.CloseAsync(ModalResult.Ok());
	}

	private static string GetRealRoleName(string formatedRoleName) =>
	formatedRoleName switch
	{
		var role when role.Contains(Labels.ReferentSolidarBuilder) => Constants.SolidarBuilderRole,
		Labels.ReferentEt => Constants.TerritorialBuilderRole,
        Labels.SecondReferentEt => Constants.TerritorialBuilderRole,
        Labels.ReferentDiffuseCoordinator => Constants.DiffuseCoordinatorRole,
		Labels.ReferentTargetedCoordinator => Constants.TargetedCoordinatorRole,
		Labels.TrustedTier => Constants.TrustedTierRole,
		_ => string.Empty
	};

	private static bool IsUserTeritorialBuilderOrCoordinator(string role)
	=> role.Equals(Constants.TerritorialBuilderRole) ||
		role.Equals(Constants.DiffuseCoordinatorRole) ||
		role.Equals(Constants.TargetedCoordinatorRole);
}