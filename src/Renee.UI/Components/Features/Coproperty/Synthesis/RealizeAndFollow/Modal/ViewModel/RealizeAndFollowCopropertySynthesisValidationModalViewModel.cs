using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.Synthesis.RealizeAndFollow.Modal.ViewModel;

public class RealizeAndFollowCopropertySynthesisValidationModalViewModel
{
    public Guid CopropertyProfileId { get; init; }
    public bool? IsUserAwareOfNoPossibilitiesToUpdateCopropertyProfile { get; set; }

    public string UserRole { get; set; } = string.Empty;

    public bool IsInTzeeProgram { get; init; }
}
