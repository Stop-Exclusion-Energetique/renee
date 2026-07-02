using MediatR;
using Renee.Domain.ReneeError;

namespace Renee.Application.CommandsUseCasesInput;

public record UpdateAccompanyingFileFacturationInformationsCommandInput(
    Guid AccompanyingFileId,
    bool BilledJalon1,
    double? AmountBilledFirstStage,
    DateTime? FundraisingLauchDateForFirstStage,
    string? BillingCallNumberFirstStage,
    string? InvoiceNumberFirstStage,
    DateTime? BillingDateFirstStage,
    bool BilledJalon2,
    double? AmountBilledSecondStage,
    DateTime? FundraisingLauchDateForSecondStage,
    string? BillingCallNumberSecondStage,
    string? InvoiceNumberSecondStage,
    DateTime? BillingDateSecondStage,
    bool BilledJalon3,
    double? AmountBilledThirdStage,
    DateTime? FundraisingLauchDateForThirdStage,
    string? BillingCallNumberThirdStage,
    string? InvoiceNumberThirdStage,
    DateTime? BillingDateThirdStage) : IRequest<ReneeStringOperationResult>;
