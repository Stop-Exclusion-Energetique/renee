using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum ParticipantType
{
	[Description(ParticipantTypeLabels.Company)]
	Company,
	[Description(ParticipantTypeLabels.ProjectManagementAssistant)]
	ProjectManagementAssistant,
	[Description(ParticipantTypeLabels.ProjectManagement)]
	ProjectManagement
}