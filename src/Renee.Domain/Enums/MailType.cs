namespace Renee.Domain.Enums;

public enum MailType
{
	CreationCode,
	InscriptionUserWaiting,
	InscriptionAdminWaiting,
	InscriptionConfirmation,
	InscriptionRefused,
	StageValidated,
	StageRefused,
	StageReady,
	AbortAccompanyingFileRequest,
	Abort,
	AbortCancellation,
	TaskAssigned,
	TaskCompleted,
	TaskDeleted
}