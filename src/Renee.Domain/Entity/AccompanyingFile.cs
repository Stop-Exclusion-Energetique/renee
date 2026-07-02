namespace Renee.Domain.Entity;

public class AccompanyingFile
{
	public Guid Id { get; set; }

	public Guid AccompanyingFileHousehold { get; set; }

	public Guid AccompanyingFileHousing { get; set; }

	public Guid AccompanyingFilePreWorkPlan { get; set; }

	public Guid AccompanyingFileSupportTeam { get; set; }

	public Guid AccompanyingFilePreFinancingPlan { get; set; }

	public Guid? AccompanyingFileWorkMonitoring { get; set; }

	public Guid? SiteSupervisionId { get; set; }

	public string AccompanyingFileReference { get; set; } = null!;

	public int AccompanyingFileMilestone { get; set; }

	public int AccompanyingFileStatus { get; set; }

	public string? CommentOnBlockingProof { get; set; }

	public DateTime? FirstEncounterDate { get; set; }

	public DateTime? StartOfAccompanyingDate { get; set; }

	public int? NumberOfEncounterWithFamilyForIdentificationMilestone { get; set; }

	public Guid? CreatedBy { get; set; }

	public Guid? UpdatedBy { get; set; }

	public Guid? ClosedBy { get; set; }

	public DateTime? OpeningDate { get; set; }

	public DateTime? LastUpdateDate { get; set; }

	public DateTime? CloseDate { get; set; }

	public int? NumberOfEncounterWithFamilyForOrganizeAndFinanceMilestone { get; set; }

	public DateTime? EndOfAccompanyingDate { get; set; }

	public DateTime? EndOfEncounterDate { get; set; }

	public int? NumberOfEncounterWithFamilyForRealizeAndFollowMilestone { get; set; }

	public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }

	public bool? IsDeleted { get; set; }

	public int? AccompanyingType { get; set; }

	public Guid? AccompanyingFileTerritory { get; set; }

	public int? DeliveryTime { get; set; }

	public DateTime? IdentifySynthesisValidationDate { get; set; }

	public DateTime? OrganizeAndFinanceSynthesisValidationDate { get; set; }

	public DateTime? RealizeAndFollowSynthesisValidationDate { get; set; }

    public string? RejectionCommentOnSynthesisValidation { get; set; }

	public Guid? IdentifyMilestoneValidatedBy { get; set; }

	public Guid? OrganizeAndFinanceMilestoneValidatedBy { get; set; }

	public Guid? RealizeAndFollowMilestoneValidatedBy { get; set; }

	public string? ExternalReference { get; set; }

	public int? AccompanyingTimeDurationForIdentificationMilestone {  get; set; }

	public int? AccompanyingTimeDurationForOrganizeAndFinanceMilestone { get; set; }

	public int? AccompanyingTimeDurationForRealizeAndFollowMilestone { get; set; }

	public Guid? ImportRunId { get; set; }

	public bool? ShouldAccompanyingFileBeSubmittedToAnah {  get; set; }

	public Guid? AbortReasonLabelId { get; set; }

	public string? OtherAbortReason { get; set; }

	public string? AnahFolderNumber { get; set; }

	public DateTime? AnahFolderFilingDate { get; set; }

	public Guid? AbortRequestedById { get; set; }

	public DateTime? AbortRequestedAt { get; set; }

	public string? SolidarBuilderAbortRequestDetails { get; set; }

	public bool? IsAbortBillingRequested { get; set; }

	public bool? HasAbortAttachment { get; set; }

	public Guid? AbortDecidedById { get; set; }

	public DateTime? AbortDecidedAt { get; set; }

	public string? ValidatorAbortComment { get; set; }

	public DateTime? AnahGrantDate { get; set; }

	public Guid? CopropertyProfileId { get; set; }

	public virtual Household AccompanyingFileHouseholdNavigation { get; set; } = null!;

	public virtual Housing AccompanyingFileHousingNavigation { get; set; } = null!;

	public virtual PreFinancingPlan AccompanyingFilePreFinancingPlanNavigation { get; set; } = null!;

	public virtual PreWorkPlan AccompanyingFilePreWorkPlanNavigation { get; set; } = null!;

	public virtual SupportTeam AccompanyingFileSupportTeamNavigation { get; set; } = null!;

	public virtual ICollection<AccompanyingFileTask> AccompanyingFileTasks { get; set; } = new List<AccompanyingFileTask>();

	public virtual Territory? AccompanyingFileTerritoryNavigation { get; set; }

	public virtual WorkMonitoring? AccompanyingFileWorkMonitoringNavigation { get; set; }

	public virtual SiteSupervision? SiteSupervision { get; set; }

	public virtual User? ClosedByNavigation { get; set; }

	public virtual User? CreatedByNavigation { get; set; }

	public virtual ICollection<Invoice> Invoices { get; set; } = [];

	public virtual User? UpdatedByNavigation { get; set; }

	public virtual User? IdentifyMilestoneValidatedByNavigation { get; set; }

	public virtual User? OrganizeAndFinanceMilestoneValidatedByNavigation { get; set; }

	public virtual User? RealizeAndFollowMilestoneValidatedByNavigation { get; set; }
	public virtual AccompanyingFileBillingLog? AccompanyingFileBillingLog { get; set; }

	public virtual ImportRun? ImportRunNavigation { get; set; }

	public virtual AbortReasonLabel? AbortReasonLabel { get; set; }

	public virtual User? AbortRequestedBy { get; set; }

	public virtual User? AbortDecidedBy { get; set; }

	public virtual CopropertyProfile? CopropertyProfileNavigation { get; set; }
}