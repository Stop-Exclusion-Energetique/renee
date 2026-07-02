using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum SunExposure
{
	[Description(Labels.East)] East,
	[Description(Labels.NorthEast)] NorthEast,
	[Description(Labels.North)] North,
	[Description(Labels.NorthWest)] NorthWest,
	[Description(Labels.West)] West,
	[Description(Labels.SouthWest)] SouthWest,
	[Description(Labels.South)] South,
	[Description(Labels.SouthEast)] SouthEast
}