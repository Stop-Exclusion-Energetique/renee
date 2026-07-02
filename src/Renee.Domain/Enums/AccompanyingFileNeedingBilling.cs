using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AccompanyingFileNeedingBilling
{
    [Description(Labels.ToBillStageOne)]
    ToBillStageOne,
    [Description(Labels.ToBillStageTwo)]
    ToBillStageTwo,
    [Description(Labels.ToBillStageThree)]
    ToBillStageThree,
}
