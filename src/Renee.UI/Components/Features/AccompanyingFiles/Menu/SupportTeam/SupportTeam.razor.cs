using Microsoft.AspNetCore.Components;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.SupportTeam;

public partial class SupportTeam
{
	[Parameter] public IReadOnlyList<SupportTeamMemberViewModel> SupportTeamMembers { get; set; } = [];

	[Parameter] public EventCallback<string> OnShowUpdateSupportTeamMemberModal { get; set; }
}