using Renee.Domain;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Layout.StageLayout;

public class StageNavigationModel
{
	public string Url { get; set; } = string.Empty;
	public AccompanyingFileStage Stage { get; set; }
	public bool IsCurrentPage { get; set; }
}

public static class StageNavigation
{
	public static List<StageNavigationModel> StageNavigationAccompanyingFileList =>
		new()
		{
			new StageNavigationModel { Url = Endpoints.NewOccupant, Stage = AccompanyingFileStage.Identify },
			new StageNavigationModel
			{
				Url = Endpoints.OrganizeAndFinanceStage, Stage = AccompanyingFileStage.OrganizingAndFinancing
			},
			new StageNavigationModel
			{
				Url = Endpoints.RealiseAndFollowStage, Stage = AccompanyingFileStage.RealisationAndFollowing
			}
		};

    public static List<StageNavigationModel> StateNavigationCopropertyProfileList =>
        new()
        {
            new StageNavigationModel { Url = Endpoints.CopropertyIdentification, Stage = AccompanyingFileStage.Identify },
            new StageNavigationModel
            {
                Url = Endpoints.CopropertyOrganizeAndFinance, Stage = AccompanyingFileStage.OrganizingAndFinancing
            },
            new StageNavigationModel
            {
                Url = Endpoints.CopropertyRealizeAndFollow, Stage = AccompanyingFileStage.RealisationAndFollowing
            }
        };
}