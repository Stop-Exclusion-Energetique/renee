using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum SocialProtectionFund
{
	[Description(SocialProtectionFundLabel.Cpam)]
	Cpam,
	[Description(SocialProtectionFundLabel.Caf)]
	Caf,
	[Description(SocialProtectionFundLabel.Urssaf)]
	Urssaf,
	[Description(SocialProtectionFundLabel.Carsat)]
	Carsat,
	[Description(SocialProtectionFundLabel.Cgss)]
	Cgss,
	[Description(SocialProtectionFundLabel.Rsi)]
	Rsi,
	[Description(SocialProtectionFundLabel.Msa)]
	Msa,
	[Description(SocialProtectionFundLabel.Other)]
	Other
}