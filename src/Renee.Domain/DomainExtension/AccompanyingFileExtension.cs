using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Renee.Domain.DomainExtension.OrganizeAndFinanceUseCase;
using Renee.Domain.Entity;
using Renee.Domain.Enums;

namespace Renee.Domain.DomainExtension;

public static class AccompanyingFileExtension
{
	public static AccompanyingFile InitializeAccompanyingFile(Guid userId)
	{
		return new AccompanyingFile
		{
			AccompanyingFileMilestone = (int)AccompanyingFileStage.Identify,
			AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress,
			AccompanyingFilePreWorkPlanNavigation = new PreWorkPlan(),
			AccompanyingFilePreFinancingPlanNavigation = new PreFinancingPlan(),
			CreatedBy = userId,
			OpeningDate = DateTime.UtcNow
		};
	}

	public static AccompanyingFile QuickAddHousehold(
		this AccompanyingFile accompanyingFile,
		MainOccupant quickAddOccupant)
	{
		accompanyingFile.AccompanyingFileHouseholdNavigation = new Household
		{
			MainOccupantNavigation = quickAddOccupant
		};

		return accompanyingFile;
	}

	public static AccompanyingFile QuickAddHousing(
		this AccompanyingFile accompanyingFile,
		Address address,
		int typology,
		int? propertyType)
	{
		accompanyingFile.AccompanyingFileHousingNavigation = new Housing
		{
			GeographicAreaTypology = typology,
			HousingAddressNavigation = address,
			HousingInitialStateNavigation = new HousingInitialState(),
			HousingAfterWorkStateNavigation = new HousingAfterWorkState(),
			HousingType = propertyType
		};

		return accompanyingFile;
	}

	public static AccompanyingFile QuickAddSupportTeam(this AccompanyingFile accompanyingFile, SupportTeam supportTeam)
	{
		accompanyingFile.AccompanyingFileSupportTeamNavigation = supportTeam;
		return accompanyingFile;
	}

	public static EntityChanges<Invoice> UpdateAccompanyingFileForRealizeAndFollowMilestone(
		this AccompanyingFile accompanyingFile,
		DateTime? endOfAccompanyingDate,
		DateTime? endOfEncounterDate,
		AccompanyingTimeDuration? accompanyingTimeDuration,
		DateTime? anahGrantDate,
		Guid userId,
		List<Invoice> updatedInvoices)
	{
		accompanyingFile.EndOfAccompanyingDate = endOfAccompanyingDate;
		accompanyingFile.EndOfEncounterDate = endOfEncounterDate;
		accompanyingFile.AccompanyingTimeDurationForRealizeAndFollowMilestone = (int?)accompanyingTimeDuration;
		accompanyingFile.LastUpdateDate = DateTime.UtcNow;
		accompanyingFile.UpdatedBy = userId;
		accompanyingFile.AnahGrantDate = anahGrantDate;

		var invoiceToAdd = updatedInvoices.Where(inv => accompanyingFile.Invoices.All(i => i.Id != inv.Id)).ToList();

		foreach (var invoice in invoiceToAdd) invoice.AccompanyingFileId = accompanyingFile.Id;

		var invoiceToRemove = accompanyingFile.Invoices.Where(i => !updatedInvoices.Exists(inv => inv.Id == i.Id))
			.ToList();

		foreach (var invoice in invoiceToRemove) invoice.AccompanyingFileId = accompanyingFile.Id;

		var invoiceToUpdate = updatedInvoices.Where(inv => accompanyingFile.Invoices.Any(i => i.Id == inv.Id)).ToList();

		foreach (var invoice in invoiceToUpdate) invoice.AccompanyingFileId = accompanyingFile.Id;

		return new EntityChanges<Invoice>(invoiceToAdd, invoiceToUpdate, invoiceToRemove);
	}

	public static AccompanyingFile UpdateHouseholdForIdentificationMilestone(
		this AccompanyingFile accompanyingFile,
		Household updatedHousehold,
		MainOccupant updatedMainOccupant)
	{
		accompanyingFile.AccompanyingFileHouseholdNavigation.UpdateMainOccupant(updatedMainOccupant)
			.UpdateHousehold(updatedHousehold);

		return accompanyingFile;
	}

	public static void UpdateHousingForIdentificationMilestone(
		this AccompanyingFile accompanyingFile,
		Housing updatedHousing,
		Address updatedAddress,
		HousingInitialState initialState)
	{
		accompanyingFile.AccompanyingFileHousingNavigation.UpdateHousing(updatedHousing).UpdateAddress(updatedAddress)
			.UpdateInitialState(initialState);
	}

	public static void UpdateHousingAfterWorkStateForRealizeAndFollowMilestone(
		this AccompanyingFile accompanyingFile,
		DpeLabel? finalDpe,
		int? finalDpeClassJump)
	{
		accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpe = (int?)finalDpe;
		accompanyingFile.AccompanyingFileHousingNavigation.HousingAfterWorkStateNavigation.FinalDpeClassJump = finalDpeClassJump;
	}

	public static void UpdateSynthesis(
		this AccompanyingFile accompanyingFile,
		AccompanyingFileStage milestone,
		AccompanyingFileStatus status,
		Guid userId,
		bool UserCanValidateSynthesis = false)

