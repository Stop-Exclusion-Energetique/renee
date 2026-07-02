namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.RealiseAndFollow.Details.ProjectCost.ViewModel;

public class ProjectCostViewModel
{
	public List<InvoiceViewModel> Invoices { get; set; } = [];
	public double? AccompanyingCost { get; set; }
	public double? HouseholdAutoFinancing { get; set; }
	public double? WorkTotalCost => Invoices.Sum(i => i.InvoiceTotalCost);
}

public class InvoiceViewModel
{
	public Guid? Id { get; set; }
	public double? InvoiceTotalCost { get; set; }
	public double? LaborBilled { get; set; }
}