using Renee.Domain.Enums;

namespace Renee.Domain.Documents;

public static class RequiredMilestonesDocuments
{
	public static readonly IReadOnlyList<Document> Documents =
	[
		new()
		{
			Name = Labels.SignedHouseholdSupportContractLabel,
			AccompanyingFileStage = AccompanyingFileStage.Identify
		},
		new()
		{
			Name = Labels.AnahNotification,
			AccompanyingFileStage = AccompanyingFileStage.OrganizingAndFinancing
		},
		new()
		{
			Name = Labels.EnergyAudit,
			AccompanyingFileStage = AccompanyingFileStage.OrganizingAndFinancing
		},
		new()
		{
			Name = RealiseAndFollowMilestone.Labels.WorksReceiptPv,
			AccompanyingFileStage = AccompanyingFileStage.RealisationAndFollowing
		},
		new()
		{
			Name = RealiseAndFollowMilestone.Labels.AccompanyingFileReport,
			AccompanyingFileStage = AccompanyingFileStage.RealisationAndFollowing
		},
		new()
		{
			Name = RealiseAndFollowMilestone.Labels.AnahHelpObtention,
			AccompanyingFileStage = AccompanyingFileStage.RealisationAndFollowing
		}
	];
}