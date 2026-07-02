using Renee.Application.CommandsUseCasesInput;
using Renee.Application.Handlers.CommandHandlers.Coproperty.OrganizeAndFinanceStageUseCase;
using Renee.Application.Interfaces;
using Renee.Domain.DomainExtension.ToRepository;

namespace CommandHandlerTests.CopropertyTests.OrganizeAndFinance;

public class SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandlerTests
{
    private readonly ICopropertyProfileRepository _copropertyProfileRepository = A.Fake<ICopropertyProfileRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    private static UpdateAids GetFakeAids() => new(
        DateOfAgVote: DateTime.UtcNow,
        MprCoproAids: 10000.0,
        ComplementaryAids: 5000.0
    );

    private static UpdateWorkTypeCost GetFakeWorkTypeCost(Guid workTypeId, double cost, string? description = null)
        => new(workTypeId, cost, description);

    private static UpdateWorkPackage GetFakeWorkPackage(Guid id, string effect, List<UpdateWorkTypeCost>? costs = null)
        => new(id, effect, costs ?? new List<UpdateWorkTypeCost>());

    private static WorkPackage GetDomainWorkPackage(Guid id, string effect, List<WorkPackageWorkTypeCost>? costs = null)
        => new()
        {
            Id = id,
            EnergeticsEffectAfterWorks = effect,
            WorkPackageWorkTypeCosts = costs ?? new List<WorkPackageWorkTypeCost>()
        };

    private static WorkPackageWorkTypeCost GetDomainWorkTypeCost(Guid workTypeId, double cost, string? description = null)
        => new()
        {
            WorkType = workTypeId,
            Cost = cost,
            Description = description
        };

    private static CopropertyWorkFinance GetFakeWorkFinance(List<WorkPackage>? workPackages = null)
        => new()
        {
            Id = Guid.NewGuid(),
            WorkPackages = workPackages ?? new List<WorkPackage>()
        };

    private static CopropertyProfile GetFakeProfile(CopropertyWorkFinance? workFinance = null)
        => new()
        {
            CopropertyWorkFinanceNavigation = workFinance ?? GetFakeWorkFinance()
        };

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenCopropertyProfileNotFound()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns((CopropertyProfile?)null);

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage>(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnTrue_WhenUpdateIsSuccessful()
    {
        // Arrange
        var workPackageId = Guid.NewGuid();
        var workTypeId = Guid.NewGuid();

        var domainWorkTypeCost = GetDomainWorkTypeCost(workTypeId, 1000, "Isolation");
        var domainWorkPackage = GetDomainWorkPackage(workPackageId, "Effet A", new List<WorkPackageWorkTypeCost> { domainWorkTypeCost });

        var workFinance = GetFakeWorkFinance(new List<WorkPackage> { domainWorkPackage });
        var copropertyProfile = GetFakeProfile(workFinance);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
            A<CopropertyProfile>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackageToUpdate>>._))
            .Returns(1);

        var updateWorkTypeCost = GetFakeWorkTypeCost(workTypeId, 1200, "Isolation modifiée");
        var updateWorkPackage = GetFakeWorkPackage(workPackageId, "Effet B", new List<UpdateWorkTypeCost> { updateWorkTypeCost });

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage> { updateWorkPackage },
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenUpdateReturnsMinusOne()
    {
        // Arrange
        var workPackageId = Guid.NewGuid();
        var workTypeId = Guid.NewGuid();

        var domainWorkTypeCost = GetDomainWorkTypeCost(workTypeId, 1000, "Isolation");
        var domainWorkPackage = GetDomainWorkPackage(workPackageId, "Effet A", new List<WorkPackageWorkTypeCost> { domainWorkTypeCost });

        var workFinance = GetFakeWorkFinance(new List<WorkPackage> { domainWorkPackage });
        var copropertyProfile = GetFakeProfile(workFinance);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
            A<CopropertyProfile>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackageToUpdate>>._))
            .Returns(-1);

        var updateWorkTypeCost = GetFakeWorkTypeCost(workTypeId, 1200, "Isolation modifiée");
        var updateWorkPackage = GetFakeWorkPackage(workPackageId, "Effet B", new List<UpdateWorkTypeCost> { updateWorkTypeCost });

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage> { updateWorkPackage },
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_ReturnFalse_WhenExceptionIsThrown()
    {
        // Arrange
        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Throws(new Exception("Database error"));

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage>(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeFalse();
    }

    [Fact]
    public async Task Handler_Should_Add_New_WorkPackage_If_Not_Exists()
    {
        // Arrange
        var workPackageId = Guid.NewGuid();
        var workTypeId = Guid.NewGuid();

        var workFinance = GetFakeWorkFinance(new List<WorkPackage>()); // No work packages initially
        var copropertyProfile = GetFakeProfile(workFinance);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
            A<CopropertyProfile>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackageToUpdate>>._))
            .Returns(1);

        var updateWorkTypeCost = GetFakeWorkTypeCost(workTypeId, 1500, "Nouveau coût");
        var updateWorkPackage = GetFakeWorkPackage(workPackageId, "Nouvel effet", new List<UpdateWorkTypeCost> { updateWorkTypeCost });

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage> { updateWorkPackage },
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_Remove_WorkPackage_If_Not_In_Updated_List()
    {
        // Arrange
        var workPackageId = Guid.NewGuid();
        var workTypeId = Guid.NewGuid();

        var domainWorkTypeCost = GetDomainWorkTypeCost(workTypeId, 1000, "Isolation");
        var domainWorkPackage = GetDomainWorkPackage(workPackageId, "Effet A", new List<WorkPackageWorkTypeCost> { domainWorkTypeCost });

        var workFinance = GetFakeWorkFinance(new List<WorkPackage> { domainWorkPackage });
        var copropertyProfile = GetFakeProfile(workFinance);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
            A<CopropertyProfile>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackageToUpdate>>._))
            .Returns(1);

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage>(),
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handler_Should_Update_WorkTypeCost_If_Exists()
    {
        // Arrange
        var workPackageId = Guid.NewGuid();
        var workTypeId = Guid.NewGuid();

        var domainWorkTypeCost = GetDomainWorkTypeCost(workTypeId, 1000, "Isolation");
        var domainWorkPackage = GetDomainWorkPackage(workPackageId, "Effet A", new List<WorkPackageWorkTypeCost> { domainWorkTypeCost });

        var workFinance = GetFakeWorkFinance(new List<WorkPackage> { domainWorkPackage });
        var copropertyProfile = GetFakeProfile(workFinance);

        A.CallTo(() => _copropertyProfileRepository.GetCopropertyProfileForOrganizeAndFinanceMilestoneAsync(A<Guid>._))
            .Returns(copropertyProfile);

        A.CallTo(() => _copropertyProfileRepository.UpdateCopropertyProfileForOrganizeAndFinanceMilestoneAsync(
            A<CopropertyProfile>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackage>>._,
            A<List<WorkPackageToUpdate>>._))
            .Returns(1);

        var updateWorkTypeCost = GetFakeWorkTypeCost(workTypeId, 2000, "Isolation améliorée");
        var updateWorkPackage = GetFakeWorkPackage(workPackageId, "Effet A", new List<UpdateWorkTypeCost> { updateWorkTypeCost });

        var command = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandInput(
            Guid.NewGuid(),
            GetFakeAids(),
            new List<UpdateWorkPackage> { updateWorkPackage },
            Guid.NewGuid()
        );
        var handler = new SaveCopropertyProfileOrganizeAndFinanceMilestoneCommandHandler(_copropertyProfileRepository, _telemetry);

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
}