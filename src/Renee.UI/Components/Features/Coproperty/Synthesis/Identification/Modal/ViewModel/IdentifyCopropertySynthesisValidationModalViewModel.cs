using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.Identification.Modal.ViewModel;

public class IdentifyCopropertySynthesisValidationModalViewModel
{
    public Guid CopropertyProfileId { get; init; }
    public bool? IsUserAwareOfNoPossibilitiesToUpdateCopropertyProfile { get; set; }

    public string UserRole { get; set; } = string.Empty;

    public bool IsInTzeeProgram { get; init; }
}
