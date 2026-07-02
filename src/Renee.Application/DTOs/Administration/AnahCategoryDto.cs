namespace Renee.Application.DTOs.Administration;

public record AnahCategoryData(
	int? PeopleNumber,
	double? VeryLowIncomeHouseholdsAmount,
	double? LowIncomeHouseholdsAmount,
	int? Year,
	DateTime? RuleStartDate,
	DateTime? RuleEndDate);

public sealed class AnahCategoryDto
{
	public Guid Id { get; init; }
	public int? PeopleNumber { get; set; }
	public double? VeryLowIncomeHouseholdsAmount { get; set; }
	public double? LowIncomeHouseholdsAmount { get; set; }
	public bool IsInIleDeFrance { get; set; }
	public int? Year { get; set; }
	public DateTime? RuleStartDate { get; set; }
	public DateTime? RuleEndDate { get; set; }

	public AnahCategoryDto()
	{
	}

	public AnahCategoryDto(
		Guid id,
		AnahCategoryData anahCategoryData,
		bool isInIleDeFrance)
	{
		Id = id;
		PeopleNumber = anahCategoryData.PeopleNumber;
		VeryLowIncomeHouseholdsAmount = anahCategoryData.VeryLowIncomeHouseholdsAmount;
		LowIncomeHouseholdsAmount = anahCategoryData.LowIncomeHouseholdsAmount;
		IsInIleDeFrance = isInIleDeFrance;
		Year = anahCategoryData.Year;
		RuleStartDate = anahCategoryData.RuleStartDate;
		RuleEndDate = anahCategoryData.RuleEndDate;
	}
}