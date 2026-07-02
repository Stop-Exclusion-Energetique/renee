using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;

namespace Renee.UI.Components.DisplayComponents;

public partial class ZeeCounter
{
	[Parameter] public string? Label { get; set; }

	[Parameter] public int? Count { get; set; }

	[Parameter] public EventCallback<int?> CountChanged { get; set; }

	[Parameter] public EditContext EditContext { get; set; } = default!;

	[Parameter] public FieldIdentifier FieldIdentifier { get; set; }

	[Parameter] public bool IsDisabled { get; set; }

	private async Task DecrementCount()
	{
		Count ??= 0;

		if (Count == 0) return;

		Count--;
		await OnCountChanged();
	}

	private async Task IncrementCount()
	{
		Count ??= 0;
		Count++;
		await OnCountChanged();
	}

	private async Task OnCountChanged()
	{
		await CountChanged.InvokeAsync(Count);

		if (EditContext != default!) EditContext.NotifyFieldChanged(FieldIdentifier);
	}
}