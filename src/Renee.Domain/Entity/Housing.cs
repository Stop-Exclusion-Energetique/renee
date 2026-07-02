namespace Renee.Domain.Entity;

public class Housing
{
	public Guid Id { get; set; }

	public Guid HousingAddress { get; set; }

	public Guid HousingInitialState { get; set; }

	public Guid HousingAfterWorkState { get; set; }

	public int? GeographicAreaTypology { get; set; }

	public bool? IsInAbfarea { get; set; }

	public string? ArchitecturalOrTownPlanningStandards { get; set; }

	public int? OwnershipStatus { get; set; }

	public int? HousingType { get; set; }

	public int? ConstructionYear { get; set; }

	public double? LivingSpace { get; set; }

	public int? NumberOfRoom { get; set; }

	public int? NumberOfFloor { get; set; }

	public int? YearOfAcquisitionOrEntry { get; set; }

	public string? CadastralReference { get; set; }

	public int? SunExposure { get; set; }

	public int? NumberOfDoor { get; set; }

	public int? NumberOfWindow { get; set; }

	public int? NumberOfPatioDoor { get; set; }

	public int? NumberOfRoofDoor { get; set; }

	public int? NumberOfBayWindow { get; set; }

	public double? CeilingHeight { get; set; }

	public bool? HasPreviousWork { get; set; }

	public string? CommentOnPreviousWork { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();

	public virtual Address HousingAddressNavigation { get; set; } = null!;

	public virtual HousingAfterWorkState HousingAfterWorkStateNavigation { get; set; } = null!;

	public virtual HousingInitialState HousingInitialStateNavigation { get; set; } = null!;
}