using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.Components;

public partial class SupportTeamCard
{
	[Inject] public IModalService ModalService { get; set; } = null!;

	[Parameter] public string? UserFullName { get; set; }
	[Parameter] public string? UserEmail { get; set; }
	[Parameter] public string? UserPhone { get; set; }
	[Parameter] public string UserRole { get; set; } = string.Empty;
	[Parameter] public bool IsEditable { get; set; }
	[Parameter] public EventCallback<string> OnShowUpdateSupportTeamMemberModal { get; set; }

	public async Task OnClickEditSupportTeamMemberIcon()
	{
		await OnShowUpdateSupportTeamMemberModal.InvokeAsync(UserRole);
	}
}