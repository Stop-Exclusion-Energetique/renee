using MediatR;
using Renee.Application.Handlers.CommandHandlers.AccompanyingFile.CommandObjectResult;
using Renee.Application.Services.Administration.ImportCsvData;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateAccompanyingFileFromCsvFileCommandInput(
    List<UpdateAccompanyingFileFromCsvFileInput> Input,
    Guid UserId,
    Guid ImportRunId) : IRequest<ImportCsvDataCommandResult>;

public record UpdateAccompanyingFileFromCsvFileInput(
    Guid AccompanyingFileId,
    ParsedCsvData DataLine,
    List<Guid> ProjectTypeIds, 
    List<Guid> InsuranceTypeIds, 
    List<Guid?>? UpdatedHouseholdDifficulties,
    List<UpdatedHouseholdExpenses> UpdatedHouseholdExpenses,
    List<UpdatedHouseholdResource> UpdatedHouseholdResources,
    List<UpdatedHouseholdHeatingEnergy> UpdatedHouseholdHeatingEnergy
    );
