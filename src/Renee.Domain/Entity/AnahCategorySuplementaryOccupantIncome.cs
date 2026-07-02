namespace Renee.Domain.Entity;

public class AnahCategorySuplementaryOccupantIncome
{
	public Guid Id { get; set; } 
	public double? SuplementaryOccupantVerylowIncome { get; set; }
	public double? SuplementaryOcupantLowIncome { get; set; }
	public bool? IsInIleDeFrance { get; set; }
	public DateTime? StartRuleDate { get; set; }
	public DateTime? EndRuleDate { get; set; }
}
