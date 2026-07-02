using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum MarkerNature
{
	[Description(MarkerNatureLabels.PublicActor)]
	PublicActor,
	[Description(MarkerNatureLabels.Association)]
	Association,
	[Description(MarkerNatureLabels.Volunteers)]
	Volunteers,
	[Description(MarkerNatureLabels.HealthFunds)]
	HealthFunds,
	[Description(MarkerNatureLabels.CityHall)]
	CityHall,
	[Description(MarkerNatureLabels.Operator)]
	Operator,
	[Description(MarkerNatureLabels.Slime)]
	Slime,
	[Description(MarkerNatureLabels.DirectCall)]
	DirectCall,
	[Description(MarkerNatureLabels.Other)]
	Other
}

public enum MarkerNatureV3
{
	[Description(MarkerNatureLabels.PublicActorv3)]
	PublicActor,
	[Description(MarkerNatureLabels.Association)]
	Association,
	[Description(MarkerNatureLabels.Volunteers)]
	Volunteers,
	[Description(MarkerNatureLabels.HealthFunds)]
	HealthFunds,
	[Description(MarkerNatureLabels.CityHall)]
	CityHall,
	[Description(MarkerNatureLabels.Operator)]
	Operator,
	[Description(MarkerNatureLabels.Slime)]
	Slime,
	[Description(MarkerNatureLabels.DirectCall)]
	DirectCall,
	[Description(MarkerNatureLabels.Other)]
	Other
}