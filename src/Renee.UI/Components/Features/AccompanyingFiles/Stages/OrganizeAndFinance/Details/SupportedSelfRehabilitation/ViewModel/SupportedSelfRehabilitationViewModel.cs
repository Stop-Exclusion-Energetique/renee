using System.ComponentModel.DataAnnotations;
using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.OrganizeAndFinance.Details.SupportedSelfRehabilitation.ViewModel;

public class SupportedSelfRehabilitationViewModel
{
	public bool? IsFamilyReadyForSupportedSelfRehabilitationApproach { get; set; }
	public bool? FamilyPhysicalCapabilitiesHaveBeenTakenIntoAccount { get; set; }
	public bool? FamilyCanMobilizeSocialCircleOnConstructionSite { get; set; }
	public string? WorkDetails { get; set; }
	public string? FamilyAvailabilityToOrganizeSupportedSelfRehabilitationApproachSite { get; set; }
	[Required(ErrorMessage = Labels.Errors.RequiredAccompayingTimeDuration)]
	public AccompanyingTimeDuration? AccompanyingTimeDuration { get; set; }
	public DateTime? StartOfAccompanyingDate { get; set; }
	[Required(ErrorMessage = Labels.Errors.RequiredAnahFolderNumber)]
	public string? AnahFolderNumber { get; set; }
	[Required(ErrorMessage = Labels.Errors.RequiredAnahFolderFilingDate)]
	public DateTime? AnahFolderFilingDate { get; set; }
}