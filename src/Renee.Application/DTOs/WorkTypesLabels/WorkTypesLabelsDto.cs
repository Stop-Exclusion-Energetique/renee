namespace Renee.Application.DTOs.WorkTypesLabels;

public class WorkTypesLabelsDto(Guid id, string label)
{
	public Guid Id { get; } = id;
	public string Label { get; } = label;
}