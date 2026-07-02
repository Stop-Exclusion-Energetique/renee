using System.ComponentModel.DataAnnotations;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.Components.
	FundingMode.ViewModel;
using Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreWorkPlanTab.Components.
	WorkPackage.ViewModel;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.PreFinancingPlan.ViewModel;

public class PreFinancingPlanViewModel
{
	public DateTime? DateOfAgVote { get; set; }
	public double? MprCoproAids { get; set; }
	public double? ComplementaryCopropertyAids { get; set; }
	public List<WorkPackageViewModel> CopropertyWorkPackages { get; init; } = [];

	public double? NationalHousingAgencyTotal =>
		Math.Round(
			(GuidedPathwayBonus ?? 0) +
			(CoOwnershipBonus ?? 0) +
			(DecentHousingBonus ?? 0) +
			(AdaptationBonus ?? 0) +
			(ExitEnergySieveBonus ?? 0));

	public double? LocalPublicAidTotalAmount =>
		Math.Round(
			(Region ?? 0) +
			(Department ?? 0) +
			(PublicEstablishmentsIntercommunalCooperation ?? 0) +
			(Municipality ?? 0));

	public double? OtherPublicAidTotalAmount => 
		Math.Round(
			(DepartmentalHouseForDisabledPersons ?? 0) +
			(FamilyAllowanceFund ?? 0));

	public double? PrivateAidTotalAmount =>
		Math.Round(
			(EnergySavingCertificates ?? 0) +
			(PensionFund?? 0) +
			(UnderprivilegedHousingFoundation ?? 0) +
			(LeroyMerlinFoundation ?? 0) +
			(WattForChangeFoundation ?? 0) +
			(SocialProtectionGroup ?? 0) +
			(StopEnergyExclusionFunds ?? 0) +
			TotalFundingModes);

	public double? OwnedFundsTotalAmount =>
		Math.Round(
			(HouseholdMaximumSavingAmountForRenovationProject ?? 0) +
			(MaximumAmountSupportFamilyMembersRenovationProject ?? 0));

	public double? PublicAidTotalAmount =>
		(NationalHousingAgencyTotal ?? 0) +
		(LocalPublicAidTotalAmount ?? 0) +
		(OtherPublicAidTotalAmount ?? 0);

	public double? AidTotalAmount =>
		(PublicAidTotalAmount ?? 0) +
		(PrivateAidTotalAmount ?? 0);

	public double? RemainingAmountAfterPublicAidDeduction =>
		(TotalWorkCost ?? 0) -
		(PublicAidTotalAmount ?? 0);

	public double? RemainingAmountAfterPublicAndPrivateAidDeduction =>
		(TotalWorkCost ?? 0) -
		(AidTotalAmount ?? 0);

	public double PreFinancingPlanTotal =>
		Math.Round(
			(AidTotalAmount ?? 0) +
			(ClassicBankLoan ?? 0) +
			(OwnedFundsTotalAmount ?? 0));

	public double? GuidedPathwayBonus { get; set; }
	public double? CoOwnershipBonus { get; set; }
	public double? DecentHousingBonus { get; set; }
	public double? AdaptationBonus { get; set; }
	public double? ExitEnergySieveBonus { get; set; }
	public double? Region { get; set; }
	public double? Department { get; set; }
	public double? PublicEstablishmentsIntercommunalCooperation { get; set; }
	public double? Municipality { get; set; }
	public string? BankLoanType { get; set; }
	public double? ClassicBankLoan { get; set; }
	public double? DepartmentalHouseForDisabledPersons { get; set; }
	public double? EnergySavingCertificates { get; set; }
	public double? FamilyAllowanceFund { get; set; }
	public double? PensionFund { get; set; }
	public double? UnderprivilegedHousingFoundation { get; set; }
	public double? LeroyMerlinFoundation { get; set; }
	public double? WattForChangeFoundation { get; set; }
	public double? SocialProtectionGroup { get; set; }
	public double? StopEnergyExclusionFunds { get; set; }
	public double? HouseholdMaximumSavingAmountForRenovationProject { get; set; }
	public double? MaximumAmountSupportFamilyMembersRenovationProject { get; set; }

	[ValidateComplexType] public List<FundingModeViewModel> FundingModes { get; init; } = [];
	public double TotalFundingModes => FundingModes.Sum(fm => fm.Amount ?? 0);

	public double PreFinancingPlanBalancing =>
		Math.Round((TotalWorkCost ?? 0) - PreFinancingPlanTotal);

	public double AbsoluteValueGapBetweenWorkPackagesAndPreFinancingPlan =>
		Math.Round(Math.Abs((TotalWorkCost ?? 0) - PreFinancingPlanTotal));

	public double? TotalWorkCost { get; set; }
}