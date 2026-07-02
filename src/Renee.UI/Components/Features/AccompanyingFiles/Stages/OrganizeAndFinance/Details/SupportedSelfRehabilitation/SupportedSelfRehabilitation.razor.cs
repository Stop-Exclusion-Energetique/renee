using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.SupportedSelfRehabilitation.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.SupportedSelfRehabilitation;

public partial class SupportedSelfRehabilitation
{
	[Parameter] public SupportedSelfRehabilitationViewModel ViewModel { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<AccompanyingTimeDuration?>>? AccompanyingTimeDurations { get; set; }

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;
	private readonly HashSet<object> _validatedObjects = [];

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }


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