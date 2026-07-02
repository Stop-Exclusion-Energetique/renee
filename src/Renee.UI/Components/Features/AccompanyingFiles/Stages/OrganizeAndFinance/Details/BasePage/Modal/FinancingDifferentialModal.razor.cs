using Blazored.Modal;
using Microsoft.AspNetCore.Components;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.BasePage.Modal;

public partial class FinancingDifferentialModal
{
	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;

	[Parameter] public string ModalText { get; set; } = string.Empty;
	[Parameter] public double Differential { get; set; }
}