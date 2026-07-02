namespace Renee.Domain.Entity;

public class NationalStructure
{
	public Guid Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<ReportingStructure> ReportingStructures { get; set; } = new List<ReportingStructure>();
}