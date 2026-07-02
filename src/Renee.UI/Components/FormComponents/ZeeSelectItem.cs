namespace Renee.UI.Components.FormComponents;

public class ZeeSelectItem<T>(string? label, T? value)
{
	public T? Value { get; } = value;
	public string? Label { get; } = label;
}