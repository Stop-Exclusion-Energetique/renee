namespace Renee.Application.DTOs.PreWorkPlan;

public class ProjectTypeDto(Guid id, string label)
{
	public Guid Id { get; } = id;
	public string Label { get; } = label;
}