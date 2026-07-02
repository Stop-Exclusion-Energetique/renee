using Renee.Domain.Enums;

namespace Renee.Application.DTOs.SiteSupervision;

public record WorkParticipantDto(
	Guid Id,
	ParticipantType? ParticipantType,
	string? WorkTypesLabel,
	string? ParticipantName,
	string? ContactAdvisor,
	DateTime? StartDateOfWork,
	DateTime? EstimatedCompletionDate,
	DateTime? ActualEndDate,
	double? Progress,
	WorkQuality? WorkQuality,
	string? CommentOnWorkQuality,
	List<Guid?> WorkParticipantDifficulties,
	string? CommentOnWorkParticipantDifficulties,
	string? SpecificComments);