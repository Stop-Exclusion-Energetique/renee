using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum ComfortLevel
{
	[Description(ComfortLevelLabel.Bad)] Bad,
	[Description(ComfortLevelLabel.Medium)]
	Medium,
	[Description(ComfortLevelLabel.Good)] Good
}