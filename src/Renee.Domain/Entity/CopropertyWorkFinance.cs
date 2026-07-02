namespace Renee.Domain.Entity;

public class CopropertyWorkFinance
{
    public Guid Id { get; set; }

    public DateTime? DateOfAgVote { get; set; }

    public double? MprCoproAids { get; set; }

    public double? ComplementaryAids { get; set; }

    public virtual ICollection<WorkPackage> WorkPackages { get; set; } = new List<WorkPackage>();

    public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();
}
