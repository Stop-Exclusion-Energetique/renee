namespace Renee.Domain.Entity;

public class CopropertyGovernance
{
    public Guid Id { get; set; }

    public int? NatureOfSyndic { get; set; }

    public string? NameOfSyndic { get; set; }

    public string? PhoneOfSyndic { get; set; }

    public string? MailOfSyndic { get; set; }

    public string? NameOfAmo { get; set; }

    public string? ContactOfAmo { get; set; }

    public int? NumberOfContacts { get; set; }

    public virtual ICollection<CopropertyProfile> CopropertyProfiles { get; set; } = new List<CopropertyProfile>();
}
