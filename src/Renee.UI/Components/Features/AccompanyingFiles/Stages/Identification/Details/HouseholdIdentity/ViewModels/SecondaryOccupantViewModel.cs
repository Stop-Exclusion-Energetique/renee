using Renee.Application.DTOs.Occupant;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class SecondaryOccupantViewModel
{
	public Guid? Id { get; private init; }

	public string? Trigram { get; set; }

	public DateTime? Birthday { get; set; }

	public int? Age { get; set; }

	public bool? IsDependent { get; set; }

	public static SecondaryOccupantViewModel DtoToSecondaryOccupantViewModel(SecondaryOccupantDto dto)
	{
		return new SecondaryOccupantViewModel
		{
			Id = dto.Id,
			Trigram = dto.Trigram,
			Birthday = dto.Birthdate,
			Age = dto.Age,
			IsDependent = dto.IsDependent
		};
	}
}