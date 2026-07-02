using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.DTOs.WorkTypeProjectType;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab;

public partial class PreWorkPlanTab
{
	[Inject] public IWorkTypesLabelsService WorkTypesLabelsService { get; set; } = null!;
	[Inject] public IProjectTypeService PreWorkPlanProjectTypeService { get; set; } = null!;
	[Inject] public IInsuranceTypeService PreWorkPlanInsuranceTypeService { get; set; } = null!;
	[Inject] public IWorkTypeProjectTypeService WorkTypeProjectTypeService { get; set; } = null!;

	[Parameter] public PreWorkPlanTabViewModel ViewModel { get; set; } = null!;
	[Parameter] public Action? FormFieldHasChanged { get; set; }

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

	private readonly List<ZeeSelectItem<RenovationType?>> _renovationTypeList =
	[
		new(RenovationTypeLabel.MajorRenovation, RenovationType.MajorRenovation),
		new(RenovationTypeLabel.EfficientRenovationInStages, RenovationType.EfficientRenovationInStages)
	];

	private readonly HashSet<object> _validatedObjects = [];

	private readonly List<ZeeSelectItem<PartlyStateTreatment?>> _yesNoPartialList =
    [
        new(PartlyStateTreatmentLabel.No, PartlyStateTreatment.No),
        new(PartlyStateTreatmentLabel.Partly, PartlyStateTreatment.Partly),
        new(PartlyStateTreatmentLabel.Yes, PartlyStateTreatment.Yes)
	];

	private List<ZeeSelectItem<Guid>> WorkTypes { get; set; } = [];
	private List<ZeeSelectItem<Guid>> PreWorkPlanProjectTypes { get; set; } = [];
	private List<ZeeSelectItem<Guid>> PreWorkPlanInsuranceTypes { get; set; } = [];
	private List<WorkTypeProjectTypeDto> WorkTypeProjectTypeDtos { get; set; } = [];

	private Guid TenYearCivilLiabilityAraInsuranceType { get; set; }
	private bool IsInitialWorkPackageNotFilled =>
		ViewModel.WorkPackages.Count == 1 &&
		(ViewModel.WorkPackages[0].WorkTypes.Count == 0 || ViewModel.WorkPackages[0].WorkTypes.Any(wt => wt.Price is null));

	private EditContext? _editContext;

	protected override async Task OnInitializedAsync()
	{
		var workTypesResult = await WorkTypesLabelsService.GetAllWorkTypesLabels();
		if (workTypesResult.IsSuccess && workTypesResult.Value is not null)
			WorkTypes = [.. workTypesResult.Value.Select(wt => new ZeeSelectItem<Guid>(wt.Label, wt.Id))];

		var projectTypesResult = await PreWorkPlanProjectTypeService.GetAllProjectTypes();
		if (projectTypesResult.IsSuccess && projectTypesResult.Value is not null)
			PreWorkPlanProjectTypes = [.. projectTypesResult.Value.Select(pt => new ZeeSelectItem<Guid>(pt.Label, pt.Id))];

		var workTypeProjectTypesResult = await WorkTypeProjectTypeService.GetAllWorkTypeProjectTypes();
		if (workTypeProjectTypesResult.IsSuccess && workTypeProjectTypesResult.Value is not null)
			WorkTypeProjectTypeDtos = [.. workTypeProjectTypesResult.Value.Select(wpt => new WorkTypeProjectTypeDto(wpt.WorkType, wpt.ProjectType))];

		var insuranceTypesResult = await PreWorkPlanInsuranceTypeService.GetAllInsuranceTypes();
		if (insuranceTypesResult.IsSuccess && insuranceTypesResult.Value is not null)
		{
			var insuranceTypeDtos = insuranceTypesResult.Value.ToList();
			PreWorkPlanInsuranceTypes = insuranceTypeDtos.Select(pt => new ZeeSelectItem<Guid>(pt.Label, pt.Id)).ToList();
			TenYearCivilLiabilityAraInsuranceType =
				insuranceTypeDtos.Find(it => it.Label == Labels.TenYearCivilLiabilityAra)?.Id ?? Guid.Empty;
		}

		if (ViewModel.WorkPackages.Count == 0) ViewModel.WorkPackages.Add(new WorkPackageViewModel { Id = Guid.NewGuid() });

		_editContext = new EditContext(ViewModel);
		FormEditContext.OnValidationRequested += EditContext_OnValidationRequested;
		FormEditContext.OnFieldChanged += HandleFieldChanged;
	}

	public void OnDeleteWorkPackage(int index)
	{
		ViewModel.WorkPackages.RemoveAt(index);
		UpdateProjectTypesList();
		FormFieldHasChanged?.Invoke();
	}

	private void EditContext_OnValidationRequested(object? sender, ValidationRequestedEventArgs e)
	{
		if (_validatedObjects.Contains(ViewModel)) return;
		_editContext?.Validate();
		_validatedObjects.Add(ViewModel);
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (e.FieldIdentifier is { FieldName: "Price", Model: WorkPackageViewModel.WorkType })
		{
			FormEditContext.NotifyFieldChanged(
				new FieldIdentifier(ViewModel, nameof(ViewModel.RecommendedWorkTotalPrice)));
			StateHasChanged();
		}
	}

	private void OnAddWorkPackage()
	{
		ViewModel.WorkPackages.Add(new WorkPackageViewModel { Id = Guid.NewGuid() });
		FormFieldHasChanged?.Invoke();
	}

	private void UpdateEnergeticClassJump()
	{
		ViewModel.EstimatedEnergyClassJump = AccompanyingFileHelper.CalculateEnergeticClassJump(ViewModel.EstimatedEnergyDpeBeforeWork, ViewModel.EstimatedEnergyDpeAfterWork);
		StateHasChanged();
	}

	private void UpdateProjectTypesList()
	{
		if (ViewModel.PreWorkPlanProjectTypes == null)
			return;

		var selectedWorkTypesFromAllWorkPackages = ViewModel.WorkPackages
			.SelectMany(wp => wp.WorkTypes)
			.Select(wt => wt.Id)
			.Distinct();

		var calculatedProjectTypesIds = WorkTypeProjectTypeDtos
			.Where(wpt => selectedWorkTypesFromAllWorkPackages.Contains(wpt.WorkType))
			.Select(wpt => wpt.ProjectType);

		ViewModel.PreWorkPlanProjectTypes = [.. calculatedProjectTypesIds];
	}
}
