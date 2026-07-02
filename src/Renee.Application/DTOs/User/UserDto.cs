using Renee.Application.DTOs.ReportingStructure;

namespace Renee.Application.DTOs.User;

public record UserPersonnalInformations(
	string? Email,
	string? FirstName,
	string? LastName,
	string? PhoneNumber,
	string? SiretNumber);

public sealed class UserDto
{
	public Guid Id { get; } = Guid.NewGuid();
	public DateTime SubscriptionDateUtc { get; } = DateTime.UtcNow;
	public string? Email { get; set; }
	public string? LastName { get; set; }
	public string? FirstName { get; set; }
	public string? PhoneNumber { get; set; }
	public Guid? TerritoryId { get; set; }
	public Guid? AskedRole { get; set; }
	public ReportingStructureDto? ReportingStructureDto { get; set; }
	public string? OtherReportingStructure { get; set; }
	public string? Function { get; set; }
	public string? SiretNumber { get; set; }
	public string? CguVersion { get; set; }

	public UserDto(Guid id, DateTime subscriptionDateUtc, string? lastName, string? firstName, string? email) : this()
	{
		Id = id;
		SubscriptionDateUtc = subscriptionDateUtc;
		LastName = lastName;
		FirstName = firstName;
		Email = email;
	}

	public UserDto(Guid id, string? lastName, string? firstName, ReportingStructureDto? reportingStructureDto) : this()
	{
		Id = id;
		LastName = lastName;
		FirstName = firstName;
		ReportingStructureDto = reportingStructureDto;
	}

	public UserDto(
		Guid id,
		UserPersonnalInformations userPersonnalInformations,
		Guid? territoryId,
		ReportingStructureDto? reportingStructureDto) : this()
	{
		Id = id;
		LastName = userPersonnalInformations.LastName;
		FirstName = userPersonnalInformations.FirstName;
		Email = userPersonnalInformations.Email;
		PhoneNumber = userPersonnalInformations.PhoneNumber;
		SiretNumber = userPersonnalInformations.SiretNumber;
		TerritoryId = territoryId;
		ReportingStructureDto = reportingStructureDto;
	}

	public UserDto(Guid id, string? email) : this()
	{
		Id = id;
		Email = email;
	}

	private UserDto()
	{
	}

	public UserDto SetPersonnalInformations(
		string? phoneNumber,
		string? siretNumber)
	{
		PhoneNumber = phoneNumber;
		SiretNumber = siretNumber;
		return this;
	}

	public UserDto SetDataForSignIn(
		Guid askedRole,
		string? function,
		ReportingStructureDto reportingStructureDto,
		string? otherReportingStructureName,
		Guid? territoryId)
	{
		AskedRole = askedRole;
		Function = function;
		ReportingStructureDto = reportingStructureDto;
		OtherReportingStructure = otherReportingStructureName;
		TerritoryId = territoryId;
		return this;
	}

	public UserDto SetCguVersion(string? cguVersion)
	{
		CguVersion = cguVersion;
		return this;
	}
}