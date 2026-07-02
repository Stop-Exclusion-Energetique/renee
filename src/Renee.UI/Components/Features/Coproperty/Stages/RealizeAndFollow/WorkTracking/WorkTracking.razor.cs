using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Renee.UI.Components.Features.Coproperty.Stages.RealizeAndFollow.BasePage.ViewModel;

namespace Renee.UI.Components.Features.Coproperty.Stages.RealizeAndFollow.WorkTracking;

public partial class WorkTracking
{
    [CascadingParameter] public EditContext EditContext { get; set; } = null!;
    [CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [Parameter] public CopropertyRealizeAndFollowViewModel ViewModel { get; set; } = null!;

    private readonly HashSet<object> _validatedObjects = [];

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        base.OnInitialized();
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
