using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record SaveCopropertyProfileIdentificationMilestoneCommandInput(
    Guid CopropertyProfileId,
    UpdatedCopropertyAddress UpdatedAddress,
    UpdatedCopropertyHousing UpdatedCopropertyHousing,
    UpdatedGovernanceContats UpdatedGovernanceContats,
    UpdatedDiagnostics UpdatedDiagnostics,
    Guid ConnectedUserId
    ) : IRequest<ReneeOperationResult<bool>>;

public record UpdatedCopropertyAddress(
    string? Label,
    string? PostalCode,
    string? City,
    string? Department,
    string? Region,
    string? AdditionalComment);

public record UpdatedCopropertyHousing(
    int? GeographicAreaTypology,
    int? HousingType,
    int? NumberOfLots,
    int? NumberOfFloor,
    int? HeatingType,
    int? PerilType
    );


public record UpdatedGovernanceContats(
    int? NatureOfSyndic,
    string? NameOfSyndic,
    string? PhoneOfSyndic,
    string? MailOfSyndic,
    string? NameOfAmo,
    string? ContactOfAmo,
    int? NumberOfContacts
    );

public record UpdatedDiagnostics(
    int? BuildingDpeLabel,
    double? BuildingDpeEnergy,
    int? ApartmentDpeLabel,
    double? ApartmentDpeEnergy
    );


