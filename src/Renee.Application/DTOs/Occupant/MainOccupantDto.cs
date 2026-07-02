using Renee.Domain.Enums;

namespace Renee.Application.DTOs.Occupant;

public sealed class MainOccupantDto
{
	public Guid? Id { get; init; }
	public string? Trigram { get; init; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public string? PhoneNumber { get; init; }
	public string? Email { get; init; }
	public string? Gender { get; set; }
	public DateTime? Birthdate { get; init; }
	public int? Age { get; init; }
	public string? Job { get; init; }
	public SocialProtectionFund? SocialWelfareFund { get; init; }
	public string? OtherSocialWelfareFund { get; init; }
	public PensionFund? PensionFund { get; init; }
	public string? OtherPensionFund { get; init; }
	public AdditionalFund? ComplementaryFund { get; init; }
	public string? OtherComplementaryFund { get; init; }
	public SocioProfessionalCategory? SocioProfessionalCategoryId { get; init; }
	public Guid AccompanyingFileId { get; set; }
}