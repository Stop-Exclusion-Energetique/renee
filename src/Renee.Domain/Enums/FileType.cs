using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum FileType
{
	[Description(GenerateFilesLabels.GenerateAnahSynthesisButtonLabel)]
	AnahSynthesis,
	[Description(GenerateFilesLabels.WorkCertificate)]
	WorkCertificate,
}