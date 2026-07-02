using System.ComponentModel;
namespace Renee.Domain.Enums;

public enum NatureOfSyndicType
{
    [Description(NatureOfSyndicTypeLabel.ProfessionalType)] Professional,
    [Description(NatureOfSyndicTypeLabel.BenevoleType)] Benevole
}
