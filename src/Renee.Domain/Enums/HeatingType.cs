using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum HeatingType
{
    [Description(HeatingTypeLabel.IndividualHeating)]
    Individual,
    [Description(HeatingTypeLabel.CollectiveHeating)]
    Collective
}
