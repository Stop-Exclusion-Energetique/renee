using Renee.Domain;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.
	FundingMode.ViewModel;

public class FundingModeViewModel
{
	public Guid? Id { get; init; }
	[Required(ErrorMessage = Labels.Errors.RequiredFundingModeName)] public string? Name { get; set; }
	public double? Amount { get; set; }
}