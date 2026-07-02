using Renee.Application.Queries.Coproperty.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.Coproperty.List.ViewModel;

public class CreatedCopropertyProfileByUserViewModel
{
    public string EditUrl =>
        Stage switch
        {
            AccompanyingFileStage.Identify => $"{Endpoints.CopropertyIdentification}/{Id}",
            AccompanyingFileStage.OrganizingAndFinancing => $"{Endpoints.CopropertyOrganizeAndFinance}/{Id}",
            AccompanyingFileStage.RealisationAndFollowing => $"{Endpoints.CopropertyRealizeAndFollow}/{Id}",
            AccompanyingFileStage.Finished => $"{Endpoints.CopropertyRealizeAndFollow}/{Id}",
            _ => string.Empty
        };

    public string SynthesisUrl =>
        Stage switch
        {
            AccompanyingFileStage.Identify => $"{Endpoints.CopropertyIdentificationSynthesis}/{Id}",
            AccompanyingFileStage.OrganizingAndFinancing => $"{Endpoints.CopropertyOrganizeAndFinanceSynthesis}/{Id}",
            AccompanyingFileStage.RealisationAndFollowing => $"{Endpoints.CopropertyRealizeAndFollowSynthesis}/{Id}",
            AccompanyingFileStage.Finished => $"{Endpoints.CopropertyRealizeAndFollowSynthesis}/{Id}",
            _ => string.Empty
        };
    public string ShowSynthesisUrl => $"{Endpoints.CopropertyIdentificationSynthesis}/{Id}";

    public string ShowCopropertyProfileTeamUrl => $"{Endpoints.CopropertyProfileMenu}/{Id}";

    public Guid Id { get; init; }
    public string? Reference { get; init; }
    public string? Address { get; init; }
    public AccompanyingFileStage Stage { get; init; }
    public AccompanyingFileStatus Status { get; init; }
    public DateTime? CreatedDateUtc { get; init; }
    public DateTime? LastUpdatedDateUtc { get; init; }
    public Guid SolidarBuilder { get; set; }
    public Guid? SecondSolidarBuilder { get; set; }
    public Guid? ThirdSolidarBuilder { get; set; }
    public Guid? ReportingStructureId { get; set; }
    public Guid? DiffuseCoordinator { get; set; }
    public Guid? TargetedCoordinator { get; set; }
    public Guid? TerritorialBuilder { get; set; }
    public Guid? SecondTerritorialBuilder { get; set; }
    public Guid? TerritoryId { get; set; }

    public static CreatedCopropertyProfileByUserViewModel CreateViewModelFromResume(CopropertyProfileResume cpr)
    => new()
        {
            Id = cpr.Id,
            Reference = cpr.Reference,
            Address = cpr.Address,
            Stage = cpr.Stage,
            Status = cpr.Status,
            CreatedDateUtc = cpr.CreateDateUtc,
            LastUpdatedDateUtc = cpr.LastUpdatedDateUtc,
            SolidarBuilder = cpr.SolidarBuilder,
            SecondSolidarBuilder = cpr.SecondSolidarBuilder,
            ThirdSolidarBuilder = cpr.ThirdSolidarBuilder,
            ReportingStructureId = cpr.ReportingStructureId,
            DiffuseCoordinator = cpr.DiffuseCoordinatorId,
            TargetedCoordinator = cpr.TargetCoordinatorId,
            TerritorialBuilder = cpr.TerritorialBuilder,
            SecondTerritorialBuilder = cpr.SecondTerritorialBuilder,
            TerritoryId = cpr.TerritoryId
    };
}
