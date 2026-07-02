using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.HousingInitialState.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.HousingInitialState;

public partial class InitialHousingState
{
	public readonly List<ZeeSelectItem<SunExposure?>> SunExposureList =
	[
		new(Labels.West, SunExposure.West), new(Labels.NorthWest, SunExposure.NorthWest),
		new(Labels.North, SunExposure.North), new(Labels.NorthEast, SunExposure.NorthEast),
		new(Labels.East, SunExposure.East), new(Labels.SouthEast, SunExposure.SouthEast),
		new(Labels.South, SunExposure.South), new(Labels.SouthWest, SunExposure.SouthWest)
	];

	[Parameter] public InitialHousingStateViewModel ViewModel { get; set; } = null!;

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;
	[CascadingParameter(Name ="IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha {  get; set; }
	private readonly HashSet<object> _validatedObjects = [];

	private EditContext? _editContext;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
		FormEditContext.OnValidationRequested += EditContext_OnValidationRequested;
		_editContext.OnFieldChanged += HandleFieldChanged;
	}

	private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		if (_validatedObjects.Contains(ViewModel)) return;
		_editContext?.Validate();
		_validatedObjects.Add(ViewModel);
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) FormEditContext.NotifyFieldChanged(e.FieldIdentifier);
	}
}