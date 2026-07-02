using FakeItEasy;
using FluentAssertions;
using Renee.Application.Handlers.QueryHandlers.AccompanyingFile;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Domain;
using Renee.Domain.DomainExtension.FilterOptions;
using Renee.Domain.Entity;
using Renee.Domain.Enums;
using Renee.Domain.Repositories;

namespace QueryHandlerTests.AccompanyingFileTests.List;

public class GetAccompanyingFileForFileListQueryHandlerTests
{
    private readonly IAccompanyingFileRepository _repo = A.Fake<IAccompanyingFileRepository>();
    private readonly IUserRepository _users = A.Fake<IUserRepository>();
    private readonly ITelemetryService _telemetry = A.Fake<ITelemetryService>();

    private GetAccompanyingFileForFileListQueryHandler CreateHandler() => new(_repo, _users, _telemetry);

    private static GetAccompanyingFileForFileListQuery CreateQuery(
        string role = Constants.SolidarBuilderRole,
        Guid? userId = null)
    {
        var uid = userId ?? Guid.NewGuid();
        var filters = new AccompanyingFilesListFilterOptions(
            [AccompanyingFileStage.Identify],
            [AccompanyingFileStatus.InProgress],
            [AccompanyingFileNeedingBilling.ToBillStageOne],
            "ref",
            SortingState.Descending,
            FilterContext.MyAccompanyingFile,
            [Guid.NewGuid()],
            [Guid.NewGuid()],
            [Guid.NewGuid()]);

        return new GetAccompanyingFileForFileListQuery(
            uid,
            role,
            filters,
            0,
            25);
    }

    private static (List<AccompanyingFileResumeView>, int) CreateRepoResult()
    {
        return (
		[
			new()
            {
                Id = Guid.NewGuid(),
                AccompanyingFileReference = "REF-1",
                FirstName = "A",
                LastName = "B",
                Label = "Adr",
                AccompanyingFileMilestone = (int)AccompanyingFileStage.OrganizingAndFinancing,
                AccompanyingFileStatus = (int)AccompanyingFileStatus.InProgress,
                OpeningDate = DateTime.UtcNow.AddDays(-10),
                LastUpdateDate = DateTime.UtcNow,
                SolidarBuilder = Guid.NewGuid(),
                SecondSolidarBuilder = Guid.NewGuid(),
                ThirdSolidarBuilder = Guid.NewGuid(),
                ReportingStructureId = Guid.NewGuid(),
                DiffuseCoordinator = Guid.NewGuid(),
                TargetCoordinator = Guid.NewGuid(),
                AccompanyingFileTerritory = Guid.NewGuid(),
                TerritorialBuilder = Guid.NewGuid(),
                SecondTerritorialBuilder = Guid.NewGuid(),
                NationalStructureId = Guid.NewGuid(),
                SolidarBuilderDeleted = false,
                SecondSolidarBuilderDeleted = true,
                ThirdSolidarBuilderDeleted = false,
                ShouldAccompanyingFileBeSubmittedToAnah = true
            }
        ], 17);
    }

    private void MockUser(Guid id, Guid? reportingStructureId)
    {
        A.CallTo(() => _users.GetUserById(id))
            .Returns(new User { Id = id, ReportingStructureId = reportingStructureId });
    }

    [Fact]
    public async Task EmptyUser_ShouldReturnEmpty()
    {
        var handler = CreateHandler();
        var query = CreateQuery(userId: Guid.Empty);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value.Should().BeNull();
    }

    [Fact]
    public async Task NoReportingStructure_ShouldReturnEmpty()
    {
        var handler = CreateHandler();
        var query = CreateQuery();
        MockUser(query.UserId, null);

        var result = await handler.Handle(query, CancellationToken.None);

		result.Value.Should().BeNull();
    }

    [Fact]
    public async Task SolidarBuilder_Flow_ShouldMapResult_AndFilterOptionPruning()
    {
        var handler = CreateHandler();
        var query = CreateQuery(Constants.SolidarBuilderRole);
        var reportingStructureId = Guid.NewGuid();
        MockUser(query.UserId, reportingStructureId);
        var data = CreateRepoResult();

        AccompanyingFilesListFilterOptions? filters = null;

        A.CallTo(() => _repo.GetAllAccompanyingFilesForSolidarBuilders(
                query.UserId, reportingStructureId, A<AccompanyingFilesListFilterOptions>._,
                query.NumberOfItempsToSkip, query.PageSize))
         .Invokes(c => filters = c.GetArgument<AccompanyingFilesListFilterOptions>(2))
         .Returns(data);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.TotalAccompanyingFilesCount.Should().Be(data.Item2);
        result.Value!.AccompanyingFiles.Should().HaveCount(1);
        var item = result.Value!.AccompanyingFiles.First();
        item.Stage.Should().Be(AccompanyingFileStage.OrganizingAndFinancing);
        item.Status.Should().Be(AccompanyingFileStatus.InProgress);
        item.HasSecondSolidarBuilderDeletedHisAccount.Should().BeTrue();
        item.HasSolidarBuilderDeletedHisAccount.Should().BeFalse();
        item.ShouldAccompanyingFileBeSubmittedToAnah.Should().BeTrue();
        filters?.ReportingStructures.Should().BeNull();
        filters?.SolidarBuilders.Should().BeNull();
        filters?.Territories.Should().BeNull();
    }

