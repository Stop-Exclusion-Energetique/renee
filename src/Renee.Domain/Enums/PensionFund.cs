using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum PensionFund
{
	[Description(PensionFundLabel.Cnav)] Cnav,
	[Description(PensionFundLabel.Carsat)] Carsat,
	[Description(PensionFundLabel.Cram)] Cram,
	[Description(PensionFundLabel.Crav)] Crav,
	[Description(PensionFundLabel.AgriculturalSocialMutuality)]
	AgriculturalSocialMutuality,
	[Description(PensionFundLabel.AgriculturalSocialMutualityEmployee)]
	AgriculturalSocialMutualityEmployee,
	[Description(PensionFundLabel.RetirementInsurance)]
	RetirementInsurance,
	[Description(PensionFundLabel.Other)] Other
}