using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum OwnershipStatus
{
	[Description(OwnershipStatusLabel.FullOwnership)]
	FullOwnership,
	[Description(OwnershipStatusLabel.CoOwner)]
	CoOwner,
	[Description(OwnershipStatusLabel.DismemberedBarePropertyOnly)]
	DismemberedBarePropertyOnly,
	[Description(OwnershipStatusLabel.DismemberedUsufructOnly)]
	DismemberedUsurfructOnly,
	[Description(OwnershipStatusLabel.JointOwnership)]
	JointOwnership,
	[Description(OwnershipStatusLabel.PrivateParkTenant)]
	PrivateParkTenant,
	[Description(OwnershipStatusLabel.PublicParkTenant)]
	PublicParkTenant,
	[Description(OwnershipStatusLabel.OccupantFreeOfCharge)]
	OccupantFreeOfCharge,
	[Description(OwnershipStatusLabel.OccupantWithoutRightAndTitle)]
	OccupantWithoutRightAndTitle,
	[Description(OwnershipStatusLabel.RealEstateCompany)]
	RealEstateCompany
}