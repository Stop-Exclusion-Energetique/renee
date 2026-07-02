using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateAccompanyingFileTargetInformationsCommandInput(Guid AccompanyingFileId,
    GeographicalHousingAreaTypology GeographicalHousingAreaTypology,
    AccompanyingType? AccompanyingType,
    Guid? AccompanyingTerritory) : IRequest<ReneeStringOperationResult>;

