using MediatR;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Commands.Administration;

public class UpdateAccompanyingFileMaximumValueHandler(IAdminConstantRepository adminConstantRepository): IRequestHandler<UpdateAccompanyingFileMaximumValue, ReneeOperationResult<bool>>
{
    public async Task<ReneeOperationResult<bool>> Handle(UpdateAccompanyingFileMaximumValue request, CancellationToken cancellationToken)
    {
        var adminConstantsToUpdate = await adminConstantRepository.GetAll();
        
        if (request.maximalNumberOfAccompanyingFileCreated.HasValue)
        {
            UpdateAdminConstantValue(IndexLabels.MaximalNumberOfAccompanyingFileCreated, request.maximalNumberOfAccompanyingFileCreated.Value, adminConstantsToUpdate);
            UpdateAdminConstantValue(IndexLabels.MaximalNumberOfAccompanyingFileToValidateFirstStage, request.maximalNumberOfAccompanyingFileToValidateFirstStage, adminConstantsToUpdate);
        }

        UpdateAdminConstantDate(IndexLabels.TzeeAccompanyingFileModificationDeadline, request.accompanyingFileModificationDeadline, adminConstantsToUpdate);
        UpdateAdminConstantDate(IndexLabels.TzeeAccompanyingFileAlertBannerStartDate, request.accompanyingFileAlertBannerStartDate, adminConstantsToUpdate);
        UpdateAdminConstantDate(IndexLabels.TzeeAccompanyingFileAlertBannerEndDate, request.accompanyingFileAlertBannerEndDate, adminConstantsToUpdate);
        UpdateAdminConstantString(IndexLabels.TzeeAccompanyingFileAlertBannerMessage, request.accompanyingFileAlertBannerMessage, adminConstantsToUpdate);

        
        var result = await adminConstantRepository.UpdateValue(adminConstantsToUpdate);
        
        return result > 0 ? ReneeOperationResult<bool>.Success(true) : ReneeOperationResult<bool>.Failure("Failed to update admin constants.");
    }

    private void UpdateAdminConstantValue(string name, int? value, List<AdminConstant> adminConstantsToUpdate)
    {
        var adminConstantToUpdate = adminConstantsToUpdate.FirstOrDefault(x => x.Name == name);
        if (adminConstantToUpdate is not null && value.HasValue)
        {
            adminConstantToUpdate.Value = value.Value;
        }
    }

    private void UpdateAdminConstantDate(string name, DateTime? value, List<AdminConstant> adminConstantsToUpdate)
    {
        var adminConstantToUpdate = adminConstantsToUpdate.FirstOrDefault(x => x.Name == name);
        if (adminConstantToUpdate is not null && value.HasValue)
        {
            adminConstantToUpdate.AccompanyingFileModificationDeadline = value.Value;
        }
    }

    private void UpdateAdminConstantString(string name, string? value, List<AdminConstant> adminConstantsToUpdate)
    {
        var adminConstantToUpdate = adminConstantsToUpdate.FirstOrDefault(x => x.Name == name);
        if (adminConstantToUpdate is not null)
        {
            adminConstantToUpdate.AccompanyingFileAlertBannerMessage = value;
        }
    }
}