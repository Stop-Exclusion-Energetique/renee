namespace Renee.Domain.Entity;

public class PreFinancingPlan
{
	public Guid Id { get; set; }

	public double? MaPrimeRenovGuidedPath { get; set; }

	public double? MaPrimeRenovCoOwnerShip { get; set; }

	public double? MaPrimeLogementDecent { get; set; }

	public double? MaPrimeAdapt { get; set; }

	public double? BonusForExitingEnergeticSieve { get; set; }

	public double? RegionalAids { get; set; }

	public double? DepartmentalAids { get; set; }

	public double? PublicEstablishmentsForInterCommunalCooperationAids { get; set; }

	public double? MunicipalityAids { get; set; }

	public string? SolicitedBankLoanType { get; set; }

	public double? ClassicBankLoan { get; set; }

	public string? RemainingAmountFinancingSource { get; set; }

	public bool? IsFinancingAskedToStopAssociation { get; set; }

	public double? EstimatedRemainingAmount { get; set; }

	public double? MdphFinancing { get; set; }

	public double? CeeFinancing { get; set; }

	public double? CafMsaFinancing { get; set; }

	public double? PensionFund { get; set; }

	public double? UnderprivilegedHousingFoundation { get; set; }

	public double? LeroyMerlinFoundation { get; set; }

	public double? WattForChangeFoundation { get; set; }

	public double? SocialProtectionGroup { get; set; }

	public double? StopEnergyExclusionFunds { get; set; }

	public double? HouseholdMaximumSavingAmountForRenovationProject { get; set; }

	public double? OtherFamilyMemberMaximumSupportAmountForRenovationProject { get; set; }

	public virtual ICollection<AccompanyingFile> AccompanyingFiles { get; set; } = new List<AccompanyingFile>();

	public virtual ICollection<FundingMode> FundingModes { get; set; } = new List<FundingMode>();
}