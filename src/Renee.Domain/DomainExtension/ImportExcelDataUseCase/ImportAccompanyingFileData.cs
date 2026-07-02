using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension.ImportExcelDataUseCase;

public class ImportAccompanyingFileData
{
	public Guid UserId { get; set; }
	public string Reference { get; set; } = string.Empty;
	public DateTime? FirstContactDate { get; set; }
	public DateTime? StartSupportDate { get; set; }
	public DateTime? EndSupportDate { get; set; }
	public DateTime? EndContactDate { get; set; }
	public int? ContactWithFamilyDuringIdentifyStage { get; set; }
	public int? ContactWithFamilyDuringOrganizeAndFinanceStage { get; set; }
	public int? ContactWithFamilyDuringRealizeAndFollowStage { get; set; }
	public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }
	public AccompanyingType? AccompanyingType { get; set; }
	public Guid? TerritoryId { get; set; }
}
