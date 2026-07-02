using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum UnsavedChangesOption
{
	[Description(UnsavedChangesOptionLabels.SaveAndLeave)]
	SaveAndLeave,
	[Description(UnsavedChangesOptionLabels.LeaveWithoutSaving)]
	LeaveWithoutSaving,
	[Description(Labels.Cancel)]
	Cancel
}