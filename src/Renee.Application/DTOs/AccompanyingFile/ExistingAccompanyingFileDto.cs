namespace Renee.Application.DTOs.AccompanyingFile;

public class ExistingAccompanyingFileDto
{
    public Guid Id { get; set; }
    public string? ExternalReference { get; set; }
    public Guid? MainOccupantId { get; set; }
    public Guid? AddressId { get; set; }
}
