using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum EnergyDeprivation
{
	[Description(EnergyDeprivationLabel.None)]
	None,
	[Description(EnergyDeprivationLabel.Partial)]
	Partial,
	[Description(EnergyDeprivationLabel.Total)]
	Total
}