using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum UnsanitaryCoefficient
{
	[Description(Labels.LowUnsanitaryCoefficient)]
	Low,
	[Description(Labels.MediumUnsanitaryCoefficient)]
	Medium,
	[Description(Labels.HighUnsanitaryCoefficient)]
	High
}