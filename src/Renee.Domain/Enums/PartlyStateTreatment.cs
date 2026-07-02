using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum PartlyStateTreatment
{
	[Description(PartlyStateTreatmentLabel.Yes)]
	Yes,
	[Description(PartlyStateTreatmentLabel.No)]
	No,
	[Description(PartlyStateTreatmentLabel.Partly)]
	Partly
}