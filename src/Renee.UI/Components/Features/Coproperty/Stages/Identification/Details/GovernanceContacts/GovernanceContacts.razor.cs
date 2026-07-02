using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GovernanceContacts.ViewModel;
using Renee.UI.Components.FormComponents;
using Renee.Domain.Enums;
using Renee.Application.Helpers;

namespace Renee.UI.Components.Features.Coproperty.Stages.Identification.Details.GovernanceContacts;

public partial class GovernanceContacts
{
    [CascadingParameter] public EditContext EditContext { get; set; } = null!;
    [CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [Parameter] public GovernanceContactsViewModel ViewModel { get; set; } = null!;

    [Parameter] public HousingType? HousingTypology { get; set; }

    private static readonly List<ZeeSelectItem<NatureOfSyndicType?>> NatureOfSyndicTypes =
        [
            new(NatureOfSyndicType.Professional.GetDescription(), NatureOfSyndicType.Professional),
            new(NatureOfSyndicType.Benevole.GetDescription(), NatureOfSyndicType.Benevole)
        ];

    private readonly HashSet<object> _validatedObjects = [];

    private EditContext? _editContext;

    protected override void OnInitialized()
    {
        base.OnInitialized();
        _editContext = new EditContext(ViewModel);
        EditContext.OnValidationRequested += EditContext_OnValidationRequested;
        _editContext.OnFieldChanged += HandleFieldChanged;
    }

    protected override void OnParametersSet()
    {
        if (ViewModel != null)
        {
            ViewModel.HousingTypology = HousingTypology;
        }
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
