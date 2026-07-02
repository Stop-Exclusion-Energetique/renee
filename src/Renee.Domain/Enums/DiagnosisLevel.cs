using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum DiagnosisLevel
{
	[Description(DiagnosisLevelLabel.VeryBad)]
	VeryBad,
	[Description(DiagnosisLevelLabel.Bad)] Bad,
	[Description(DiagnosisLevelLabel.Poor)]
	Poor,
	[Description(DiagnosisLevelLabel.Good)]
	Good
}