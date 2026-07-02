namespace Renee.Application.DTOs.Occupant;

public class SecondaryOccupantDto
{
	public Guid Id { get; set; }
	public string? Trigram { get; set; }
	public DateTime? Birthdate { get; set; }
	public int? Age { get; set; }
	public bool? IsDependent { get; set; }
}