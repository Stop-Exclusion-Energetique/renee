namespace Renee.Application.DTOs.ReportingStructure;

public class ReportingStructureDto(Guid id, string name, Guid? nationalStructureId)
{
	public Guid Id { get; set; } = id;
	public string Name { get; set; } = name;
	public Guid? NationalStructureId { get; set; } = nationalStructureId;
}