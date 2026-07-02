using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.DomainExtension;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.QuickAddUseCases;

public class CreateCopropertyProfileWithQuickAddCommandHandler(
    
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService,
    IAccompanyingFileRepository accompanyingFileRepository)
    : IRequestHandler<CreateCopropertyProfileWithQuickAddCommandInput, ReneeOperationResult<Guid?>>
{
    private static readonly Random _random = new();
    public async Task<ReneeOperationResult<Guid?>> Handle(
        CreateCopropertyProfileWithQuickAddCommandInput request,
        CancellationToken cancellationToken)
    {
        try
        {
            if (request.CopropertyProfileEntities.SupportTeam.SolidarBuilder == Guid.Empty)
                return ReneeOperationResult<Guid?>.Failure(Labels.Errors.SolidarBuilderRequired);

            var copropertyProfile = CopropertyProfileExtension.InitializeCopropertyProfile(request.ConnectedUserId)
                .QuickAddHousing(request.CopropertyProfileEntities.HousingAddress.CreateAddress(), 
                (int)request.CopropertyProfileEntities.GeographicalAreaTypology.Type)
                .QuickAddSupportTeam(request.CopropertyProfileEntities.SupportTeam.CreateSupportTeam());

            copropertyProfile.CopropertyReference = CreateReference(request.CopropertyProfileEntities.SupportTeam.SolidarBuilderFullName,
                request.CopropertyProfileEntities.HousingAddress.PostalCode);

            copropertyProfile.ZeroEnergyExclusionTerritoriesProgram = request.ZeroEnergyExclusionTerritoriesProgram;
            copropertyProfile.AccompanyingType = (int?)request.AccompanyingType;
            copropertyProfile.IsDeleted = false;
            copropertyProfile.CopropertyTerritory = request.Territory;

            var numberItemsChanged = await copropertyProfileRepository.AddCopropertyProfile(copropertyProfile);

            if (numberItemsChanged <= 1) return ReneeOperationResult<Guid?>.Failure(Labels.Errors.CopropertyProfileCreationFailed);

            if (request.CopropertyProfileEntities.RelatedAccompanyingFilesIds.Count > 0)
            {
                await accompanyingFileRepository.UpdateAccompanyingFilesCopropertyProfileId(
                       request.CopropertyProfileEntities.RelatedAccompanyingFilesIds,
                       copropertyProfile.Id);
            }

            return ReneeOperationResult<Guid?>.Success(copropertyProfile.Id);
		}
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<Guid?>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    public static string CreateReference(
        string SolidarBuilderFullName,
        string postalCode,
        DateTime? creationDate = null)
    {
        var randomCharacter = Extractcharacter(SolidarBuilderFullName);
        return $"{SolidarBuilderFullName}-COPRO-{randomCharacter}-{postalCode}-{creationDate ?? DateTime.Now:dd/MM/yyyy}";
    } 

    public static string Extractcharacter(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return string.Empty;

        var characters = input.Replace(" ", "").ToCharArray();

        if (characters.Length < 3)
            return new string(characters); 

        return new string([.. characters
            .OrderBy(c => _random.Next())
            .Take(3)]);
    }
}
