using Renee.Application.CommandsUseCasesInput;
using Renee.Application.DTOs.CopropertyProfile;
using Renee.Application.Queries.Coproperty.QueryObjectResult;
using Renee.Application.Queries.Task;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;

namespace Renee.Application.Interfaces;

public  interface ICopropertyProfileService
{
    Task<ReneeOperationResult<Guid?>> CreateCopropertyProfileWithQuickAdd(
        QuickAddCreatedCopropertyProfileEntities copropertyProfileEntities,
        Guid connectedUserId,
        bool zeroEnergyExclusionTerritoriesProgram,
        AccompanyingType? accompanyingType,
        Guid? territory);

    Task<ReneeOperationResult<bool>> DeleteCopropertyProfile(Guid copropertyProfileId);

    Task<ReneeOperationResult<CopropertyProfileIdentificationDto?>> GetCopropertyProfileForIdentificationMilestoneById(Guid id, Guid userId, string userRole);

    Task<ReneeOperationResult<CopropertyProfileOrganizeAndFinanceDto?>> GetCopropertyProfileForOrganizeAndFinanceMilestoneById(Guid id, Guid userId, string userRole);

    Task<ReneeOperationResult<CopropertyProfileRealizeAndFollowDto?>> GetCopropertyProfileForRealizeAndFollowMilestoneById(Guid id, Guid userId, string userRole);

    Task<ReneeOperationResult<bool>> UpdateCopropertyIdentificationMilestone(SaveCopropertyProfileIdentificationMilestoneCommandInput input);

    Task<ReneeOperationResult<bool>> UpdateCopropertyOrganizeAndFinanceMilestone(SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput input);

    Task<ReneeOperationResult<bool>> UpdateCopropertRealizeAndFollowMilestone(SaveCopropertyProfileRealizeAndFollowMilestoneCommandInput input);

    Task<ReneeStringOperationResult> UpdateCopropertyProfileSynthesis(Guid copropertyProfileId, AccompanyingFileStatus status, AccompanyingFileStage stage, Guid userId);

    Task<ReneeStringOperationResult> ValidateCopropertyStage(Guid copropertyProfileId, bool isValidated, Guid userId, string? commentOnValidation);

    Task<ReneeOperationResult<GetCopropertyProfileListQueryObjectResult>> GetCopropertyProfileList(string role, Guid? userId);

    Task<ReneeOperationResult<GetAllAccompanyingFileTasksAndSupportTeamQueryObjectResult>> GetAllCopropertyProfileSupportTeam(Guid copropertyProfileId, Guid userId);
}
