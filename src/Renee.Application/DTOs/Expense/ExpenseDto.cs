using Renee.Domain.Enums;

namespace Renee.Application.DTOs.Expense;

public class ExpenseDto
{
	public Guid? Id { get; init; }
	public ExpenseType Type { get; init; }
	public double? Value { get; init; }
}