using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AccompanyingType
{
	[Description(AccompanyingTypeLabel.Targeted)]
	Targeted,
	[Description(AccompanyingTypeLabel.Diffuse)]
	Diffuse
}