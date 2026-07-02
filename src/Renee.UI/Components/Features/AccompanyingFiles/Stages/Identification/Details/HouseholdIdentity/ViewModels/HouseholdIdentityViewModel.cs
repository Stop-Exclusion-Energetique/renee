using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class HouseholdIdentityViewModel(
	HouseholdViewModel householdViewModel,
	MainOccupantViewModel mainOccupant,
	List<SecondaryOccupantViewModel> secondaryOccupants)
{
	[ValidateComplexType] public HouseholdViewModel HouseholdViewModel { get; } = householdViewModel;
	[ValidateComplexType] public MainOccupantViewModel MainOccupantViewModel { get; } = mainOccupant;
	[ValidateComplexType]
	public List<SecondaryOccupantViewModel> SecondaryOccupantViewModels { get; } = secondaryOccupants;
}