using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum HouseholdTypology
{
	[Description(HouseholdTypologyLabel.CoupleWithAdultStaying)]
	CoupleWithAdultStaying,
	[Description(HouseholdTypologyLabel.CoupleWithChildren)]
	CoupleWithChildren,
	[Description(HouseholdTypologyLabel.CoupleWithoutChildren)]
	CoupleWithoutChildren,
	[Description(HouseholdTypologyLabel.SingleParentFamily)]
	SingleParentFamily,
	[Description(HouseholdTypologyLabel.SinglePerson)]
	SinglePerson,
	[Description(HouseholdTypologyLabel.SinglePersonWithAdultStaying)]
	SinglePersonWithAdultStaying
}