using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum FundingType
{
	[Description(Labels.Region)] Region,
	[Description(Labels.Department)] Department,
	[Description(Labels.PublicEstablishmentsIntercommunalCooperationWithoutUnit)]
	PublicEstablishmentsInterCooperation,
	[Description(Labels.Municipality)] Municipality,
	[Description(Labels.PrivateAid)] PrivateActors,
	[Description(Labels.PensionFunds)] PensionFunds,
	[Description(Labels.HouseholdMaximumSavingAmountForRenovationProject)]
	HouseholdMaximumSavingAmountForRenovationProject,
	[Description(Labels.MaximumAmountSupportFamilyMembersRenovationProject)]
	MaximumAmountSupportFamilyMembersRenovationProject
}