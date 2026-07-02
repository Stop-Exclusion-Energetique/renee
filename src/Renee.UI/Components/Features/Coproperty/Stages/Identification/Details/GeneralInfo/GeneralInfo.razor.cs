using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Radzen;
using Renee.Application.DTOs.Occupant;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GeneralInfo.ViewModel;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GeneralInfo;

public partial class GeneralInfo
{
    [CascadingParameter] public EditContext EditContext { get; set; } = null!;

    [CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }

    [Parameter] public GeneralInfoViewModel ViewModel { get; set; } = null!;
    [Parameter] public List<AddressDto>? AddressList { get; set; }

    [Parameter] public Func<LoadDataArgs?, Task>? LoadAddresses { get; set; }

    private readonly HashSet<object> _validatedObjects = [];

    private EditContext? _editContext;

    private bool IsCustomAddress => ViewModel.IsManualInput == true && !IsUserAllowedToEdit;

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

    private async Task OnLoadAddresses(LoadDataArgs? args)
    {
        if (LoadAddresses is not null) await LoadAddresses.Invoke(args);
    }
}
