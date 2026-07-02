using Microsoft.AspNetCore.Components;
using Renee.Domain;
using Renee.UI.Components.Layout.SynthesisLayoutManager;

namespace Renee.UI.Components.Layout;

public partial class SynthesisLayout
{
	public bool ShouldDisplayLayoutNavigation { get; set; } = true;
	[Inject] public NavigationManager NavigationManager { get; set; } = null!;
	[Inject] public SynthesysLayoutStateManager SynthesisLayoutManager { get; set; } = null!;

	protected override void OnInitialized()
	{
		SynthesisLayoutManager.OnSynthesysChanged += HandleStateChanged;
	}

	private void HandleStateChanged(object? sender, EventArgs e)
	{
		InvokeAsync(StateHasChanged);
	}

	private void RedirectToIdentifySynthesis() =>
		NavigationManager.NavigateTo($"{(SynthesisLayoutManager.IsAccompanyingFile ? Endpoints.IdentificationSynthesis : Endpoints.CopropertyIdentificationSynthesis)}/{SynthesisLayoutManager.AssociatedResourceId}");

	private void RedirectToOrganizeAndFinanceSynthesis() =>
		NavigationManager.NavigateTo($"{(SynthesisLayoutManager.IsAccompanyingFile ? Endpoints.OrganizeAndFinanceSynthesis : Endpoints.CopropertyOrganizeAndFinanceSynthesis)}/{SynthesisLayoutManager.AssociatedResourceId}");

	private void RedirectToRealizeAndFollowSynthesis() =>
		NavigationManager.NavigateTo($"{(SynthesisLayoutManager.IsAccompanyingFile ? Endpoints.RealizeAndFollowSynthesis : Endpoints.CopropertyRealizeAndFollowSynthesis)}/{SynthesisLayoutManager.AssociatedResourceId}");
}