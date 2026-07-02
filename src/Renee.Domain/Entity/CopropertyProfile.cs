namespace Renee.Domain.Entity;

public class CopropertyProfile
{
    public Guid Id { get; set; }

    public string CopropertyReference { get; set; } = null!;

    public int CopropertyMilestone { get; set; }

    public int CopropertyStatus { get; set; }

    public int? AccompanyingType { get; set; }

    public bool ZeroEnergyExclusionTerritoriesProgram { get; set; }

    public Guid CopropertyHousing { get; set; }

    public Guid CopropertyGovernance { get; set; }

    public Guid CopropertyDiagnostics { get; set; }

    public Guid CopropertyWorkFinance { get; set; }

    public Guid CopropertyWorkTraking { get; set; }

    public Guid CopropertySupportTeam { get; set; }

    public Guid? CopropertyTerritory { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public Guid? ClosedBy { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? LastUpdateDate { get; set; }

    public DateTime? CloseDate { get; set; }

    public DateTime? IdentifySynthesisValidationDate { get; set; }

    public DateTime? OrganizeAndFinanceSynthesisValidationDate { get; set; }

    public DateTime? RealizeAndFollowSynthesisValidationDate { get; set; }

    public string? RejectionCommentOnSynthesisValidation { get; set; }

    public Guid? IdentifyMilestoneValidatedBy { get; set; }

    public Guid? OrganizeAndFinanceMilestoneValidatedBy { get; set; }

    public Guid? RealizeAndFollowMilestoneValidatedBy { get; set; }

    public virtual CopropertyHousing CopropertyHousingNavigation { get; set; } = null!;

    public virtual CopropertyGovernance CopropertyGovernanceNavigation { get; set; } = null!;

    public virtual CopropertyDiagnostics CopropertyDiagnosticsNavigation { get; set; } = null!;

    public virtual CopropertyWorkFinance CopropertyWorkFinanceNavigation { get; set; } = null!;

    public virtual CopropertyWorkTracking CopropertyWorkTrackingNavigation { get; set;} = null!;

    public virtual SupportTeam CopropertySupportTeamNavigation { get; set; } = null!;

    public virtual Territory? CopropertyTerritoryNavigation { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User? ClosedByNavigation { get; set; }

    public virtual User? IdentifyMilestoneValidatedByNavigation { get; set; }

    public virtual User? OrganizeAndFinanceMilestoneValidatedByNavigation { get; set; }

    public virtual User? RealizeAndFollowMilestoneValidatedByNavigation { get; set; }
    
    public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();
}
