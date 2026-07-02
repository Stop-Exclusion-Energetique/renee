using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectEnd;

public partial class ProjectEnd
{
	[Parameter] public ProjectEndViewModel ViewModel { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<AccompanyingTimeDuration?>>? AccompanyingTimeDurations { get; set; }

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

    private EditContext? _editContext;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
		_editContext.OnFieldChanged += HandleFieldChanged;
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) FormEditContext.NotifyFieldChanged(e.FieldIdentifier);
	}
}