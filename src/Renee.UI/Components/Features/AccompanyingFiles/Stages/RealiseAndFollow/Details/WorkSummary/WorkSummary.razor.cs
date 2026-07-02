using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Helpers;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.WorkSummary.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.WorkSummary;

public partial class WorkSummary
{
	[Parameter] public WorkSummaryViewModel ViewModel { get; set; } = null!;

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
    [CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }

    private readonly List<ZeeSelectItem<PartlyStateTreatment?>> _yesNoPartialList =
    [
        new(PartlyStateTreatmentLabel.No, PartlyStateTreatment.No),
        new(PartlyStateTreatmentLabel.Partly, PartlyStateTreatment.Partly),
        new(PartlyStateTreatmentLabel.Yes, PartlyStateTreatment.Yes)
	];
	private EditContext? _editContext;

	protected override void OnInitialized()
	{
		_editContext = new EditContext(ViewModel);
		_editContext.OnFieldChanged += HandleFieldChanged;
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) FormEditContext.NotifyFieldChanged(e.FieldIdentifier);
	}

	public void OnHasEffectiveComplianceWithWorkRecommendationsChanged()
	{
		if (ViewModel.HasEffectiveComplianceWithWorkRecommendations == true)
		{
			ViewModel.ShouldChangeFinalEstimatedDpe = null;
			ViewModel.FinalDpe = null;
			UpdateEnergeticClassJump();
		}
	}

	public void OnShouldChangeFinalEstimatedDpeChanged()
	{
		if (ViewModel.ShouldChangeFinalEstimatedDpe == false)
			ViewModel.FinalDpe = null;

		if (ViewModel.ShouldChangeFinalEstimatedDpe == true)
			ViewModel.FinalDpe = ViewModel.EstimatedDpe;

		UpdateEnergeticClassJump();
	}

	private void UpdateEnergeticClassJump()
	{
		ViewModel.FinalDpeClassJump = AccompanyingFileHelper.CalculateEnergeticClassJump(ViewModel.InitialDpe, ViewModel.FinalDpe);
		StateHasChanged();
	}
}