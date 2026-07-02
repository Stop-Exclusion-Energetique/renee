using System.Collections;
using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.Identification.Details.HouseholdIdentity.ViewModels;

public class HouseholdViewModel(ExpensesViewModel expenses)
{
	public double? ResourcesTotalValue => ResourceTypologieValues.Sum(res => res.Value);

	public double? EnergyEffortRate
	{
		get
		{
			if (Expenses.EnergeticTotal is not null && IncomeTaxReference > 0)
				return Math.Round((double)(Expenses.EnergeticTotal / (IncomeTaxReference) * 100));
			return null;
		}
	}

	public double? DebtRate
	{
		get
		{
			if (Expenses.CasualExpenseTotal is not null && ResourcesTotalValue > 0)
				return Math.Round((double)(Expenses.CasualExpenseTotal * 100 / ResourcesTotalValue));

			return null;
		}
	}

	public double? AvailableBudget => ResourcesTotalValue - Expenses.Total;

	[ValidateComplexType]
	public List<ResourceTypologyValueViewModel> ResourceTypologieValues { get; init; } = [];

	public List<HouseholdHeatingEnergyValueViewModel> HeatingEnergiesValues { get; init; } = [];

	public List<Guid?> HeatingEnergiesValuesSelected { get; set; } = [];

	[ValidateComplexType]
	public ExpensesViewModel Expenses { get; init; } = expenses;

	[Required(ErrorMessage = Labels.Errors.RequiredUnpaidEnergyBills)]
	public bool? HasUnpaidEnergyBills { get; set; }

	[Required(ErrorMessage = Labels.Errors.HouseholdTypologyError)]
	public HouseholdTypology? AskedHouseholdTypology { get; set; }

	public bool? FollowedBySocialWorker { get; set; }

	public bool? Disability { get; set; }

	public bool? LongTermIllness { get; set; }

	public bool? LackOfAutonomy { get; set; }

	public bool? Curatorship { get; set; }

	public bool? Guardianship { get; set; }

	[Required(ErrorMessage = Labels.Errors.RequiredTaxIncomeInput)]
	public int? IncomeTaxReference { get; set; } = null;

	public string? AnahCategory { get; set; }

	[RequiredAskedResourcesTypology]
	public List<Guid?> AskedResourcesTypology { get; set; } = [];
}

[AttributeUsage(AttributeTargets.Property)]
public class RequiredAskedResourcesTypologyAttribute : ValidationAttribute
{
	public RequiredAskedResourcesTypologyAttribute()
		: base(() => Labels.Errors.RequiredAskedResourcesTypologyInput) { }

	public override bool IsValid(object? value) => value is IList list && list.Count > 0;
}