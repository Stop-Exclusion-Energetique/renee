using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace Renee.UI.Components.FormComponents;

public sealed partial class ZeeSelect<TItem>
{
	[Parameter] public bool IsMultiSelect { get; set; }

	[Parameter] public List<ZeeSelectItem<TItem>>? Items { get; set; }

	[Parameter] public TItem? CheckedValue { get; set; }

	[Parameter] public List<TItem>? CheckedValues { get; set; }

	[Parameter] public string? ElementName { get; set; }
	[Parameter] public string? Label { get; set; }
	[Parameter] public bool ShowLabelBefore { get; set; } = true;
	[Parameter] public bool? IsRequired { get; set; }

	[Parameter] public EventCallback<TItem?> CheckedValueChanged { get; set; }

	[Parameter] public EventCallback<List<TItem>?> CheckedValuesChanged { get; set; }

	[Parameter] public EditContext? EditContext { get; set; }

	[Parameter] public FieldIdentifier? FieldIdentifier { get; set; }

	[Parameter] public bool IsDisabled { get; set; }

	[Parameter] public EventCallback OnNewValueSelected { get; set; }

	[Parameter(CaptureUnmatchedValues = true)]
	public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

	[Parameter] public bool ShouldSort { get; set; }

	private string IsRequiredCssClass => IsRequired != null && IsRequired.Value ? "required" : "";
	[Inject] private IJSRuntime? JsRuntime { get; set; }

	private bool IsVisible { get; set; }

	protected override void OnParametersSet()
	{
		if (Items != null && ShouldSort)
		{
			Items = [.. Items.OrderBy(x => x.Label, StringComparer.CurrentCultureIgnoreCase)];
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		IsVisible = !Equals(CheckedValue, default(TItem)) || (CheckedValues != null && CheckedValues.Count != 0);
		if (JsRuntime != null && !firstRender) await JsRuntime.InvokeVoidAsync("window.appendStar", ElementName, IsRequired);
	}

	private static bool Compare<T>(T x, T y) => EqualityComparer<T>.Default.Equals(x, y);

	private async Task OnCheckRadioClicked(TItem? value)
	{
		if (!IsDisabled)
		{
			CheckedValue = Compare(value, CheckedValue) ? default! : value;

			await CheckedValueChanged.InvokeAsync(CheckedValue);

			if (OnNewValueSelected.HasDelegate) await OnNewValueSelected.InvokeAsync();

			if (EditContext is not null && FieldIdentifier is not null)
				EditContext.NotifyFieldChanged(FieldIdentifier.Value);
		}
	}

	private async Task OnListChanged(object obj)
	{
		if (IsMultiSelect)
		{
			var tItems = obj as IEnumerable<TItem>;
			CheckedValues = tItems?.ToList() ?? null;
			await CheckedValuesChanged.InvokeAsync(CheckedValues);
		}
		else
		{
			CheckedValue = obj is TItem item ? item : default;
			await CheckedValueChanged.InvokeAsync(CheckedValue);
		}

		if (OnNewValueSelected.HasDelegate) await OnNewValueSelected.InvokeAsync();

		if (EditContext is not null && FieldIdentifier is not null)
			EditContext.NotifyFieldChanged(FieldIdentifier.Value);

		IsVisible = !Equals(CheckedValue, default(TItem)) || (CheckedValues != null && CheckedValues.Count != 0);
		if (JsRuntime != null) await JsRuntime.InvokeVoidAsync("window.appendStar", ElementName, IsRequired);
	}

	private async Task<IEnumerable<ZeeSelectItem<TItem>>> SearchMethod(string arg)
	{
		return await Task.Run(
			() =>
			{
				return Items != null
					? Items.Where(
						x => x.Label != null && x.Label.Contains(arg, StringComparison.CurrentCultureIgnoreCase))
					: new List<ZeeSelectItem<TItem>>();
			});
	}
}