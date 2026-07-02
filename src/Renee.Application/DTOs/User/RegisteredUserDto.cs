namespace Renee.Application.DTOs.User;

public sealed class RegisteredUserDto
{
	public Guid Id { get; init; }
	public string? UserName { get; init; }
	public string? Email { get; init; }
	public Guid? RoleId { get; init; }
	public string? RoleName { get; init; }
	public string? GroupSid { get; init; }
	public bool? IsAccountDeleted {  get; init; }
	public Guid? TerritoryId { get; init; }
	public DateTime? LastLoginDate { get; init; }
	public DateTime NextDateForAnahGrantCheck { get; init; }
}