using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain.Enums;
using Renee.UI.Components.Features.AccompanyingFiles.Menu.Billing.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.Billing;

public partial class AccompanyingFileBilling
{
	[Parameter] public BillingLogViewModel BillingLog { get; set; } = null!;
	[Parameter] public EditContext BillingEditContext { get; set; } = null!;

	[Parameter] public bool ShowSubmitButtonForFirstFacturationData { get; set; }
	[Parameter] public bool ShowSubmitButtonForSecondFacturationData { get; set; }
	[Parameter] public bool ShowSubmitButtonForThirdFacturationData { get; set; }

	[Parameter] public EventCallback OnSubmit { get; set; }
	[Parameter] public EventCallback<FacturationStage> OnFacturationDataChange { get; set; }

	[Parameter] public Func<bool> FacturationFieldShouldBeDisabled { get; set; } = default!;
}