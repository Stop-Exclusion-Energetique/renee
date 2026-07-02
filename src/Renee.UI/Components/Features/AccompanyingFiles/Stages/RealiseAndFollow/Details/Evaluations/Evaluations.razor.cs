using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.Evaluations.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.Evaluations;

public partial class Evaluations
{
	[Parameter] public EvaluationsViewModel ViewModel { get; set; } = null!;

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

    private readonly List<ZeeSelectItem<int?>> _notations =
	[
		new("0", 0), new("1", 1), new("2", 2), new("3", 3), new("4", 4), new("5", 5), new("6", 6), new("7", 7),
		new("8", 8), new("9", 9), new("10", 10)
	];

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