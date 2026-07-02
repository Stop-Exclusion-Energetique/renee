using MediatR;
using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Entity;
using Renee.Domain.ReneeError;
using Renee.Domain.Repositories;

namespace Renee.Application.Handlers.CommandHandlers.Administration;

public class UpdateAccompanyingFileFacturationInformationsCommandHandler(IAccompanyingFileRepository accompanyingFileRepository,
    ITelemetryService telemetryService) : IRequestHandler<UpdateAccompanyingFileFacturationInformationsCommandInput, ReneeStringOperationResult>
{
    public async Task<ReneeStringOperationResult> Handle(UpdateAccompanyingFileFacturationInformationsCommandInput request, CancellationToken cancellationToken)
    {
        try
        {
            var accompanyingFile = await accompanyingFileRepository.GetAccompanyingFileWithBillingLog(request.AccompanyingFileId);

            if (accompanyingFile == null)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileLoadingAccompanyingFile);

            UpdateBillingInformation(accompanyingFile, request);

            var result = await accompanyingFileRepository.UpdateAccompanyingFileBillingLog(accompanyingFile);

            if (!result)
                return ReneeStringOperationResult.Failure(Labels.Errors.ErrorWhileUpdatingAccompanyingFile);

            return ReneeStringOperationResult.Success(Labels.AccompanyingFileFacturationInformationUpdateSuccessMessage);
        }
        catch (Exception ex)
        {
            await telemetryService.TrackExceptionAsync(ex, cancellationToken);
            return ReneeStringOperationResult.Failure(Labels.Errors.UnhandledErrorOccured);
        }
    }
    
    private void UpdateBillingInformation(Domain.Entity.AccompanyingFile accompanyingFile, UpdateAccompanyingFileFacturationInformationsCommandInput request)
    {
        var billingLog = accompanyingFile.AccompanyingFileBillingLog ?? new AccompanyingFileBillingLog
        {
            AccompanyingFileId = accompanyingFile.Id
        };

        billingLog.BilledJalon1 = request.BilledJalon1;
        billingLog.AmountBilledFirstStage = request.AmountBilledFirstStage;
        billingLog.FundraisingLauchDateForFirstStage = request.FundraisingLauchDateForFirstStage;
        billingLog.BillingCallNumberFirstStage = request.BillingCallNumberFirstStage;
        billingLog.InvoiceNumberFirstStage = request.InvoiceNumberFirstStage;
        billingLog.BillingDateFirstStage = request.BillingDateFirstStage;

        billingLog.BilledJalon2 = request.BilledJalon2;
        billingLog.AmountBilledSecondStage = request.AmountBilledSecondStage;
        billingLog.FundraisingLauchDateForSecondStage = request.FundraisingLauchDateForSecondStage;
        billingLog.BillingCallNumberSecondStage = request.BillingCallNumberSecondStage;
        billingLog.InvoiceNumberSecondStage = request.InvoiceNumberSecondStage;
        billingLog.BillingDateSecondStage = request.BillingDateSecondStage;

        billingLog.BilledJalon3 = request.BilledJalon3;
        billingLog.AmountBilledThirdStage = request.AmountBilledThirdStage;
        billingLog.FundraisingLauchDateForThirdStage = request.FundraisingLauchDateForThirdStage;
        billingLog.BillingCallNumberThirdStage = request.BillingCallNumberThirdStage;
        billingLog.InvoiceNumberThirdStage = request.InvoiceNumberThirdStage;
        billingLog.BillingDateThirdStage = request.BillingDateThirdStage;

        accompanyingFile.AccompanyingFileBillingLog = billingLog;
    }
}
