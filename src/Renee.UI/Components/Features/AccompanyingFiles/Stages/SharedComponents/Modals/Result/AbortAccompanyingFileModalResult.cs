namespace Renee.UI.Components.Features.AccompanyingFiles.Stages.SharedComponents.Modals.Result;

public record AbortAccompanyingFileModalResult(
	Guid? AbortReasonLabelId,
	string? SolidarBuilderComment,
	bool? IsBillingRequested,
	bool HasAttachment);