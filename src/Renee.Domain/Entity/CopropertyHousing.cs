namespace Renee.Domain.Entity;

public class CopropertyHousing
{
    public Guid Id { get; set; }

    public Guid HousingAddress { get; set; }

    public int? GeographicAreaTypology { get; set; }

    public int? HousingType { get; set; }

    public int? NumberOfLots { get; set; }

    public int? NumberOfFloor { get; set; }

    public int? HeatingType { get; set; }

    public int? PerilType { get; set; }

    public virtual Address HousingAddressNavigation { get; set; } = null!;
    public  virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();

}
