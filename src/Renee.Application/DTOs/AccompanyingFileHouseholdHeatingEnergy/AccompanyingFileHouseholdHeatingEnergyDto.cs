namespace Renee.Application.DTOs.AccompanyingFileHouseholdHeatingEnergy;

public class AccompanyingFileHouseholdHeatingEnergyDto
{
	public Guid HouseholdHeatingEnergyId { get; init; }
	public string Name { get; init; } = null!;
	public double? Value { get; init; }
}
