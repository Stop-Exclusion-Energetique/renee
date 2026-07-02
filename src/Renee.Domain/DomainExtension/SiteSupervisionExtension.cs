using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension;

public static class SiteSupervisionExtension
{
	public static void UpdateSiteSupervision(
		this SiteSupervision siteSupervision,
		SiteSupervision updatedSiteSupervision)
	{
		siteSupervision.OverallStartDate = updatedSiteSupervision.OverallStartDate;
		siteSupervision.EstimatedOverallCompletionDate = updatedSiteSupervision.EstimatedOverallCompletionDate;
		siteSupervision.ActualOverallEndDate = updatedSiteSupervision.ActualOverallEndDate;
		siteSupervision.OverallProgress = updatedSiteSupervision.OverallProgress;
		siteSupervision.NextCoordinationMeetingScheduledFor = updatedSiteSupervision.NextCoordinationMeetingScheduledFor;
		siteSupervision.OverallObservations = updatedSiteSupervision.OverallObservations;
		siteSupervision.PreSiteSupervisionMeetingDate = updatedSiteSupervision.PreSiteSupervisionMeetingDate;
	}

	public static EntityChanges<WorkParticipant>
	UpdateSiteSupervisionWorkParticipants(
        this SiteSupervision siteSupervision,
        List<WorkParticipant> updatedWorkParticipants)
    {
		var existing = siteSupervision.WorkParticipants;
		var updated = updatedWorkParticipants;

		var workParticipantsToAdd = updated
			.ExceptBy(existing.Select(e => e.Id), u => u.Id)
			.ToList();

		foreach (var item in workParticipantsToAdd) item.SiteSupervisionId = siteSupervision.Id;

		var workParticipantsToUpdate = updated
			.IntersectBy(existing.Select(e => e.Id), u => u.Id)
			.ToList();

		foreach (var item in workParticipantsToUpdate) item.SiteSupervisionId = siteSupervision.Id;


		var workParticipantsToRemove = existing
			.ExceptBy(updated.Select(u => u.Id), e => e.Id)
			.ToList();

		return new EntityChanges<WorkParticipant>(
			workParticipantsToAdd,
			workParticipantsToUpdate,
			workParticipantsToRemove);
    }

	public static EntityChanges<WorkParticipantDifficulty>
		UpdateWorkParticipantDifficulties(
			this WorkParticipant updatedWorkParticipant,
			WorkParticipant existingWorkParticipant)
	{
		var existing = existingWorkParticipant.WorkParticipantDifficulties;
		var updated = updatedWorkParticipant.WorkParticipantDifficulties;

		var difficultiesToAdd = updated
			.ExceptBy(existing.Select(e => e.DifficultyId), u => u.DifficultyId)
			.ToList();

		foreach (var item in difficultiesToAdd) item.WorkParticipantId = updatedWorkParticipant.Id;
		
		var difficultiesToUpdate = updated
			.IntersectBy(existing.Select(e => e.DifficultyId), u => u.DifficultyId)
			.ToList();

		foreach (var item in difficultiesToUpdate) item.WorkParticipantId = updatedWorkParticipant.Id;

		var difficultiesToRemove = existing
			.ExceptBy(updated.Select(u => u.DifficultyId), e => e.DifficultyId)
			.ToList();

		return new EntityChanges<WorkParticipantDifficulty>(
			difficultiesToAdd,
			difficultiesToUpdate,
			difficultiesToRemove);
	}
}