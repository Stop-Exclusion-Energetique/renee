using System.Reflection.Emit;
using Renee.Application.Abstraction.Query;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.AccompanyingFile;

public class GetAccompanyingFileForFileListQueryHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IUserRepository userRepository,
	ITelemetryService telemetryService)
	: QueryHandler<GetAccompanyingFileForFileListQuery, ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>>
{
	public override async Task<ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>> HandleQuery(
		GetAccompanyingFileForFileListQuery request)
	{
		try
		{
			if (request.UserId == Guid.Empty) return ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>.Failure(Labels.Errors.UserNotFound);

			var user = await userRepository.GetUserById(request.UserId);

			if (user?.ReportingStructureId == null) return ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>.Failure(Labels.Errors.UserNotFound);

			var filterOptions = request.FilterOptions;

			var (accompanyingFiles, totalCount) = request.Role switch
			{
				Constants.SolidarBuilderRole => await accompanyingFileRepository
					.GetAllAccompanyingFilesForSolidarBuilders(
						request.UserId,
						(Guid)user.ReportingStructureId,
						new AccompanyingFilesListFilterOptions(
							filterOptions.AccompanyingFileStages,
							filterOptions.AccompanyingFileStatuses,
							filterOptions.AccompanyingFileNeedingBillings,
							filterOptions.FilterValue,
							filterOptions.SortingState,
							filterOptions.FilterContext),
						request.NumberOfItempsToSkip,
						request.PageSize),
				Constants.AssociationMemberRole or Constants.AdminRole => await accompanyingFileRepository
					.GetAllAccompanyingFileForAssociationMembersAndAdmins(
						new AccompanyingFilesListFilterOptions(
							filterOptions.AccompanyingFileStages,
							filterOptions.AccompanyingFileStatuses,
							filterOptions.AccompanyingFileNeedingBillings,
							filterOptions.FilterValue,
							filterOptions.SortingState,
							filterOptions.FilterContext,
							filterOptions.ReportingStructures),
						request.NumberOfItempsToSkip,
						request.PageSize),
				Constants.DiffuseCoordinatorRole or Constants.TargetedCoordinatorRole =>
					await accompanyingFileRepository.GetAllAccompanyingFilesForCoordinators(
						request.UserId,
						new AccompanyingFilesListFilterOptions(
							filterOptions.AccompanyingFileStages,
							filterOptions.AccompanyingFileStatuses,
							filterOptions.AccompanyingFileNeedingBillings,
							filterOptions.FilterValue,
							filterOptions.SortingState,
							filterOptions.FilterContext,
							filterOptions.ReportingStructures,
							filterOptions.SolidarBuilders,
							filterOptions.Territories),
						request.NumberOfItempsToSkip,
						request.PageSize),
				Constants.TerritorialBuilderRole => await accompanyingFileRepository
					.GetAllAccompanyingFilesForTerritorialBuilders(
						request.UserId,
						new AccompanyingFilesListFilterOptions(
							filterOptions.AccompanyingFileStages,
							filterOptions.AccompanyingFileStatuses,
							filterOptions.AccompanyingFileNeedingBillings,
							filterOptions.FilterValue,
							filterOptions.SortingState,
							filterOptions.FilterContext,
							filterOptions.ReportingStructures,
							filterOptions.SolidarBuilders),
						request.NumberOfItempsToSkip,
						request.PageSize),
				Constants.StructuralReferentRole => await accompanyingFileRepository
					.GetAllAccompanyingFilesForStructuralReferents(
						(Guid)user.ReportingStructureId,
						new AccompanyingFilesListFilterOptions(
							filterOptions.AccompanyingFileStages,
							filterOptions.AccompanyingFileStatuses,
							filterOptions.AccompanyingFileNeedingBillings,
							filterOptions.FilterValue,
							filterOptions.SortingState,
							filterOptions.FilterContext),
						request.NumberOfItempsToSkip,
						request.PageSize),
				_ => ([], 0)
			};

			var accompanyingFilesResumes = accompanyingFiles.Select(af => new UserAccompanyingFileResume
			{
				Id = af.Id,
				Reference = af.AccompanyingFileReference,
				FirstName = af.FirstName,
				LastName = af.LastName,
				Address = af.Label,
				Stage = (AccompanyingFileStage)af.AccompanyingFileMilestone,
				Status = (AccompanyingFileStatus)af.AccompanyingFileStatus,
				OpeningDateUtc = af.OpeningDate ?? DateTime.UtcNow,
				LastModificationDateUtc = af.LastUpdateDate ?? DateTime.UtcNow,
				ClosedDateUtc = af.CloseDate,
				SolidarBuilder = af.SolidarBuilder,
				SecondSolidarBuilder = af.SecondSolidarBuilder,
				ReportingStructureId = af.ReportingStructureId,
				DiffuseCoordinatorId = af.DiffuseCoordinator,
				TargetCoordinatorId = af.TargetCoordinator,
				TerritoryId = af.AccompanyingFileTerritory,
				TerritorialBuilder = af.TerritorialBuilder,
				SecondTerritorialBuilder = af.SecondTerritorialBuilder,
				ThirdSolidarBuilder = af.ThirdSolidarBuilder,
                IsInTZEEProgram = af.ZeroEnergyExclusionTerritoriesProgram == true,
				NationalStructureId = af.NationalStructureId,
				HasSolidarBuilderDeletedHisAccount = af.SolidarBuilderDeleted == true,
				HasSecondSolidarBuilderDeletedHisAccount = af.SecondSolidarBuilderDeleted == true,
				HasThirdSolidarBuilderDeletedHisAccount = af.ThirdSolidarBuilderDeleted == true,
				ShouldAccompanyingFileBeSubmittedToAnah = af.ShouldAccompanyingFileBeSubmittedToAnah,
				AbortReasonLabelId = af.AbortReasonLabelId,
				StageFacturationLabel = GetStageFacturationLabel(af),
				AbortLabel = GetAbortLabel(af),
				InvoiceNumber = GetLastInvoiceNumber(af),
				InvoiceAmount = GetLastInvoiceAmount(af),
				InvoiceDateUtc = GetLastInvoiceDateUtc(af),
				SolidarBuilderAbortRequestDetails = af.SolidarBuilderAbortRequestDetails,
				IsBillingRequested = af.IsAbortBillingRequested == true
			}).ToList();

			return ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>.Success(new GetAccompanyingFileForFileListQueryObjectResult
			{
				AccompanyingFiles = accompanyingFilesResumes,
				TotalAccompanyingFilesCount = totalCount
			});
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, CancellationToken.None);
			return ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}

	private static string? GetStageFacturationLabel(AccompanyingFileResumeView af)
	{
		if (af.BilledJalon3 == true)
			return Labels.ThirdStageFacturation;
		if (af.BilledJalon2 == true)
			return Labels.SecondStageFacturation;
		if (af.BilledJalon1 == true)
			return Labels.FirstStageFacturation;
		return null;
	}

	private static string? GetAbortLabel(AccompanyingFileResumeView af)
	{
		if (af.AccompanyingFileStatus != (int)AccompanyingFileStatus.Aborted)
			return null;

		if (af.IsAbortBillingRequested == true)
			return string.Format(Labels.AbortWithBillingRequestLabel, (int?)af.AccompanyingFileMilestone + 1);

		if (af.IsAbortBillingRequested == false)
			return Labels.AbortWithoutBillingRequestLabel;

		return null;
	}

	private static double? GetLastInvoiceAmount(AccompanyingFileResumeView af)
	{
		if (af.AmountBilledThirdStage.HasValue)
			return af.AmountBilledThirdStage.Value;
		if (af.AmountBilledSecondStage.HasValue)
			return af.AmountBilledSecondStage.Value;
		if (af.AmountBilledFirstStage.HasValue)
			return af.AmountBilledFirstStage.Value;
		return null;
	}

	private static string? GetLastInvoiceNumber(AccompanyingFileResumeView af)
	{
		if (!string.IsNullOrEmpty(af.InvoiceNumberThirdStage))
			return af.InvoiceNumberThirdStage;
		if (!string.IsNullOrEmpty(af.InvoiceNumberSecondStage))
			return af.InvoiceNumberSecondStage;
		if (!string.IsNullOrEmpty(af.InvoiceNumberFirstStage))
			return af.InvoiceNumberFirstStage;
		return null;
	}
	
	private static DateTime? GetLastInvoiceDateUtc(AccompanyingFileResumeView af)
	{
		if (af.BillingDateThirdStage.HasValue)
			return af.BillingDateThirdStage.Value;
		if (af.BillingDateSecondStage.HasValue)
			return af.BillingDateSecondStage.Value;
		if (af.BillingDateFirstStage.HasValue)
			return af.BillingDateFirstStage.Value;
		return null;
	}
}