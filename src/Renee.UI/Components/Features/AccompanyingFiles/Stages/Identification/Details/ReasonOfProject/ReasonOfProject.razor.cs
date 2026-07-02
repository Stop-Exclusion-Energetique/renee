using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.ReasonOfProject;

public partial class ReasonOfProject
{
	[CascadingParameter] public EditContext EditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha {  get; set; }

	[Parameter] public ReasonOfProjectViewModel ViewModel { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<Guid?>>? DifficultiesFacedFamily { get; set; }

	[Parameter] public List<ZeeSelectItem<AccompanyingTimeDuration?>>? AccompanyingTimeDurations { get; set; }

	private readonly HashSet<object> _validatedObjects = [];
	private EditContext? _editContext;

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