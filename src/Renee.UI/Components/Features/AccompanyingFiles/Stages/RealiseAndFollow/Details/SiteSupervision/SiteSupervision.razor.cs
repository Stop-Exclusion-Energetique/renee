using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Queries.SiteSupervisionDifficultyLabels;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.Components.WorkParticipant.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.SiteSupervision;

public partial class SiteSupervision
{
	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;
	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha { get; set; }
	[Parameter] public SiteSupervisionViewModel ViewModel { get; set; } = null!;
	[Parameter] public Action? FormFieldHasChanged { get; set; }

	[Inject] private ISendEventQuery SendEventQuery { get; set; } = null!;

	private List<ZeeSelectItem<Guid?>> SiteSupervisionDifficulties = [];
	private EditContext? _editContext;

	protected override async Task OnInitializedAsync()
	{
		_editContext = new EditContext(ViewModel);
		_editContext.OnFieldChanged += HandleFieldChanged;

		SiteSupervisionDifficulties = [.. (await SendEventQuery.Send(new GetAllSiteSupervisionDifficultyLabelQuery())).Value!.Select(x => new ZeeSelectItem<Guid?>(x.Label, x.Id))];

		if (ViewModel.WorkParticipants.Count == 0) ViewModel.WorkParticipants.Add(new WorkParticipantViewModel { Id = Guid.NewGuid() });
	}

	private void HandleFieldChanged(object? sender, FieldChangedEventArgs e)
	{
		if (sender == _editContext) FormEditContext?.NotifyFieldChanged(e.FieldIdentifier);
	}

	private void AddWorkParticipant()
	{
		ViewModel.WorkParticipants.Add(new WorkParticipantViewModel { Id = Guid.NewGuid() });
		FormFieldHasChanged?.Invoke();
	}

	public void DeleteWorkParticipant(int index)
	{
		ViewModel.WorkParticipants.RemoveAt(index);
		FormFieldHasChanged?.Invoke();
	}
}