    [Theory]
    [InlineData(Constants.AssociationMemberRole)]
    [InlineData(Constants.AdminRole)]
    public async Task AssociationOrAdmin_ShouldUseAdminRepo_WithReportingStructures(string role)
    {
        var handler = CreateHandler();
        var query = CreateQuery(role);
        MockUser(query.UserId, Guid.NewGuid());
        var data = CreateRepoResult();

        AccompanyingFilesListFilterOptions? filters = null;
        A.CallTo(() => _repo.GetAllAccompanyingFileForAssociationMembersAndAdmins(
            A<AccompanyingFilesListFilterOptions>._, query.NumberOfItempsToSkip, query.PageSize))
            .Invokes(c => filters = c.GetArgument<AccompanyingFilesListFilterOptions>(0))
            .Returns(data);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.TotalAccompanyingFilesCount.Should().Be(data.Item2);
        filters?.ReportingStructures.Should().NotBeNull();
        filters?.SolidarBuilders.Should().BeNull();
        filters?.Territories.Should().BeNull();
    }

    [Theory]
    [InlineData(Constants.DiffuseCoordinatorRole)]
    [InlineData(Constants.TargetedCoordinatorRole)]
    public async Task Coordinators_ShouldIncludeTerritoriesAndSolidarBuildersFilters(string role)
    {
        var handler = CreateHandler();
        var query = CreateQuery(role);
        MockUser(query.UserId, Guid.NewGuid());
        var data = CreateRepoResult();

        AccompanyingFilesListFilterOptions? filters = null;
        A.CallTo(() => _repo.GetAllAccompanyingFilesForCoordinators(
            query.UserId, A<AccompanyingFilesListFilterOptions>._, query.NumberOfItempsToSkip, query.PageSize))
            .Invokes(c => filters = c.GetArgument<AccompanyingFilesListFilterOptions>(1))
            .Returns(data);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.TotalAccompanyingFilesCount.Should().Be(data.Item2);
        filters?.Territories.Should().NotBeNull();
        filters?.SolidarBuilders.Should().NotBeNull();
    }

    [Fact]
    public async Task TerritorialBuilder_ShouldCallRepository_WithSolidarBuilders()
    {
        var handler = CreateHandler();
        var query = CreateQuery(Constants.TerritorialBuilderRole);
        MockUser(query.UserId, Guid.NewGuid());
        var data = CreateRepoResult();

        AccompanyingFilesListFilterOptions? filters = null;
        A.CallTo(() => _repo.GetAllAccompanyingFilesForTerritorialBuilders(
            query.UserId, A<AccompanyingFilesListFilterOptions>._, query.NumberOfItempsToSkip, query.PageSize))
            .Invokes(c => filters = c.GetArgument<AccompanyingFilesListFilterOptions>(1))
            .Returns(data);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.AccompanyingFiles.Should().HaveCount(1);
        filters?.SolidarBuilders.Should().NotBeNull();
    }

    [Fact]
    public async Task StructuralReferent_ShouldCallRepository_NoExtraCollections()
    {
        var handler = CreateHandler();
        var query = CreateQuery(Constants.StructuralReferentRole);
        var reportingStructureId = Guid.NewGuid();
        MockUser(query.UserId, reportingStructureId);
        var data = CreateRepoResult();

        AccompanyingFilesListFilterOptions? filters = null;
        A.CallTo(() => _repo.GetAllAccompanyingFilesForStructuralReferents(
            reportingStructureId, A<AccompanyingFilesListFilterOptions>._, query.NumberOfItempsToSkip, query.PageSize))
            .Invokes(c => filters = c.GetArgument<AccompanyingFilesListFilterOptions>(1))
            .Returns(data);

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.AccompanyingFiles.Should().HaveCount(1);
        filters?.ReportingStructures.Should().BeNull();
        filters?.SolidarBuilders.Should().BeNull();
        filters?.Territories.Should().BeNull();
    }

    [Fact]
    public async Task UnknownRole_ShouldReturnEmpty()
    {
        var handler = CreateHandler();
        var query = CreateQuery("UNKNOWN");
        MockUser(query.UserId, Guid.NewGuid());

        var result = await handler.Handle(query, CancellationToken.None);

        result.Value!.AccompanyingFiles.Should().BeEmpty();
        result.Value!.TotalAccompanyingFilesCount.Should().Be(0);
    }

    [Fact]
    public async Task Exception_ShouldTrackTelemetry_AndReturnEmpty()
    {
        var handler = CreateHandler();
        var query = CreateQuery(Constants.SolidarBuilderRole);
        var reportingStructureId = Guid.NewGuid();
        MockUser(query.UserId, reportingStructureId);

        A.CallTo(() => _repo.GetAllAccompanyingFilesForSolidarBuilders(
            A<Guid>._, A<Guid>._, A<AccompanyingFilesListFilterOptions>._, A<int>._, A<int>._))
            .ThrowsAsync(new InvalidOperationException());

        var result = await handler.Handle(query, CancellationToken.None);

		result.Value.Should().BeNull();
        A.CallTo(() => _telemetry.TrackExceptionAsync(A<Exception>._, A<CancellationToken?>._))
            .MustHaveHappened();
    }
}
