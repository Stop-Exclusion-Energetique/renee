using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.TargetInformations;

public partial class TargetInformations
{
	[Parameter] public AccompanyingFileTargetDetailsViewModel TargetInformation { get; set; } = null!;
	[Parameter] public EditContext TargetInformationEditContext { get; set; } = null!;

	[Parameter] public List<ZeeSelectItem<Guid?>>? Territory { get; set; }

	[Parameter] public bool ShowSubmitButton { get; set; }

	[Parameter] public EventCallback OnSubmit { get; set; }

	[Parameter] public EventCallback OnTargetInformationChanged { get; set; }

	[Parameter] public string UserRole { get; set; } = string.Empty;

	private bool IsSolidarBuilder => UserRole == Constants.SolidarBuilderRole;

	private Task OnAnyFieldChanged() => OnTargetInformationChanged.HasDelegate
		? OnTargetInformationChanged.InvokeAsync()
		: Task.CompletedTask;
}