using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum ExpenseType
{
	[Description(Labels.Housing)] Housing,
	[Description(Labels.Electricity)] Electricity,
	[Description(Labels.Gaz)] Gas,
	[Description(Labels.Water)] Water,
	[Description(Labels.Insurance)] Insurance,
	[Description(Labels.Tax)] Taxes,
	[Description(Labels.PhoneExpenditure)] Phone,
	[Description(Labels.Internet)] Internet,
	[Description(Labels.ChildrenExpenditure)]
	Children,
	[Description(Labels.RepaymentOfCredit)]
	CreditRepayment,
	[Description(Labels.Food)] Food,
	[Description(Labels.Transport)] Transport,
	[Description(Labels.Health)] Health,
	[Description(Labels.Leisure)] Leisure,
	[Description(Labels.Clothing)] Clothing,
	[Description(Labels.Travel)] Travel,
	[Description(Labels.Furniture)] Furniture,
	[Description(Labels.Mutual)] Mutual,
	[Description(Labels.MonthlyEnergecticsExpenses)] MonthlyEnergecticsExpenses,
}