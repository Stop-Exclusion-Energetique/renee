using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectCost.ViewModel;
using Renee.UI.Components.FormComponents;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectCost;

public partial class ProjectCost
{
	[Parameter] public ProjectCostViewModel ViewModel { get; set; } = null!;
	[Parameter] public EventCallback ShowSaveButton { get; set; }

	[CascadingParameter] public EditContext FormEditContext { get; set; } = null!;

	[CascadingParameter(Name = "IsUserAllowedToEdit")] public bool IsUserAllowedToEdit { get; set; }
	[CascadingParameter(Name = "IsSoliha")] public bool IsSoliha {  get; set; }

	private readonly List<ZeeSelectItem<double?>> _costsList = [new("6500€", 6500), new("7000€", 7000)];

	private void AddInvoice()
	{
		ViewModel.Invoices.Add(new InvoiceViewModel());

		if (ShowSaveButton.HasDelegate) ShowSaveButton.InvokeAsync();

		StateHasChanged();
	}

	private void RemoveInvoice(int invoiceIndex)
	{
		ViewModel.Invoices.RemoveAt(invoiceIndex);

		if (ShowSaveButton.HasDelegate) ShowSaveButton.InvokeAsync();

		StateHasChanged();
	}
}