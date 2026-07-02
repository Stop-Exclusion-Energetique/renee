namespace Renee.Domain.Entity;

public class User
{
	public Guid Id { get; set; }

	public Guid RoleId { get; set; }

	public string? Email { get; set; }

	public string? LastName { get; set; }

	public string? FirstName { get; set; }

	public string? PhoneNumber { get; set; }

	public Guid? TerritoryId { get; set; }

	public string? ReportingStructure { get; set; }

	public string? Function { get; set; }

	public Guid? ReportingStructureId { get; set; }

	public string? SiretNumber { get; set; }

	public string? LastValidatedCGUVersion {  get; set; }
	public DateTime? LastValidatedCGUDate { get; set; }

	public bool IsDeleted { get; set; }

	public bool IsAccountDeletionRequested { get; set; }

	public bool? IsFakeUser { get; set; }

	public DateTime? LastLoginDate { get; set; }

	public DateTime NextDateForAnahGrantCheck { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFileClosedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileCreatedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFileTask> AccompanyingFileTasks { get; set; } =
		new List<AccompanyingFileTask>();

	public virtual ICollection<AccompanyingFileTask> CreatedAccompanyingFileTasks { get; set; } =
		new List<AccompanyingFileTask>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileUpdatedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileIdentifyMilestoneValidatedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileOrganizeAndFinanceMilestoneValidatedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileRealizeAndFollowMilestoneValidatedByNavigations { get; set; } =
		new List<AccompanyingFile>();

	public virtual ICollection<AccompanyingFile> AccompanyingFileAbortRequestedByNavigations { get; set; } = [];

	public virtual ICollection<AccompanyingFile> AccompanyingFileAbortDecidedByNavigations { get; set; } = [];

	public virtual ICollection<CopropertyProfile> CopropertyProfileCreatedByNavigations { get; set; } = 
		new List<CopropertyProfile>();
	public virtual ICollection<CopropertyProfile> CopropertyProfileUpdatedByNavigations { get; set; } = 
		new List<CopropertyProfile>();
		
	public virtual ICollection<CopropertyProfile> CopropertyProfileClosedByNavigations { get; set; } = 
		new List<CopropertyProfile>();

    public virtual ICollection<CopropertyProfile> CopropertyIdentifyMilestoneValidatedByNavigations { get; set; } =
		new List<CopropertyProfile>();

    public virtual ICollection<CopropertyProfile> CopropertyOrganizeAndFinanceMilestoneValidatedByNavigations { get; set; } =
        new List<CopropertyProfile>();

    public virtual ICollection<CopropertyProfile> CopropertyRealizeAndFollowMilestoneValidatedByNavigations { get; set; } =
        new List<CopropertyProfile>();

    public virtual ICollection<AnahCategory> AnahCategoryCreatedBies { get; set; } = new List<AnahCategory>();

	public virtual ICollection<AnahCategory> AnahCategoryLastUpdateBies { get; set; } = new List<AnahCategory>();

	public virtual Territory? Territory { get; set; }

	public virtual ReportingStructure? ReportingStructureNavigation { get; set; }

	public virtual Role Role { get; set; } = null!;

	public virtual ICollection<SupportTeam> SupportTeamDiffuseCoordinatorNavigations { get; set; } =
		new List<SupportTeam>();

	public virtual ICollection<SupportTeam> SupportTeamSecondSolidarBuilderNavigations { get; set; } =
		new List<SupportTeam>();

	public virtual ICollection<SupportTeam> SupportTeamSolidarBuilderNavigations { get; set; } =
		new List<SupportTeam>();

	public virtual ICollection<SupportTeam> SupportTeamTargetCoordinatorNavigations { get; set; } =
		new List<SupportTeam>();

	public virtual ICollection<SupportTeam> SupportTeamTerritorialBuilderNavigations { get; set; } =
		new List<SupportTeam>();

    public virtual ICollection<SupportTeam> SupportTeamSecondTerritorialBuilderNavigations { get; set; } = 
		new List<SupportTeam>();


    public virtual ICollection<SupportTeam> SupportTeamThirdSolidarBuilderNavigations { get; set; } =
		new List<SupportTeam>();
}