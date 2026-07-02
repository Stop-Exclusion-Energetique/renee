using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Coproperty.IdentificationStageUseCase;

public class SaveCopropertyProfileIdentificationMilestoneCommandHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<SaveCopropertyProfileIdentificationMilestoneCommandInput, ReneeOperationResult<bool>>

{
    public async Task<ReneeOperationResult<bool>> Handle(
        SaveCopropertyProfileIdentificationMilestoneCommandInput request,
        CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile = await copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync(request.CopropertyProfileId);

            if (copropertyProfile == null)
                return ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorCopropertyProfileNotFound);

            copropertyProfile.UpdatedBy = request.ConnectedUserId;
            copropertyProfile.LastUpdateDate = DateTime.UtcNow;

            var copropertyHousing = request.UpdatedCopropertyHousing;
            UpdateCopropertyHousing(copropertyProfile.CopropertyHousingNavigation, copropertyHousing);

            var copropertyGovernanceContacts = request.UpdatedGovernanceContats;
            UpdateCopropertyGovernanceContacts(copropertyProfile.CopropertyGovernanceNavigation, copropertyGovernanceContacts);

            var copropertyDiagnostics = request.UpdatedDiagnostics;
            UpdateCopropertyDiagnostics(copropertyProfile.CopropertyDiagnosticsNavigation, copropertyDiagnostics);

            var address = request.UpdatedAddress;
            UpdateCopropertyAddress(copropertyProfile.CopropertyHousingNavigation.HousingAddressNavigation, address);

            var numberItemsChanged = await copropertyProfileRepository.UpdateCopropertyProfileForIdentificationMilestoneAsync(copropertyProfile);

            return numberItemsChanged > -1
                ? ReneeOperationResult<bool>.Success(true, Labels.SaveMilestoneSuccess)
                : ReneeOperationResult<bool>.Failure(Labels.Errors.ErrorWhileSavingMilestone);
        }
        catch (Exception ex) {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<bool>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    private static void UpdateCopropertyHousing(CopropertyHousing copropertyHousing, UpdatedCopropertyHousing updatedCopropertyHousing)
    {
        copropertyHousing.GeographicAreaTypology = updatedCopropertyHousing.GeographicAreaTypology;
        copropertyHousing.HousingType = updatedCopropertyHousing.HousingType;
        copropertyHousing.NumberOfLots = updatedCopropertyHousing.NumberOfLots;
        copropertyHousing.NumberOfFloor = updatedCopropertyHousing.NumberOfFloor;
        copropertyHousing.HeatingType = updatedCopropertyHousing.HeatingType;
        copropertyHousing.PerilType = updatedCopropertyHousing.PerilType;
    }

    private static void UpdateCopropertyGovernanceContacts(CopropertyGovernance copropertyGovernance, UpdatedGovernanceContats updatedGovernanceContats)
    {
        copropertyGovernance.NatureOfSyndic = updatedGovernanceContats.NatureOfSyndic;
        copropertyGovernance.NameOfSyndic = updatedGovernanceContats.NameOfSyndic;
        copropertyGovernance.PhoneOfSyndic = updatedGovernanceContats.PhoneOfSyndic;
        copropertyGovernance.MailOfSyndic = updatedGovernanceContats.MailOfSyndic;
        copropertyGovernance.NameOfAmo = updatedGovernanceContats.NameOfAmo;
        copropertyGovernance.ContactOfAmo = updatedGovernanceContats.ContactOfAmo;
        copropertyGovernance.NumberOfContacts = updatedGovernanceContats.NumberOfContacts;
    }

    private static void UpdateCopropertyDiagnostics(CopropertyDiagnostics copropertyDiagnostics, UpdatedDiagnostics updatedDiagnostics)
    {
        copropertyDiagnostics.BuildingDpeEnergy = updatedDiagnostics.BuildingDpeEnergy;
        copropertyDiagnostics.BuildingDpeLabel = updatedDiagnostics.BuildingDpeLabel;
        copropertyDiagnostics.ApartmentDpeEnergy = updatedDiagnostics.ApartmentDpeEnergy;
        copropertyDiagnostics.ApartmentDpeLabel = updatedDiagnostics.ApartmentDpeLabel;
    }

    public static void UpdateCopropertyAddress(Address address, UpdatedCopropertyAddress updatedAddress)
    {
        address.Label = updatedAddress.Label ?? string.Empty;
        address.PostalCode = updatedAddress.PostalCode ?? string.Empty;
        address.City = updatedAddress.City ?? string.Empty;
        address.Department = updatedAddress.Department ?? string.Empty;
        address.Region = updatedAddress.Region ?? string.Empty;
        address.AdditionnalComment = updatedAddress.AdditionalComment ?? string.Empty;
    }
}
