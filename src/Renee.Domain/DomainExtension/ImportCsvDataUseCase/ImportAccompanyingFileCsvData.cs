using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension.ImportCsvDataUseCase;

public class ImportAccompanyingFileCsvData
{
    public Guid UserId { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string? ExternalReference {  get; set; } = string.Empty;
    public DateTime? FirstContactDate { get; set; }
    public DateTime? StartSupportDate { get; set; }
    public DateTime? FileOpeningDate { get; set; }
    public DateTime? FileClosingDate { get; set; }
    public DateTime? EndSupportDate { get; set; }
    public DateTime? EndContactDate { get; set; }
    public AccompanyingTimeDuration? AccompanyingTimeDurationForIdentificationMilestone { get; set; }
    public AccompanyingTimeDuration? AccompanyingTimeDurationForOrganizeAndFinanceMilestone { get; set; }
    public AccompanyingTimeDuration? AccompanyingTimeDurationForRealizeAndFollowMilestone { get; set; }
    public bool? ZeroEnergyExclusionTerritoriesProgram { get; set; }
    public AccompanyingType? AccompanyingType { get; set; }
    public Guid? TerritoryId { get; set; }
    public bool? IsDeleted { get; set; } 
    public Guid? ImportRunId { get; set; }
}