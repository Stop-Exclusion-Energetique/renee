using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Interfaces;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.
	FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan;

public partial class PreFinancingPlan
{
	[Inject] public IWorkTypesLabelsService WorkTypesLabelsService { get; set; } = null!;

	[Parameter] public PreFinancingPlanViewModel ViewModel { get; set; } = null!;

	[Parameter] public Guid AccompanyingFileId { get; set; }

	[Parameter] public bool IsThirdStage { get; set; } = false;

	[Parameter] public double? TotalWorkCost { get; set; }

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

	public bool IsDuplicateFundingModeName { get; set; }

	private readonly HashSet<object> _validatedObjects = [];
	private Dictionary<Guid, string> _workTypeLabels = [];

	private EditContext? _editContext;
	private bool ShouldDisplayCopropertyWorkFinanceSection =>
		ViewModel.DateOfAgVote is not null ||
		ViewModel.MprCoproAids is not null ||
		ViewModel.ComplementaryCopropertyAids is not null ||
		ViewModel.CopropertyWorkPackages.Count > 0;

	private string? DateOfAgVoteDisplay => ViewModel.DateOfAgVote?.ToString("dd/MM/yyyy");

	private double CopropertyWorkPackagesTotalPrice =>
		ViewModel.CopropertyWorkPackages.Sum(workPackage => workPackage.TotalPrice ?? 0);

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
		FormEditContext.OnValidationRequested += EditContext_OnValidationRequested;
		_editContext.OnFieldChanged += HandleFieldChanged;
	}

	protected override async Task OnInitializedAsync()
	{
		var workTypesResult = await WorkTypesLabelsService.GetAllWorkTypesLabels();
		if (workTypesResult.IsSuccess && workTypesResult.Value is not null)
			_workTypeLabels = workTypesResult.Value.ToDictionary(workType => workType.Id, workType => workType.Label);
	}

	protected override void OnParametersSet()
	{
		ViewModel.TotalWorkCost = TotalWorkCost;
	}

	private void CheckDuplicateFundingModeName(string fundingModeName)
	{
		var allFundingModesExceptLast = ViewModel.FundingModes.Take(ViewModel.FundingModes.Count - 1);

		IsDuplicateFundingModeName = allFundingModesExceptLast.Any(
			fm => fm.Name!.Equals(fundingModeName, StringComparison.CurrentCultureIgnoreCase));
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
		if (e.FieldIdentifier is { Model: PreFinancingPlanViewModel, FieldName: nameof(ViewModel.FundingModes) })
			StateHasChanged();
	}

	private void OnAddFundingMode()
	{
		ViewModel.FundingModes.Add(new FundingModeViewModel());
	}

	private void OnDeleteFundingMode(FundingModeViewModel fundingMode)
	{
		ViewModel.FundingModes.Remove(fundingMode);
		IsDuplicateFundingModeName = false;
	}

	private string GetPreFinancingPlanBalancingTextColor() =>
		ViewModel.PreFinancingPlanBalancing switch
		{
			> 0 => "financing-surplus",
			< 0 => "insufficient-financing",
			_ => "right-balancing"
		};

	private string GetWorkTypeLabel(Guid workTypeId) =>
		_workTypeLabels.GetValueOrDefault(workTypeId) ?? "Travaux recommandes";
}