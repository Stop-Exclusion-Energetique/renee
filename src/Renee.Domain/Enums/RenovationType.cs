using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum RenovationType
{
	[Description(RenovationTypeLabel.MajorRenovation)]
	MajorRenovation,
	[Description(RenovationTypeLabel.EfficientRenovationInStages)]
	EfficientRenovationInStages
}