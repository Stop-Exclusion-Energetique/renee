using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum WorkQuality
{
	[Description(WorkQualityLabels.Compliant)]
	Compliant,
	[Description(WorkQualityLabels.Partial)]
	Partial,
	[Description(WorkQualityLabels.NonCompliant)]
	NonCompliant
}