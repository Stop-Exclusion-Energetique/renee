using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.UI.Components.Features.AccompanyingFiles.List.ViewModels;

namespace Renee.UI.Components.Features.AccompanyingFiles.List.Presenter;

public class CreatedAccompanyingFilesByUserPresenter
{
    private List<CreatedAccompanyingFilesByUserViewModel> _allAccompanyingFile = [];
    private int _totalCount;

    public CreatedAccompanyingFilesByUserPresenter FromQuery(
        GetAccompanyingFileForFileListQueryObjectResult queryObjectResult)
    {
        _allAccompanyingFile = queryObjectResult.AccompanyingFiles.Select(
            af => new CreatedAccompanyingFilesByUserViewModel
            {
                Id = af.Id,
                Reference = af.Reference,
                FirstName = af.FirstName,
                LastName = af.LastName,
                Address = af.Address,
                Stage = af.Stage,
                Status = af.Status,
                OpeningDateUtc = af.OpeningDateUtc,
                LastModificationDateUtc = af.LastModificationDateUtc,
                ClosedDateUtc = af.ClosedDateUtc,
                SolidarBuilder = af.SolidarBuilder,
                SecondSolidarBuilder = af.SecondSolidarBuilder,
                ThirdSolidarBuilder = af.ThirdSolidarBuilder,
                ReportingStructureId = af.ReportingStructureId,
                DiffuseCoordinator = af.DiffuseCoordinatorId,
                TerritoryId = af.TerritoryId,
                TargetedCoordinator = af.TargetCoordinatorId,
                TerritorialBuilder = af.TerritorialBuilder,
                SecondTerritorialBuilder = af.SecondTerritorialBuilder,
                HasSolidarBuilderDeletedHisAccount = af.HasSolidarBuilderDeletedHisAccount,
                HasSecondSolidarBuilderDeletedHisAccount = af.HasSecondSolidarBuilderDeletedHisAccount,
                HasThirdSolidarBuilderDeletedHisAccount = af.HasThirdSolidarBuilderDeletedHisAccount,
                NationalStructureId = af.NationalStructureId,
                ShouldAccompanyingFileBeSubmittedToAnah = af.ShouldAccompanyingFileBeSubmittedToAnah,
                AbortReasonLabelId = af.AbortReasonLabelId,
                StageFacturationLabel = af.StageFacturationLabel,
                InvoiceNumber = af.InvoiceNumber,
                InvoiceAmount = af.InvoiceAmount,
                InvoiceDateUtc = af.InvoiceDateUtc,
                SolidarBuilderAbortRequestDetails = af.SolidarBuilderAbortRequestDetails,
                IsBillingRequested = af.IsBillingRequested,
                AbortLabel = af.AbortLabel,
                IsInTZEEProgram = af.IsInTZEEProgram
			}).ToList();
        _totalCount = queryObjectResult.TotalAccompanyingFilesCount;

        return this;
    }

    public CreatedAccompanyingFilesByUserListViewModel Present() => 
        new() { AccompanyingFiles = _allAccompanyingFile, TotalCount = _totalCount };
}