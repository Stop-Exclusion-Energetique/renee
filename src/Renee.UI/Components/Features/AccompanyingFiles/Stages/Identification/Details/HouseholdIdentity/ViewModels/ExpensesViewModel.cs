using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.Expense;
using Renee.Domain;
using Renee.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class ExpensesViewModel
{
	public double? EnergeticTotal => (MonthlyEnergeticsExpense.Value ?? 0) * 12;

	public double? FixedTotal =>
		(Housing.Value ?? 0) +
		(Insurance.Value ?? 0) +
		(Taxes.Value ?? 0) +
		(Phone.Value ?? 0) +
		(Internet.Value ?? 0) +
		(Children.Value ?? 0) +
		(MonthlyEnergeticsExpense.Value ?? 0) +
		(CreditRepayment.Value ?? 0) +
		(Water.Value ?? 0) +
		(Mutual.Value ?? 0);

	public double? Total =>
		(FixedTotal ?? 0) +
		(MensualityTotal ?? 0) +
		(Travel.Value ?? 0) +
		(Clothing.Value ?? 0) +
		(Furniture.Value ?? 0);

	public double? CasualExpenseTotal => (CreditRepayment.Value ?? 0) + (Children.Value ?? 0) + (Housing.Value ?? 0);
	public ExpenseModel Housing { get; private set; } = new();
	public ExpenseModel Electricity { get; set; } = new();
	public ExpenseModel Gas { get; set; } = new();
	public ExpenseModel Water { get; set; } = new();
	public ExpenseModel Insurance { get; private set; } = new();
	public ExpenseModel Taxes { get; private set; } = new();
	public ExpenseModel Phone { get; private set; } = new();
	public ExpenseModel Internet { get; set; } = new();
	public ExpenseModel Children { get; private set; } = new();
	public ExpenseModel CreditRepayment { get; private set; } = new();
	public ExpenseModel Food { get; private set; } = new();
	public ExpenseModel Transport { get; private set; } = new();
	public ExpenseModel Health { get; private set; } = new();
	public ExpenseModel Leisure { get; private set; } = new();
	public ExpenseModel Clothing { get; private set; } = new();
	public ExpenseModel Travel { get; private set; } = new();
	public ExpenseModel Furniture { get; private set; } = new();
	public ExpenseModel Mutual { get; private set; } = new();
	[ValidateComplexType]
	public ExpenseModel MonthlyEnergeticsExpense { get; set; } = new(ExpenseType.MonthlyEnergecticsExpenses);

	private double? MensualityTotal =>
		(Food.Value ?? 0) + (Transport.Value ?? 0) + (Health.Value ?? 0) + (Leisure.Value ?? 0);

	public static ExpensesViewModel MapFromDtoList(ICollection<ExpenseDto> expenseDtOs)
	{
		var viewModel = new ExpensesViewModel();
		foreach (var item in expenseDtOs)
			switch (item.Type)
			{
				case ExpenseType.Housing: 
					viewModel.Housing = new ExpenseModel(item.Id, item.Type, item.Value);
					break;
				case ExpenseType.Electricity: 
					viewModel.Electricity = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Gas: 
					viewModel.Gas = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Water: 
					viewModel.Water = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Insurance: 
					viewModel.Insurance = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Taxes: 
					viewModel.Taxes = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Phone: 
					viewModel.Phone = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Internet: 
					viewModel.Internet = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Children: 
					viewModel.Children = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.CreditRepayment:
					viewModel.CreditRepayment = new ExpenseModel(item.Id, item.Type, item.Value);
					break;
				case ExpenseType.Food: 
					viewModel.Food = new ExpenseModel(item.Id, item.Type, item.Value);
					break;
				case ExpenseType.Transport: 
					viewModel.Transport = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Health: 
					viewModel.Health = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Leisure: 
					viewModel.Leisure = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Clothing: 
					viewModel.Clothing = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Travel: 
					viewModel.Travel = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Furniture: 
					viewModel.Furniture = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.Mutual: 
					viewModel.Mutual = new ExpenseModel(item.Id, item.Type, item.Value); 
					break;
				case ExpenseType.MonthlyEnergecticsExpenses:
					viewModel.MonthlyEnergeticsExpense = new ExpenseModel(item.Id, item.Type, item.Value);
					break;
				default: throw new NotSupportedException($"item.Type {item.Type} not supported");
			}

		return viewModel;
	}

	public List<UpdatedHouseholdExpenses> ToUpdatedExpenseInput()
	{
		var updatedExpenses = new List<UpdatedHouseholdExpenses>
		{
			new(Housing.Id, ExpenseType.Housing, Housing.Value),
			new(Electricity.Id, ExpenseType.Electricity, Electricity.Value),
			new(Gas.Id, ExpenseType.Gas, Gas.Value),
			new(Water.Id, ExpenseType.Water, Water.Value),
			new(Insurance.Id, ExpenseType.Insurance, Insurance.Value),
			new(Taxes.Id, ExpenseType.Taxes, Taxes.Value),
			new(Phone.Id, ExpenseType.Phone, Phone.Value),
			new(Internet.Id, ExpenseType.Internet, Internet.Value),
			new(Children.Id, ExpenseType.Children, Children.Value),
			new(CreditRepayment.Id, ExpenseType.CreditRepayment, CreditRepayment.Value),
			new(Food.Id, ExpenseType.Food, Food.Value),
			new(Transport.Id, ExpenseType.Transport, Transport.Value),
			new(Health.Id, ExpenseType.Health, Health.Value),
			new(Leisure.Id, ExpenseType.Leisure, Leisure.Value),
			new(Clothing.Id, ExpenseType.Clothing, Clothing.Value),
			new(Travel.Id, ExpenseType.Travel, Travel.Value),
			new(Furniture.Id, ExpenseType.Furniture, Furniture.Value),
			new(Mutual.Id, ExpenseType.Mutual, Mutual.Value),
			new(MonthlyEnergeticsExpense.Id, ExpenseType.MonthlyEnergecticsExpenses, MonthlyEnergeticsExpense.Value)
		};

		return updatedExpenses;
	}
}

public class ExpenseModel
{
	public Guid? Id { get; init; }
	public ExpenseType Type { get; set; }
	[RequiredExpense]
	public double? Value { get; set; }

	public ExpenseModel()
	{}

	public ExpenseModel(ExpenseType type)
	{
		Type = type;
	}

	public ExpenseModel(Guid? id, ExpenseType expenseType, double? value)
	{
		Id = id;
		Type = expenseType;
		Value = value;
	}
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredExpenseAttribute : ValidationAttribute
{
	protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
	{
		ArgumentNullException.ThrowIfNull(validationContext);

		if (validationContext.ObjectInstance is not ExpenseModel expense || value is not null)
			return ValidationResult.Success;

		var messages = new Dictionary<ExpenseType, string>
		{
			{ ExpenseType.MonthlyEnergecticsExpenses, Labels.Errors.RequiredMonthlyEnergeticsExpenses }
		};

		if (messages.TryGetValue(expense.Type, out string? errorMessage))
		{
			return new ValidationResult(errorMessage, [validationContext.MemberName!]);
		}

		return ValidationResult.Success;
	}
}