using Renee.Domain.Entity;

namespace Renee.Domain.DomainExtension.ToRepository;

public record SaveAndSubmitRealizeAndFollowMilestoneData(
	EntityChanges<Invoice> InvoiceChanges,
	EntityChanges<FundingMode> FundingModeChanges,
	EntityChanges<WorkParticipant> WorkParticipantChanges);