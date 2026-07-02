namespace Renee.Domain.Entity;

public class Address
{
	public Guid Id { get; set; }

	public string Label { get; set; } = null!;

	public string PostalCode { get; set; } = null!;

	public string City { get; set; } = null!;

	public string Department { get; set; } = null!;

	public string Region { get; set; } = null!;

	public string? AdditionnalComment { get; set; }

	public string? HouseNumber { get; set; }

	public string? Street { get; set; }

	public virtual ICollection<Housing> Housings { get; set; } = new List<Housing>();
    public virtual ICollection<CopropertyHousing> CopropertyHousings { get; set; } = new List<CopropertyHousing>();
}