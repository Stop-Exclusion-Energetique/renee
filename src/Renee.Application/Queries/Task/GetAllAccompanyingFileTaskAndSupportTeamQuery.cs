using Renee.Application.Abstraction.Query;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.Task;

public class GetAllAccompanyingFileTaskAndSupportTeamQuery : IQuery<ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>>
{
	public Guid AssociatedResourceId { get; init; }
	public Guid UserId { get; set; }
	public bool IsCoproperty { get; set; } = false;
}

public record SupportTeamMemberObjectResult
{
	public string? FullName { get; init; }
	public string? Email { get; init; }
	public string? PhoneNumber { get; init; }
	public string? Role { get; set; }
	public bool IsEditable { get; set; }
}

public record TaskObjectResult
{
	public required Guid TaskId { get; init; }
	public required string TaskName { get; init; }
	public TaskPriority Priority { get; init; }
	public DateTime DueDate { get; init; }
	public DateTime StartDate { get; init; }
	public string AssignedUserName { get; init; } = string.Empty;
	public ProgressTask? ProgressTask { get; set; }
}

public record AccompanyingFileTargetInformationObjectResult
{
	public required GeographicalHousingAreaTypology? GeographicalHousingAreaTypology { get; init; }
	public required AccompanyingType? AccompanyingType { get; init; }
	public required Guid? AccompanyingTerritory { get; init; }
	public required string MarValue { get; init; }
	public required bool IsInTzeeProgram { get; init; }
}

public record BillingLogObjectResult
{
	public bool BilledJalon1 { get; set; }
	public double? AmountBilledFirstStage { get; set; }
	public DateTime? FundraisingLauchDateForFirstStage { get; set; }
	public string? BillingCallNumberFirstStage { get; set; }
	public string? InvoiceNumberFirstStage { get; set; }
	public DateTime? BillingDateFirstStage { get; set; }

	public bool BilledJalon2 { get; set; }
	public double? AmountBilledSecondStage { get; set; }
	public DateTime? FundraisingLauchDateForSecondStage { get; set; }
	public string? BillingCallNumberSecondStage { get; set; }
	public string? InvoiceNumberSecondStage { get; set; }
	public DateTime? BillingDateSecondStage { get; set; }

	public bool BilledJalon3 { get; set; }
	public double? AmountBilledThirdStage { get; set; }
	public DateTime? FundraisingLauchDateForThirdStage { get; set; }
	public string? BillingCallNumberThirdStage { get; set; }
	public string? InvoiceNumberThirdStage { get; set; }
	public DateTime? BillingDateThirdStage { get; set; }
}

public record GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult
{
	public string AccompanyingFileReference { get; set; } = string.Empty;
	public required List<TaskObjectResult>? Tasks { get; init; }
	public required List<SupportTeamMemberObjectResult> SupportTeamMembers { get; init; }
	public required bool IsUserInSupportTeam { get; init; }
	public AccompanyingFileTargetInformationObjectResult? TargetInformation { get; init; }
	public BillingLogObjectResult? BillingLog { get; set; }
}