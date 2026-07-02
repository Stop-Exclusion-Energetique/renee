namespace Renee.Domain.Entity;

public class HousingInitialState
{
	public Guid Id { get; set; }

	public int? DegradationIndex { get; set; }

	public int? UnsanitaryCoefficient { get; set; }

	public int? EnergyDepravation { get; set; }

	public int? SummerThermalComfortLevel { get; set; }

	public int? WinterThermalComfortLevel { get; set; }

	public int? NoiseComfortLevel { get; set; }

	public int? RoofingState { get; set; }

	public int? WallsState { get; set; }

	public int? FloorState { get; set; }

	public int? ElectricalSafetyState { get; set; }

	public int? GasSafetyState { get; set; }

	public int? FireSafetyState { get; set; }

	public int? VentilationState { get; set; }

	public int? CarpentryState { get; set; }

	public int? HeatingState { get; set; }

	public int? HotWaterProductionState { get; set; }

	public int? HumidityState { get; set; }

	public int? LeadAndAsbestosState { get; set; }

	public int? SanitaryPlumbingState { get; set; }

	public int? SanitationState { get; set; }

	public int? InteriorDesignState { get; set; }

	public string? InitialStateDiagnosticCommentary { get; set; }

	public bool? HasPestOrMold { get; set; }

	public bool? HasFaultyElectricalSystem { get; set; }

	public bool? HasVentilationSystem { get; set; }

	public bool? HasHeatingSystem { get; set; }

	public bool? HasHotWaterProduction { get; set; }

	public bool? HasHousingCover { get; set; }

	public bool? HasOpenings { get; set; }

	public bool? HasInsulation { get; set; }

	public string? DisordersObservedCommentary { get; set; }

	public int? Dpe { get; set; }

	public int? Ges { get; set; }

	public double? AnnualEnergyConsumption { get; set; }

	public double? AnnualGesemission { get; set; }

	public string? HeatingEnergy { get; set; }

	public virtual ICollection<Housing> Housings { get; set; } = new List<Housing>();
}