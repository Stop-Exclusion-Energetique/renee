namespace Renee.Domain.Entity;

public class HousingAfterWorkState
{
	public Guid Id { get; set; }

	public int? EstimatedDpeclassJump { get; set; }

	public int? EstimatedDpeafterWork { get; set; }

	public double? EstimatedAnnualEnergyConsumptionAfterWork { get; set; }

	public double? EstimatedAnnualGesemissionsAfterWork { get; set; }

	public int? EstimatedGesafterWork { get; set; }

	public int? FinalDpe { get; set; }

	public int? FinalDpeClassJump { get; set; }

	public virtual ICollection<Housing> Housings { get; set; } = new List<Housing>();
}