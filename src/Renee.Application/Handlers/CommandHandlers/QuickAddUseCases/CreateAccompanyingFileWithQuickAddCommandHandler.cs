using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.QuickAddUseCases;

public class CreateAccompanyingFileWithQuickAddCommandHandler(
	IAccompanyingFileRepository accompanyingFileRepository,
	IAdminConstantRepository adminConstantsRepository,
	ITelemetryService telemetryService)
	: IRequestHandler<CreateAccompanyingFileWithQuickAddCommandInput, ReneeOperationResult<Guid?>>
{
	public async Task<ReneeOperationResult<Guid?>> Handle(
		CreateAccompanyingFileWithQuickAddCommandInput request,
		CancellationToken cancellationToken)
	{
		try
		{
			if (request.AccompanyingFileEntities.SupportTeam.SolidarBuilder == Guid.Empty)
				return ReneeOperationResult<Guid?>.Failure(Labels.Errors.SolidarBuilderRequired);
			
			if(request.ZeroEnergyExclusionTerritoriesProgram)
			{
				var tzeeMaximumNumberOfAccompanyingFile = (await adminConstantsRepository.GetAll()).FirstOrDefault(c => c.Name == IndexLabels.MaximalNumberOfAccompanyingFileCreated)?.Value;
				var numberOfAccompanyingFileInTzeeProgram = await accompanyingFileRepository.CountAllActiveAndInTzeeProgramAccompanyingFile();

				if (numberOfAccompanyingFileInTzeeProgram >= tzeeMaximumNumberOfAccompanyingFile)
					return ReneeOperationResult<Guid?>.Failure(Labels.Errors.TzeeProgramDeadlineReached);
			}


			var accompanyingFile = AccompanyingFileExtension.InitializeAccompanyingFile(request.ConnectedUserId)
				.QuickAddSupportTeam(request.AccompanyingFileEntities.SupportTeam.CreateSupportTeam()).QuickAddHousing(
					request.AccompanyingFileEntities.HousingAddress.CreateAddress(),
					(int)request.AccompanyingFileEntities.GeographicalAreaTypology.Type,
					request.AccompanyingFileEntities.HousingPropertyType.GetAskedPropertyType())
				.QuickAddHousehold(request.AccompanyingFileEntities.MainOccupant.CreateOccupant());

			accompanyingFile.AccompanyingFileReference = AccompanyingFileHelper.CreateReference(
				request.AccompanyingFileEntities.SupportTeam.SolidarBuilderFullName,
				request.AccompanyingFileEntities.MainOccupant.Trigram,
				request.AccompanyingFileEntities.HousingAddress.PostalCode);
			
			accompanyingFile.CopropertyProfileId = request.AccompanyingFileEntities.HousingPropertyType.GetCopropertyProfileId();

			accompanyingFile.ZeroEnergyExclusionTerritoriesProgram = request.ZeroEnergyExclusionTerritoriesProgram;
			accompanyingFile.AccompanyingType = (int?)request.AccompanyingType;
			accompanyingFile.IsDeleted = false;
			accompanyingFile.AccompanyingFileTerritory = request.Territory;

			var numberItemsChanged = await accompanyingFileRepository.AddAccompanyingFile(accompanyingFile);

			return numberItemsChanged > 1
				? ReneeOperationResult<Guid?>.Success(accompanyingFile.Id, Labels.CreateAccompanyingFileSuccess)
				: ReneeOperationResult<Guid?>.Failure(Labels.Errors.ErrorWhileCreatingAccompanyingFile);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex, cancellationToken);
			return ReneeOperationResult<Guid?>.Failure(Labels.Errors.UnhandledErrorOccured);
		}
	}
}