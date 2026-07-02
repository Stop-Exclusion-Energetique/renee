namespace Renee.Application.DTOs.Occupant;

public sealed class AddressDto
{
	public Guid? Id { get; init; }
	public string? Label { get; init; }
	public string? PostalCode { get; init; }
	public string? City { get; init; }
	public string? Department { get; init; }
	public string? Region { get; init; }
	public string? Name { get; init; }
	public string? Type { get; init; }
	public string? AdditionalAddress { get; init; }
	public string? Street { get; init; }
	public string? HouseNumber { get; init; }
}