using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AdditionalFund
{
	[Description(AdditionalFundLabel.Agir)]
	Agir,
	[Description(AdditionalFundLabel.Arrco)]
	Arrco,
	[Description(AdditionalFundLabel.Ircantec)]
	Ircantec,
	[Description(AdditionalFundLabel.Rafp)]
	Rafp,
	[Description(AdditionalFundLabel.Cgss)]
	Cgss,
	[Description(AdditionalFundLabel.Other)]
	Other
}