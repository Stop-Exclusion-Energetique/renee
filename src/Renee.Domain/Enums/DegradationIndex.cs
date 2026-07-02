using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum DegradationIndex
{
	[Description(Labels.LowDegradationIndex)]
	Low,
	[Description(Labels.MediumDegradationIndex)]
	Medium,
	[Description(Labels.HighDegradationIndex)]
	High
}