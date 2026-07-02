using MediatR;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record CreateCopropertyProfileWithQuickAddCommandInput(
    Guid ConnectedUserId,
    QuickAddCreatedCopropertyProfileEntities CopropertyProfileEntities,
    bool ZeroEnergyExclusionTerritoriesProgram,
    AccompanyingType? AccompanyingType,
    Guid? Territory) : IRequest<ReneeOperationResult<Guid?>>;


public record QuickAddCreatedCopropertyProfileEntities(
    QuickAddCreatedAddress HousingAddress,
    QuickAddGeographicalAreaTypology GeographicalAreaTypology,
    QuickAddSupportTeam SupportTeam,
    List<Guid> RelatedAccompanyingFilesIds);
