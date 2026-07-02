using Renee.Application.Abstraction.Query;
using Renee.Application.DTOs.ExcelExport;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetCompleteAccompanyingFileForBillingLogAndAdministrationExcelExportQueryHandler(
    IAccompanyingFileRepository accompanyingFileRepository,
    IUserRepository userRepository,
    ITelemetryService telemetryService) 
    : QueryHandler<GetCompleteAccompanyingFileForBillingLogAndAdministrationExcelExportQuery, ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>>
{
    public async override Task<ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>> HandleQuery(GetCompleteAccompanyingFileForBillingLogAndAdministrationExcelExportQuery request)
    {
        try
        {
            var user = await userRepository.GetUserById(request.UserId);

            if (user is null)
                return ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>.Failure(Labels.Errors.UserNotFound);

            var accompanyingFiles = request.UserRole switch
            {
                string userRole when userRole == Constants.SolidarBuilderRole || userRole == Constants.TerritorialBuilderRole => await accompanyingFileRepository.GetSolidarOrTerritorialBuilderAccompanyingFileForBillingLogAndAdministrationExcelExport(request.UserId),
                string userRole when userRole == Constants.DiffuseCoordinatorRole || userRole == Constants.TargetedCoordinatorRole => await accompanyingFileRepository.GetAllAccompanyingFileForBillingLogAndAdministrationExcelExport(),
                string userRole when userRole == Constants.StructuralReferentRole =>
                    (user.ReportingStructureNavigation?.NationalStructureId != null) ? await accompanyingFileRepository.GetStructuralReferentAccompanyingFileForBillingLogAndAdministrationExcelExport((Guid)user.ReportingStructureNavigation.NationalStructureId) : null,
                _ => null
            };

            if (accompanyingFiles is null)
                return ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFiles);

            var excelData = new List<AccompanyingFileForBillingAndAdministrationExcelExportDto>();
            foreach (var af in accompanyingFiles)
            {
                var dto = MapToDto(af);
                excelData.Add(dto);
            }

            return ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>.Success(excelData);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex);

            return ReneeOperationResult<List<AccompanyingFileForBillingAndAdministrationExcelExportDto>>.Failure(ExcelDataLabel.Errors.ErrorWhileGeneratingExcelFile);
        }
    }

    private static AccompanyingFileForBillingAndAdministrationExcelExportDto MapToDto(Renee.Domain.Entity.AccompanyingFile af)
    {
        return new AccompanyingFileForBillingAndAdministrationExcelExportDto
        {
            AccompanyingFileReference = af.AccompanyingFileReference,
            AccompanyingFileExternalReference = af.ExternalReference,
            ReportingStructure = af.AccompanyingFileSupportTeamNavigation != null && af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation != null
                ? af.AccompanyingFileSupportTeamNavigation.SolidarBuilderNavigation.ReportingStructureNavigation?.Name : string.Empty,
            FirstSolidarBuilderName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.SolidarBuilderNavigation),
            SecondSolidarBuilderName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.SecondSolidarBuilderNavigation),
            ThirdSolidarBuilderName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.ThirdSolidarBuilderNavigation),
            FirstTerritorialBuilderName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.TerritorialBuilderNavigation),
            SecondTerritorialBuilderName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.SecondTerritorialBuilderNavigation),
            DiffuseCoordinatorName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.DiffuseCoordinatorNavigation),
            TargetCoordinatorName = GetUserName(af.AccompanyingFileSupportTeamNavigation?.TargetCoordinatorNavigation),
            AccompanyingType = EnumHelper.GetDescription((AccompanyingType?)af.AccompanyingType),
            GeographicAreaTypology = EnumHelper.GetDescription((GeographicalHousingAreaTypology?)af.AccompanyingFileHousingNavigation.GeographicAreaTypology),
            AccompanyingFileTerritory = af.AccompanyingFileTerritoryNavigation != null ? af.AccompanyingFileTerritoryNavigation.Label : string.Empty,
            AccompanyingFileMilestone = EnumHelper.GetDescription((AccompanyingFileStage)af.AccompanyingFileMilestone),
            AccompanyingFileStatus = EnumHelper.GetDescription((AccompanyingFileStatus)af.AccompanyingFileStatus),
            IsFirstStageBilled = af.AccompanyingFileBillingLog?.BilledJalon1 == true ? "Oui" : "Non",
            FirstStageInvoiceNumber = af.AccompanyingFileBillingLog?.InvoiceNumberFirstStage,
            FirstStageAmountBilled = af.AccompanyingFileBillingLog?.AmountBilledFirstStage,
            FirstStageBillingDate = af.AccompanyingFileBillingLog?.BillingDateFirstStage,
            FirstStageCallForFundsDate = af.AccompanyingFileBillingLog?.FundraisingLauchDateForFirstStage,
            IsSecondStageBilled = af.AccompanyingFileBillingLog?.BilledJalon2 == true ? "Oui" : "Non",
            SecondStageInvoiceNumber = af.AccompanyingFileBillingLog?.InvoiceNumberSecondStage,
            SecondStageAmountBilled = af.AccompanyingFileBillingLog?.AmountBilledSecondStage,
            SecondStageBillingDate = af.AccompanyingFileBillingLog?.BillingDateSecondStage,
            SecondStageCallForFundsDate = af.AccompanyingFileBillingLog?.FundraisingLauchDateForSecondStage,
            IsThirdStageBilled = af.AccompanyingFileBillingLog?.BilledJalon3 == true ? "Oui" : "Non",
            ThirdStageInvoiceNumber = af.AccompanyingFileBillingLog?.InvoiceNumberThirdStage,
            ThirdStageAmountBilled = af.AccompanyingFileBillingLog?.AmountBilledThirdStage,
            ThirdStageBillingDate = af.AccompanyingFileBillingLog?.BillingDateThirdStage,
            ThirdStageCallForFundsDate = af.AccompanyingFileBillingLog?.FundraisingLauchDateForThirdStage,
            AnahFolderNumber = af.AnahFolderNumber,
            AnahFolderFilingDate = af.AnahFolderFilingDate
        };
    }
    
    private static string? GetUserName (Renee.Domain.Entity.User? user)
    {
        if (user is null)
            return null;

        return $"{user.FirstName} {user.LastName}";
    }
}
