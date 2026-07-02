using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.Coproperty;
using Renee.Application.Queries.Coproperty.QueryObjectResult;
using Renee.Application.Queries.Task;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Services;

public sealed class CopropertyProfileService(
    IMediator mediator
    ) : ICopropertyProfileService
{

    public async Task<ReneeOperationResult<Guid?>> CreateCopropertyProfileWithQuickAdd(
        QuickAddCreatedCopropertyProfileEntities copropertyProfileEntities,
        Guid connectedUserId,
        bool zeroEnergyExclusionTerritoriesProgram,
        AccompanyingType? accompanyingType,
        Guid? territory)
    {
        return await mediator.Send(
            new CreateCopropertyProfileWithQuickAddCommandInput(
                connectedUserId,
                copropertyProfileEntities,
                zeroEnergyExclusionTerritoriesProgram,
                accompanyingType,
                territory));
    }

    public async Task<ReneeOperationResult<bool>> DeleteCopropertyProfile(Guid copropertyProfileId) =>
        await mediator.Send(new DeleteCopropertyProfileCommandInput(copropertyProfileId));

    public async Task<ReneeOperationResult<CopropertyProfileIdentificationDto?>> GetCopropertyProfileForIdentificationMilestoneById(Guid id, Guid userId, string userRole) =>
        await mediator.Send(new GetCopropertyProfileForIdentificationMilestoneQuery(id, userId, userRole));

    public async Task<ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>> GetCopropertyProfileForOrganizeAndFinanceMilestoneById(Guid id, Guid userId, string userRole) =>
        await mediator.Send(new GetCopropertyProfileForOrganizeAndFinanceMilestoneQuery(id, userId, userRole));

    public async Task<ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>> GetCopropertyProfileForRealizeAndFollowMilestoneById(Guid id, Guid userId, string userRole) =>
        await mediator.Send(new GetCopropertyProfileForRealizeAndFollowMilestoneQuery(id, userId, userRole));

    public async Task<ReneeOperationResult<bool>> UpdateCopropertyIdentificationMilestone(SaveCopropertyProfileIdentificationMilestoneCommandInput input) =>
        await mediator.Send(input);

    public async Task<ReneeOperationResult<bool>> UpdateCopropertyOrganizeAndFinanceMilestone(SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput input) =>
        await mediator.Send(input);

    public async Task<ReneeOperationResult<bool>> UpdateCopropertRealizeAndFollowMilestone(SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput input) =>
        await mediator.Send(input);

    public async Task<ReneeStringOperationResult> UpdateCopropertyProfileSynthesis(Guid copropertyProfileId, AccompanyingFileStatus status, AccompanyingFileStage stage, Guid userId) =>
        await mediator.Send(new SaveCopropertyMilestoneSynthesisValidationCommandInput(copropertyProfileId, status, stage, userId));

    public async Task<ReneeStringOperationResult> ValidateCopropertyStage(Guid copropertyProfileId, bool isValidated, Guid userId, string? commentOnValidation) =>
        await mediator.Send(new ValidateCopropertyProfileStageChangeCommandInput(copropertyProfileId, isValidated, userId, commentOnValidation));

    public async Task<ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>> GetCopropertyProfileList(string role, Guid? userId) =>
        await mediator.Send(new GetCopropertyProfileListQuery(role, userId));

    public async Task<ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>> GetAllCopropertyProfileSupportTeam(Guid copropertyProfileId, Guid userId) =>
        await mediator.Send(new GetAllAccompanyingFileTaskAndSupportTeamQuery
        {
            AssociatedResourceId = copropertyProfileId,
            UserId = userId,
            IsCoproperty = true
        });
}
