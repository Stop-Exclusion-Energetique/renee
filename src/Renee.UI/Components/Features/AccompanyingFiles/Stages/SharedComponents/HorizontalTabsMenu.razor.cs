using Microsoft.AspNetCore.Components;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents;

public partial class HorizontalTabsMenu
{
	[Parameter] public List<string> TabsList { get; set; } = [];

	[Parameter] public int SelectedTab { get; set; }

	[Parameter] public EventCallback<int> OnChangeTab { get; set; } = default!;
}