using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum HousingType
{
	[Description(HousingTypeLabel.IndividualHouse)]
	IndividualHouse,
	[Description(HousingTypeLabel.ResidentialCollective)]
	ResidentialCollective
}