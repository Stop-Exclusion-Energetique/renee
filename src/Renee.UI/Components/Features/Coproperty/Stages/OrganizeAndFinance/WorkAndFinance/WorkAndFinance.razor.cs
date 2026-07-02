using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Renee.Application.Interfaces;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.WorkPackage.ViewModel;
using Renee.UI.Components.Features.Coproperty.Stages.OrganizeAndFinance.BasePage.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.Coproperty.Stages.OrganizeAndFinance.WorkAndFinance;

public partial class WorkAndFinance
{
    [Inject] public IWorkTypesLabelsService WorkTypesLabelsService { get; set; } = null!;

    [CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

    [CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }

    private EditContext? _editContext;

    [Parameter] public CopropertyOrganizeAndFinanceViewModel ViewModel { get; set; } = null!;
    [Parameter] public Action? FormFieldHasChanged { get; set; }

    private readonly HashSet<object> _validatedObjects = [];

    private List<ZeeSelectItem<Guid>> WorkTypes { get; set; } = [];

    protected override async Task OnInitializedAsync()
    {
        WorkTypes = (await WorkTypesLabelsService.GetAllWorkTypesLabels()).Value!
            .Select(wt => new ZeeSelectItem<Guid>(wt.Label, wt.Id)).ToList();

        if (ViewModel.WorkPackages.Count == 0) ViewModel.WorkPackages.Add(new WorkPackageViewModel());

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
        if (e.FieldIdentifier is { FieldName: "Price", Model: WorkPackageViewModel.WorkType })
        {
            FormEditContext.NotifyFieldChanged(
                new FieldIdentifier("PreWorkPlanTabViewModel", "RecommendedWorkTotalPrice"));
            StateHasChanged();
        }
    }

    private void OnAddWorkPackage()
    {
        ViewModel.WorkPackages.Add(new WorkPackageViewModel());
    }
    public void OnDeleteWorkPackage(int index)
    {
        ViewModel.WorkPackages.RemoveAt(index);
        FormFieldHasChanged?.Invoke();
    }
}
