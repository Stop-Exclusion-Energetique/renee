namespace Renee.Domain.Entity;

public class CopropertyDiagnostics
{
    public Guid Id { get; set; }

    public int? BuildingDpeLabel { get; set; }
    public double? BuildingDpeEnergy { get; set; }

    public int? ApartmentDpeLabel { get; set; }
    public double? ApartmentDpeEnergy { get; set; }

    public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();
}
