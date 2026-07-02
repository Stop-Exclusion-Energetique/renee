using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AccompanyingFileStatus
{
	[Description(Labels.InProgressStatus)] InProgress,
	[Description(Labels.RejectedStatus)] Rejected,
	[Description(IndexLabels.FinishedAccompanyingFiles)] Finished,
	[Description(IndexLabels.WaitingForApproval)] WaitingForApproval,
	[Description(Labels.WaitingForAbortion)] WaitingForAbortion,
	[Description(Labels.Aborted)] Aborted
}