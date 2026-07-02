using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum TrustedTierRole
{
	[Description(TrustedTierRoleLabel.Identifier)]
	Identifier,
	[Description(TrustedTierRoleLabel.FinancialMonitoring)]
	FinancialMonitoring,
	[Description(TrustedTierRoleLabel.SocialWorker)]
	SocialWorker,
	[Description(TrustedTierRoleLabel.TechnicalMonitoring)]
	TechnicalMonitoring,
	[Description(TrustedTierRoleLabel.SocialMonitoring)]
	SocialMonitoring,
	[Description(TrustedTierRoleLabel.MainContact)]
	MainContact,
	[Description(TrustedTierRoleLabel.Other)]
	Other
}