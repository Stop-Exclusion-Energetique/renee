using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum GeographicalHousingAreaTypology
{
	[Description(GeographicalAreaTypologyLabel.Rural)]
	Rural,
	[Description(GeographicalAreaTypologyLabel.Urban)]
	Urban
}