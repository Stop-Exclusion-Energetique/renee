using System.ComponentModel;

namespace Renee.Domain.Enums;

public enum AccompanyingFileStage
{
	[Description(Labels.IdentifyAccompanyingFileStage)]
	Identify,
	[Description(Labels.OrganizeAndFinanceAccompanyingFileStage)]
	OrganizingAndFinancing,
	[Description(Labels.RealizeAndFollowAccompanyingFileStage)]
	RealisationAndFollowing,
	[Description(Labels.FinishedAccompanyingFileStage)]
	Finished
}