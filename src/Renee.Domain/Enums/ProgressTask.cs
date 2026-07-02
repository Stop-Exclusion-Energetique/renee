using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum ProgressTask
{
	[Description("Tâches à réaliser")] ToBeCompletedTask = 0,
	[Description("Tâches en cours")] CurrentTask = 1,
	[Description("Tâches terminées")] DoneTask = 2
}