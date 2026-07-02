namespace Renee.Application.DTOs.WorkPackageWorkTypeCost;

public class WorkPackageWorkTypeCostDto(Guid id, double? cost, string? description)
{
	public Guid Id { get; } = id;
	public double? Cost { get; } = cost;
	public string? Description { get; } = description;
}