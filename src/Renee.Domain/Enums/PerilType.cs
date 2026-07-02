using System.ComponentModel;

namespace Renee.Domain.Enums;


public enum PerilType
{
    [Description(PerilTypeLabel.TechnicalReliability)]
    TechnicalReliability,
    [Description(PerilTypeLabel.BudgetControl)]
    BudgetControl,
    [Description(PerilTypeLabel.DeadLineCompliance)]
    DeadLineCompliance,
    [Description(PerilTypeLabel.CollectiveAdhesion)]
    CollectiveAdhesion,
    [Description(PerilTypeLabel.RegulatoryCompliance)]
    RegulatoryCompliance,
    [Description(PerilTypeLabel.EnergyPerformance)]
    EnergyPerformance
}

