using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Helpers;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.EnergeticProfil;

public partial class EnergyProfile
{
	[CascadingParameter] public EditContext EditContext { get; set; } = null!;
	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

    [Parameter] public EnergyProfileViewModel ViewModel { get; set; } = null!;
	[Parameter] public bool IsLinkedToCoproperty { get; set; }

	[Parameter] public EventCallback<bool> OnValidation { get; set; }
	private static readonly List<ZeeSelectItem<EnergyDeprivation?>> PartialTotalList =
	[
		new(EnergyDeprivation.None.GetDescription(), EnergyDeprivation.None),
		new(EnergyDeprivation.Partial.GetDescription(), EnergyDeprivation.Partial),
		new(EnergyDeprivation.Total.GetDescription(), EnergyDeprivation.Total)
	];

	private readonly HashSet<object> _validatedObjects = [];
	private EditContext? _editContext;
	private bool AreCopropertyManagedEnergyFieldsDisabled => IsUserAllowedToEdit || IsSoliha || IsLinkedToCoproperty;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
		EditContext.OnValidationRequested += EditContext_OnValidationRequested;
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
		if (sender == _editContext) EditContext.NotifyFieldChanged(e.FieldIdentifier);
	}
}