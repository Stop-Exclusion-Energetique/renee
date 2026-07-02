using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.Presenter;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant;

public partial class WorkParticipant
{
	[Parameter] public List<Guid?> SelectedWorkParticipantsIds { get; set; } = [];
	[Parameter] public List<ZeeSelectItem<Guid?>> SiteSupervisionDifficulties { get; init; } = [];
	[Parameter] public WorkParticipantViewModel ViewModel { get; set; } = new();
	[Parameter] public EditContext EditContext { get; init; } = null!;
	[Parameter] public bool IsUserAllowedToEdit { get; init; }
	[Parameter] public int Index { get; init; }

	public List<ZeeSelectItem<ParticipantType?>> ParticipantTypes { get; } = 
	[
		new(ParticipantTypeLabels.Company, ParticipantType.Company),
		new(ParticipantTypeLabels.ProjectManagementAssistant, ParticipantType.ProjectManagementAssistant),
		new(ParticipantTypeLabels.ProjectManagement, ParticipantType.ProjectManagement)
	];

	public List<ZeeSelectItem<WorkQuality?>> WorkQualities { get; } = 
	[
		new(WorkQualityLabels.Compliant, WorkQuality.Compliant),
		new(WorkQualityLabels.Partial, WorkQuality.Partial),
		new(WorkQualityLabels.NonCompliant, WorkQuality.NonCompliant)
	];

	private void OnSelectedWorkParticipantChanged()
	{
		ViewModel = new WorkParticipantPresenter(ViewModel, SiteSupervisionDifficulties, SelectedWorkParticipantsIds).Present();
		StateHasChanged();
	}
}