using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum TaskPriority
{
	[Description(TaskPriorityLabel.Low)] Low,
	[Description(TaskPriorityLabel.Middle)]
	Middle,
	[Description(TaskPriorityLabel.High)] High
}