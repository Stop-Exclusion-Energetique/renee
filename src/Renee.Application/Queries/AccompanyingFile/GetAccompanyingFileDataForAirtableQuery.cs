using Renee.Application.Abstraction.Query;
using Renee.Domain.ReneeError;

namespace Renee.Application.Queries.AccompanyingFile;

public class GetAccompanyingFileDataForAirtableQuery : IQuery<ReneeOperationResult<GetAccompanyingFileDataForAirtableQueryObjectResult?>>
{
    public string AccompanyingFileReference { get; set;} = string.Empty;
    public Guid UserId { get; set;}
}

public record GetAccompanyingFileDataForAirtableQueryObjectResult
{
    public int NumberOfPeopleInHousehold { get; init; }
    public string? MainOccupantLastName { get; init; }
    public int HousingPostalCode { get; init; }
    public string? HousingCity { get; init; }
    public double? IncomeTaxReference { get; init; }
    public string? HouseholdResourcesTypology { get; init; }
    public string? HouseholdDebt { get; init; }
    public string? HouseholdSocialSituation { get; init; }
    public string? HousingType { get; init; }
    public int? HousingYearOfConstruction { get; init; }
    public double? HousingSurface { get; init; }
    public double? EnergeticPerformanceBeforeRenovation { get; init; }
    public double? EnergeticPerformanceAfterRenovation { get; init; }
    public double? GesEmissionBeforeRenovation { get; init; }
    public double? GesEmissionAfterRenovation { get; init; }
    public string? DpeLabelBeforeRenovation { get; init; }
    public string? DpeLabelAfterRenovation { get; init; }
    public double TotalDevisValue { get; init; }
    public string? AraDescription { get; init; }
	public double? AnahTotalAid { get; init; }
    public double? RegionAid { get; init; }
    public double? DepartmentAid { get; init; }
    public double? CityAid { get; init; }
    public double? CommunityAid { get; init; }
    public double? CeeAid { get; init; }
    public double? UnderprivilegedHousingFoundation { get; init; }
    public double? SocialProtectionGroupAid { get; init; }
    public double? MdphAid { get; init; }
    public double? StopFoundAidAsked { get; init; }
    public double? HouseholdMaximumSavingsForRenovation { get; init; }
    public double? HouseholdMaximalFamilyAid { get; init; }
    public string? TypeOfBankLoanRequested { get; init; }
    public string? EnergyDepravation { get; init; }
    public double? FamilyMonthlyIncome { get; init; }
    public string? HotWaterSystemDescription { get; init; }
    public bool? IsInStopProgram { get; init; }
    public double? WattForChangeAIds { get; init; }
}