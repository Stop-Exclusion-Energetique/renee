using Renee.Domain.Enums;

namespace Renee.Application.Queries.AccompanyingFile.QueryObjectResult;

public class GetAccompanyingFileForFileListQueryObjectResult
{
	public List<UserAccompanyingFileResume> AccompanyingFiles { get; init; } = [];
	public int TotalAccompanyingFilesCount { get; init; }
}

public class UserAccompanyingFileResume
{
	public Guid Id { get; init; }
	public string Reference { get; init; } = null!;
	public string? FirstName { get; init; } = null!;
	public string? LastName { get; init; } = null!;
	public string? Address { get; init; }
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }
	public DateTime OpeningDateUtc { get; init; }
	public DateTime LastModificationDateUtc { get; init; }
	public DateTime? ClosedDateUtc { get; init; }
	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ReportingStructureId { get; set; }
	public Guid? DiffuseCoordinatorId { get; set; }
	public Guid? TargetCoordinatorId { get; set; }
	public Guid? TerritorialBuilder { get; set; }
    public Guid? SecondTerritorialBuilder { get; set; }
	public Guid? TerritoryId { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? NationalStructureId { get; set; }
	public bool HasSolidarBuilderDeletedHisAccount { get; set; }
	public bool HasSecondSolidarBuilderDeletedHisAccount { get; set; }
	public bool HasThirdSolidarBuilderDeletedHisAccount { get; set; }
	public bool? ShouldAccompanyingFileBeSubmittedToAnah { get; set; }
	public Guid? AbortReasonLabelId { get; set; }
	public string? StageFacturationLabel { get; set; }
	public string? AbortLabel { get; set; }
	public string? InvoiceNumber { get; set; }
	public double? InvoiceAmount { get; set; }
	public DateTime? InvoiceDateUtc { get; set; }
	public string? SolidarBuilderAbortRequestDetails { get; set; }
	public bool IsBillingRequested { get; set; }
	public bool IsInTZEEProgram { get; set; }
}