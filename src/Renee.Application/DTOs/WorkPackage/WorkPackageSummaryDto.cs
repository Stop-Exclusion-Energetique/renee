namespace Renee.Application.DTOs.WorkPackage;

public record WorkPackageSummaryDto(
    Guid Id,
    string CurrentWorkPackageWorkTypes,
    double WorkPackageTotalCost
    );
