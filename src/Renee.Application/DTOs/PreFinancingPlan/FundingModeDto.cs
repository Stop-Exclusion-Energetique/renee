using Renee.Domain.Entity;

namespace Renee.Application.DTOs.PreFinancingPlan;

public class FundingModeDto(Guid? id, string? label, double? value)
{
	public Guid Id { get; } = id ?? Guid.Empty;
	public string Label { get; } = label ?? string.Empty;
	public double Value { get; } = value ?? 0;

	public FundingMode CreateUpdateFundingMode() => new FundingMode
	{
		Id = Id,
		Label = Label,
		Value = Value
	};
}