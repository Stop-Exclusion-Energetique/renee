namespace Renee.Domain.Entity;

public class SupportTeam
{
    public Guid Id { get; set; }

    public Guid SolidarBuilder { get; set; }

    public Guid? TerritorialBuilder { get; set; }

    public Guid? SecondTerritorialBuilder { get; set; }

    public int MarkerNature { get; set; }

    public string? CommentOnMarkerNature { get; set; }

    public string? TrustedTierFirstName { get; set; }

    public string? TrustedTierStructureName { get; set; }

    public string? TrustedTierLastName { get; set; }

    public string? TrustedTierPhoneNumber { get; set; }

    public string? TrustedTierEmail { get; set; }

    public int? TrustedTierRole { get; set; }

    public string? CommentOnTrustedTierRole { get; set; }

    public Guid? SecondSolidarBuilder { get; set; }

    public Guid? DiffuseCoordinator { get; set; }

    public Guid? TargetCoordinator { get; set; }

    public Guid? ThirdSolidarBuilder { get; set; }

    public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();

    public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();

    public virtual User? DiffuseCoordinatorNavigation { get; set; }

    public virtual User? SecondSolidarBuilderNavigation { get; set; }

    public virtual User SolidarBuilderNavigation { get; set; } = null!;

    public virtual User? TargetCoordinatorNavigation { get; set; }

    public virtual User? TerritorialBuilderNavigation { get; set; }

    public virtual User? SecondTerritorialBuilderNavigation { get; set; }

    public virtual User? ThirdSolidarBuilderNavigation { get; set; }
}