using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AnahType
{
	[Description(Labels.GuidedPathwayBonusWithoutUnit)]
	GuidedPathwayBonus,
	[Description(Labels.CoOwnershipBonusWithoutUnit)]
	CoOwnershipBonus,
	[Description(Labels.DecentHousingBonusWithoutUnit)]
	DecentHousingBonus,
	[Description(Labels.GuidedPathwayBonusAdaptationBonus)]
	GuidedPathwayBonusAdaptationBonus
}