using MediatR;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.DTOs.Occupant;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.QueryHandlers.Coproperty;

public class GetCopropertyProfileForIdentificationMilestoneQueryHandler(
    ICopropertyProfileRepository copropertyProfileRepository,
    ITelemetryService telemetryService)
    : IRequestHandler<GetCopropertyProfileForIdentificationMilestoneQuery, ReneeOperationResult<CopropertyProfileIdentificationDto?>>
{
    public async Task<ReneeOperationResult<CopropertyProfileIdentificationDto?>> Handle(
        GetCopropertyProfileForIdentificationMilestoneQuery request,
        CancellationToken cancellationToken)
    {
        try
        {
            var copropertyProfile =
                await copropertyProfileRepository.GetCopropertyProfileForIdentificationMilestoneAsync((Guid)request.CopropertyProfileId!);

            if (copropertyProfile is null)
                return ReneeOperationResult<CopropertyProfileIdentificationDto?>.Failure(Labels.Errors.ErrorWhileLoadingCopropertyProfile);

            if (IsUserInSupportTeam(copropertyProfile, request.UserId) || IsUserACoordinatorOrAdmin(request.UserRole))
                return ReneeOperationResult<CopropertyProfileIdentificationDto?>.Success(ToCopropertyProfileForIdentification(copropertyProfile));

            return ReneeOperationResult<CopropertyProfileIdentificationDto?>.Failure(Labels.Errors.UserNotAllowedToSeeAccompanyingFile);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeOperationResult<CopropertyProfileIdentificationDto?>.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }

    private static bool IsUserInSupportTeam(CopropertyProfile copropertyProfile, Guid userId)
        => userId == copropertyProfile.CopropertySupportTeamNavigation?.TerritorialBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SecondTerritorialBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SolidarBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.SecondSolidarBuilder ||
            userId == copropertyProfile.CopropertySupportTeamNavigation?.ThirdSolidarBuilder;

    private static bool IsUserACoordinatorOrAdmin(string userRole)
        => userRole.Equals(Constants.DiffuseCoordinatorRole) ||
            userRole.Equals(Constants.TargetedCoordinatorRole) ||
            userRole.Equals(Constants.AdminRole);

    private static CopropertyProfileIdentificationDto ToCopropertyProfileForIdentification(CopropertyProfile copropertyProfile)
    {
        return new CopropertyProfileIdentificationDto
        {
            Id = copropertyProfile.Id,
            Reference = copropertyProfile.CopropertyReference,
            Stage = (AccompanyingFileStage)copropertyProfile.CopropertyMilestone,
            Status = (AccompanyingFileStatus)copropertyProfile.CopropertyStatus,
            TerritorialBuilderId = copropertyProfile.CopropertySupportTeamNavigation.TerritorialBuilder,
            CreationDatetimeUtc = copropertyProfile.CreationDate,
            LastUpdateDatetimeUtc = copropertyProfile.LastUpdateDate,
            SolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SolidarBuilder,
            SecondSolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SecondSolidarBuilder,
            ThirdSolidarBuilderId = copropertyProfile.CopropertySupportTeamNavigation.ThirdSolidarBuilder,
            DiffuseCoordinatorId = copropertyProfile.CopropertySupportTeamNavigation.DiffuseCoordinator,
            SecondTerritorialBuilderId = copropertyProfile.CopropertySupportTeamNavigation.SecondSolidarBuilder,
            TargetedCoordinatorId = copropertyProfile.CopropertySupportTeamNavigation.TargetCoordinator,
            IsInTzeeProgram = copropertyProfile.ZeroEnergyExclusionTerritoriesProgram,
            Typology = (GeographicalHousingAreaTypology?)copropertyProfile.CopropertyHousingNavigation.GeographicAreaTypology,
            HousingType = (HousingType?)copropertyProfile.CopropertyHousingNavigation.HousingType,
            NumberOfFloor = copropertyProfile.CopropertyHousingNavigation.NumberOfFloor,
            NumberOfLots = copropertyProfile.CopropertyHousingNavigation.NumberOfLots,
            Address = CreateAddressDto(copropertyProfile.CopropertyHousingNavigation.HousingAddressNavigation),
            AdditionalAddress = copropertyProfile.CopropertyHousingNavigation.HousingAddressNavigation.AdditionnalComment,
            HeatingType = (HeatingType?)copropertyProfile.CopropertyHousingNavigation.HeatingType,
            PerilType = (PerilType?)copropertyProfile.CopropertyHousingNavigation.PerilType,
            NatureOfSyndicType = (NatureOfSyndicType?)copropertyProfile.CopropertyGovernanceNavigation.NatureOfSyndic,
            NameOfSyndic = copropertyProfile.CopropertyGovernanceNavigation.NameOfSyndic,
            PhoneOfSyndic = copropertyProfile.CopropertyGovernanceNavigation.PhoneOfSyndic,
            MailOfSyndic = copropertyProfile.CopropertyGovernanceNavigation.MailOfSyndic,
            NameOfAmo = copropertyProfile.CopropertyGovernanceNavigation.NameOfAmo,
            ContactOfAmo = copropertyProfile.CopropertyGovernanceNavigation.ContactOfAmo,
            NumberOfContacts = copropertyProfile.CopropertyGovernanceNavigation.NumberOfContacts,
            BuildingDpeLabel = (DpeLabel?)copropertyProfile.CopropertyDiagnosticsNavigation.BuildingDpeLabel,
            BuildingDpeEnergy = copropertyProfile.CopropertyDiagnosticsNavigation.BuildingDpeEnergy,
            ApartmentDpeLabel = (DpeLabel?)copropertyProfile.CopropertyDiagnosticsNavigation.ApartmentDpeLabel,
            ApartmentDpeEnergy = copropertyProfile.CopropertyDiagnosticsNavigation.ApartmentDpeEnergy
        };
    }
    private static AddressDto CreateAddressDto(Address addressEntity)
    {
        return new AddressDto
        {
            Label = addressEntity.Label,
            PostalCode = addressEntity.PostalCode,
            City = addressEntity.City,
            Department = addressEntity.Department,
            Region = addressEntity.Region,
            Street = addressEntity.Street,
            HouseNumber = addressEntity.HouseNumber
        };
    }
}
