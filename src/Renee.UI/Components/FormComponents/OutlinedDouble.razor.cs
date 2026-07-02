using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Renee.Domain;

namespace Renee.UI.Components.FormComponents;

public partial class OutlinedDouble
{
	[Parameter(CaptureUnmatchedValues = true)]
	public IReadOnlyDictionary<string, object?>? AdditionalAttributes { get; set; }

	/// <summary>
	///     The value of the input.
	///     use Double = ... to display the value without binding
	///     use bind-Double = ... to bind the value to a variable
	/// </summary>
	[Parameter]
	public double? Double { get; set; }

	[Parameter] public EventCallback<double?> DoubleChanged { get; set; }

	[Parameter] public string? Label { get; set; }

	[Parameter] public TypeOfValue Type { get; set; }

	[Parameter] public string? InfoBullMessage { get; set; }

	[Parameter] public string? InfoBullTitle { get; set; }

	[Parameter] public EditContext? EditContext { get; set; }

	[Parameter] public FieldIdentifier FieldIdentifier { get; set; }

	[Parameter] public bool IsNullable { get; set; }

	[Parameter] public bool? IsRequired { get; set; }

	[Parameter] public EventCallback ActionCallAfterChange { get; set; }

	private string DoubleAsString
	{
		get
		{
			if (Double == null) return string.Empty;

			switch (Type)
			{
				case TypeOfValue.Percentage:
					var percentageValue = Double / 100;
					return $"{percentageValue.Value.ToString("0.##%", CultureInfo.InvariantCulture)}";
				case TypeOfValue.Currency:
					return $"{Double.Value.ToString("C", CultureInfo.CreateSpecificCulture("fr-FR"))}";
				case TypeOfValue.Meter:
					return $"{Double.Value.ToString(CultureInfo.CreateSpecificCulture("fr-FR"))} m";
                case TypeOfValue.Double:
				default:
					return $"{Double.Value.ToString(CultureInfo.CreateSpecificCulture("fr-FR"))}";
			}
		}
	}

	private string? Validation { get; set; }

	public enum TypeOfValue
	{
		Double, Percentage, Currency, Meter
	}

	private void OnBlur()
	{
		EditContext?.NotifyFieldChanged(FieldIdentifier);
	}

	private async Task OnValueChanged(ChangeEventArgs args)
	{
		Validation = string.Empty;

		var argsValue = args.Value?.ToString();
		if (!string.IsNullOrEmpty(argsValue))
		{
			argsValue = argsValue.Replace("€", "")
			.Replace(" m", "").Replace("m","").Trim();
			await TryParseDoubleValue(argsValue);
			if (ActionCallAfterChange.HasDelegate) await ActionCallAfterChange.InvokeAsync();
		}
		else
		{
			if (!IsNullable) Validation = Labels.Errors.NumberNotBeEmpty;
			Double = null;
			await DoubleChanged.InvokeAsync(Double);
		}
	}

	private async Task TryParseDoubleValue(string argsValue)
	{
		if (Regex.IsMatch(
			    argsValue,
			    Constants.FrenchDecimalPattern,
			    RegexOptions.NonBacktracking,
			    TimeSpan.FromMilliseconds(1000)))
		{
			if (double.TryParse(argsValue, new CultureInfo("fr-FR", false), out var amount))
				Double = Math.Round(amount, 2);
			else
				Double = null;

			await DoubleChanged.InvokeAsync(Double);
			Validation = string.Empty;
		}
		else
		{
			var doubleVariable = Double;
			Double = doubleVariable;
			Validation = Labels.Errors.NumberShouldBeCorrect;
		}
	}
}