	{
		accompanyingFile.AccompanyingFileStatus = (int)status;
		switch ((AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone)
		{
			case AccompanyingFileStage.Identify:
				accompanyingFile.IdentifySynthesisValidationDate = DateTime.Now;
				break;
			case AccompanyingFileStage.OrganizingAndFinancing:
				accompanyingFile.OrganizeAndFinanceSynthesisValidationDate = DateTime.Now;
				break;
			case AccompanyingFileStage.RealisationAndFollowing:
				accompanyingFile.RealizeAndFollowSynthesisValidationDate = DateTime.Now;
				if (UserCanValidateSynthesis)
				{
					accompanyingFile.CloseDate = DateTime.Now;
					accompanyingFile.ClosedBy = userId;
				}
				break;
			default:
				break;
		}
		accompanyingFile.AccompanyingFileMilestone = (int)milestone;
	}

	public static void ChangeAccompanyingFileStageAndStatus(
		this AccompanyingFile accompanyingFile,
		bool isValidate,
		Guid userId,
		string? commentOnSynthesis)
	{
		if (isValidate)
		{
			ValidateStage(accompanyingFile, (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone, userId);
		}
		else
		{
			RejectStage(accompanyingFile, (AccompanyingFileStage)accompanyingFile.AccompanyingFileMilestone, userId, commentOnSynthesis);
		}
	}

	private static void ValidateStage(AccompanyingFile file, AccompanyingFileStage stage, Guid userId)
	{
		switch (stage)
		{
			case AccompanyingFileStage.Identify:
				file.AccompanyingFileMilestone = (int)AccompanyingFileStage.OrganizingAndFinancing;
				file.AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress;
				file.IdentifySynthesisValidationDate = DateTime.UtcNow;
				file.IdentifyMilestoneValidatedBy = userId;
				break;

			case AccompanyingFileStage.OrganizingAndFinancing:
				file.AccompanyingFileMilestone = (int)AccompanyingFileStage.RealisationAndFollowing;
				file.AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress;
				file.OrganizeAndFinanceSynthesisValidationDate = DateTime.UtcNow;
				file.OrganizeAndFinanceMilestoneValidatedBy = userId;
				break;

			case AccompanyingFileStage.RealisationAndFollowing:
				file.AccompanyingFileMilestone = (int)AccompanyingFileStage.Finished;
				file.AccompanyingFileStatus = (int)AccompanyingFileStatus.Finished;
				file.RealizeAndFollowSynthesisValidationDate = DateTime.UtcNow;
				file.CloseDate = DateTime.UtcNow;
				file.ClosedBy = userId;
				file.RealizeAndFollowMilestoneValidatedBy = userId;
				break;
		}
	}

	private static void RejectStage(
		AccompanyingFile file,
		AccompanyingFileStage stage,
		Guid userId,
		string? commentOnSynthesis)
	{
		file.AccompanyingFileStatus = (int)AccompanyingFileStatus.Rejected;
		file.CloseDate = DateTime.UtcNow;
		file.ClosedBy = userId;

		switch (stage)
		{
			case AccompanyingFileStage.Identify:
				file.IdentifyMilestoneValidatedBy = userId;
				break;

			case AccompanyingFileStage.OrganizingAndFinancing:
				file.OrganizeAndFinanceMilestoneValidatedBy = userId;
				break;

			case AccompanyingFileStage.RealisationAndFollowing:
				file.RealizeAndFollowMilestoneValidatedBy = userId;
				break;
		}

		if (!string.IsNullOrWhiteSpace(commentOnSynthesis))
		{
			file.RejectionCommentOnSynthesisValidation = commentOnSynthesis;
		}
	}

	public static void UpdateAccompanyingFileForAbortRequest(
		this AccompanyingFile accompanyingFile,
		Guid? abortReasonLabelId,
		Guid? userId,
		string solidarBuilderAbortRequestDetails,
		bool isAbortBillingRequested,
		bool hasAttachment)
	{
		accompanyingFile.AbortRequestedById = userId;
		accompanyingFile.AbortRequestedAt = DateTime.UtcNow;
		accompanyingFile.IsAbortBillingRequested = isAbortBillingRequested;
		accompanyingFile.HasAbortAttachment = hasAttachment;

		if (abortReasonLabelId is not null)
			accompanyingFile.AbortReasonLabelId = abortReasonLabelId;

		if (!string.IsNullOrEmpty(solidarBuilderAbortRequestDetails))
			accompanyingFile.SolidarBuilderAbortRequestDetails = solidarBuilderAbortRequestDetails;

		accompanyingFile.AccompanyingFileStatus = (int)AccompanyingFileStatus.WaitingForAbortion;
	}

	public static void UpdateAccompanyingFileForAbortValidation(
		this AccompanyingFile accompanyingFile,
		Guid? userId,
		AccompanyingFileStatus accompanyingFileStatus = AccompanyingFileStatus.WaitingForAbortion,
		string? validatorComment = null)
	{
		if (accompanyingFileStatus == AccompanyingFileStatus.Aborted)
		{
			accompanyingFile.CloseDate = DateTime.UtcNow;
			accompanyingFile.ClosedBy = userId;
		}

		accompanyingFile.AbortDecidedById = userId;
		accompanyingFile.AbortDecidedAt = DateTime.UtcNow;
		accompanyingFile.AccompanyingFileStatus = (int)accompanyingFileStatus;

		if (!string.IsNullOrEmpty(validatorComment))
			accompanyingFile.ValidatorAbortComment = validatorComment;
	}
}