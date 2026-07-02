using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

public class CreatedAccompanyingFilesByUserViewModel
{
	public string EditUrl =>
		Stage switch
		{
			AccompanyingFileStage.Identify => $"{Endpoints.NewOccupant}/{Id}",
			AccompanyingFileStage.OrganizingAndFinancing => $"{Endpoints.OrganizeAndFinanceStage}/{Id}",
			AccompanyingFileStage.RealisationAndFollowing => $"{Endpoints.RealiseAndFollowStage}/{Id}",
            AccompanyingFileStage.Finished => $"{Endpoints.RealiseAndFollowStage}/{Id}",
            _ => string.Empty
		};

	public string ShowSynthesisUrl => $"{Endpoints.IdentificationSynthesis}/{Id}";
	public string ShowAccompanyingFileTeamUrl => $"{Endpoints.AccompanyingFileMenu}/{Id}";

	public string SynthesisUrl =>
        Stage switch
        {
            AccompanyingFileStage.Identify => $"{Endpoints.IdentificationSynthesis}/{Id}",
            AccompanyingFileStage.OrganizingAndFinancing => $"{Endpoints.OrganizeAndFinanceSynthesis}/{Id}",
            AccompanyingFileStage.RealisationAndFollowing => $"{Endpoints.RealizeAndFollowSynthesis}/{Id}",
			AccompanyingFileStage.Finished => $"{Endpoints.RealizeAndFollowSynthesis}/{Id}",
            _ => string.Empty
        };

    public Guid Id { get; init; }
	public string? Reference { get; init; }
	public string? FirstName { get; init; }
	public string? LastName { get; init; }
	public string? Address { get; init; }
	public AccompanyingFileStage Stage { get; init; }
	public AccompanyingFileStatus Status { get; init; }
	public DateTime OpeningDateUtc { get; init; }
	public DateTime LastModificationDateUtc { get; init; }
	public DateTime? ClosedDateUtc { get; init; }
	public Guid SolidarBuilder { get; set; }
	public Guid? SecondSolidarBuilder { get; set; }
	public Guid? ThirdSolidarBuilder { get; set; }
	public Guid? ReportingStructureId { get; set; }
	public Guid? DiffuseCoordinator { get; set; }
	public Guid? TargetedCoordinator { get; set; }
	public Guid? TerritorialBuilder { get; set; }
    public Guid? SecondTerritorialBuilder { get; set; }
    public Guid? TerritoryId { get; set; }
	public Guid? NationalStructureId { get; set; }
	public bool HasSolidarBuilderDeletedHisAccount { get; set; }
	public bool HasSecondSolidarBuilderDeletedHisAccount { get; set; }
	public bool HasThirdSolidarBuilderDeletedHisAccount { get; set; }
	public bool? ShouldAccompanyingFileBeSubmittedToAnah { get; set; }
	public Guid? AbortReasonLabelId { get; set; }
	public string? StageFacturationLabel { get; set; }
	public string? InvoiceNumber { get; set; }
	public double? InvoiceAmount { get; set; }
	public DateTime? InvoiceDateUtc { get; set; }
	public string? SolidarBuilderAbortRequestDetails { get; set; }
	public bool IsBillingRequested { get; set; }
	public string? AbortLabel { get; set; }
	public bool IsInTZEEProgram { get; set; }
}