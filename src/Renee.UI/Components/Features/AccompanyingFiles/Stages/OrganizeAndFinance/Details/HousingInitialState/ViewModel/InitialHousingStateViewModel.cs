using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.HousingInitialState.
	ViewModel;

public class InitialHousingStateViewModel
{
	public int? RoomCounter { get; set; }
	public int? DoorCounter { get; set; }
	public int? WindowCounter { get; set; }
	public int? PatioDoorCounter { get; set; }
	public int? RoofWindowCounter { get; set; }
	public int? BayWindowCounter { get; set; }
	public double? CeilingHeight { get; set; }
	public SunExposure? SunExposure { get; set; }
	public bool? HasPestOrMold { get; set; }
	public bool? HasFaultyElectricalSystem { get; set; }
	public bool? HasVentilationSystem { get; set; }
	public bool? HasHeatingSystem { get; set; }
	public bool? HasHotWaterProduction { get; set; }
	public bool? HasOpenings { get; set; }
	public bool? HasInsulation { get; set; }
	public bool? HasHousingCover { get; set; }
	public string? DisordersObservedCommentary { get; set; }
}