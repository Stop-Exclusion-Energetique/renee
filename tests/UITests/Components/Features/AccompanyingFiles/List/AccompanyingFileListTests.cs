using BlazorContextMenu;
using Blazored.Modal.Services;
using Bunit.TestDoubles;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Radzen;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Interfaces;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.AccompanyingFile.QueryObjectResult;
using Renee.Domain;
using Renee.Domain.Enums;
using Renee.Domain.ReneeError;
using Renee.UI;
using Renee.UI.Components.Features.AccompanyingFiles.List;
using System.Security.Claims;

namespace UITests.Components.Features.AccompanyingFiles.List;

public class AccompanyingFileListTests : BunitContext
{
    private void SetupContext(string role = Constants.SolidarBuilderRole)
    {
        var auth = this.AddAuthorization();
        auth.SetAuthorized("testuser@sqli.com");
        auth.SetClaims(
            new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, role)
        );

        Services.AddSingleton(A.Fake<IUserService>());
        Services.AddSingleton(A.Fake<NotificationService>());
        Services.AddSingleton(A.Fake<IModalService>());
        Services.AddSingleton(A.Fake<IInternalContextMenuHandler>());
        Services.AddSingleton(A.Fake<AccompanyingFileListFilterDataPersistance>());
        JSInterop.Mode = JSRuntimeMode.Loose;
    }

    private IRenderedComponent<CreatedAccompanyingFilesByUser> RenderWithFake(
        IEnumerable<UserAccompanyingFileResume> dataset,
        Func<GetAccompanyingFileForFileListQuery?, IEnumerable<UserAccompanyingFileResume>>? filter = null,
        LoadAccompanyingFileListDataQueryObjectResult? filterData = null)
    {
        var sendEvent = A.Fake<ISendEventQuery>();

        A.CallTo(() => sendEvent.Send(A<GetAccompanyingFileForFileListQuery>._))
            .ReturnsLazily(call =>
            {
                var query = call.GetArgument<GetAccompanyingFileForFileListQuery>(0);
                var result = filter == null ? dataset : filter(query);
                return Task.FromResult(ReneeOperationResult<GetAccompanyingFileForFileListQueryObjectResult>.Success(new GetAccompanyingFileForFileListQueryObjectResult
                {
                    AccompanyingFiles = [.. result],
                    TotalAccompanyingFilesCount = 50
                }));
            });

        A.CallTo(() => sendEvent.Send(A<LoadFilterDataBasedOnRoleQuery>._))
            .ReturnsLazily(_ => Task.FromResult(ReneeOperationResult<LoadAccompanyingFileListDataQueryObjectResult>.Success(filterData ?? new LoadAccompanyingFileListDataQueryObjectResult())));

        Services.AddSingleton(sendEvent);
        return Render<CreatedAccompanyingFilesByUser>();
    }

    private static IEnumerable<UserAccompanyingFileResume> FilterText(
        GetAccompanyingFileForFileListQuery? q,
        IEnumerable<UserAccompanyingFileResume> data,
        Func<UserAccompanyingFileResume, string?> selector,
        bool multiField = false,
        params Func<UserAccompanyingFileResume, string?>[] extraSelectors)
    {
        var term = q?.FilterOptions.FilterValue;
        if (string.IsNullOrWhiteSpace(term)) return data;

        if (multiField)
        {
            return data.Where(r =>
                (selector(r)?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false) ||
                extraSelectors.Any(es => es(r)?.Contains(term, StringComparison.OrdinalIgnoreCase) ?? false));
        }

        return data.Where(r => selector(r)?.Contains(term, StringComparison.OrdinalIgnoreCase) == true);
    }

    private void RunSimpleFilterTest(
        string searchTerm,
        IEnumerable<UserAccompanyingFileResume> dataset,
        Func<UserAccompanyingFileResume, string?> selector,
        int expectedCount)
    {
        SetupContext();
        var cut = RenderWithFake(dataset, q => FilterText(q, dataset, selector));
        cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList(searchTerm));
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(expectedCount);
    }

    [Fact]
    public void Filter_Address_Bretagne()
    {
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(),Reference = "AB-1234-5678", Address = "Pont Germain Muller 67000 Strasbourg" },
            new() { Id = Guid.NewGuid(), Reference = "CD-1234-5678", Address = "20 Chemin de Bretagne 68000 Marseilles" },
            new() { Id = Guid.NewGuid(), Reference = "EF-1234-5678", Address = "82 Impasse de Bretagne 71000 Bordeaux" },
            new() { Id = Guid.NewGuid(), Reference = "GH-1234-5678", Address = "87 Place Bretagne 75000 Paris" }
        };
        RunSimpleFilterTest("Bretagne", data, d => d.Address, 3);
    }

    [Fact]
    public void Filter_Reference_AB()
    {
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Reference = "AB-1234-5678", Address = "Pont Germain Muller 67000 Strasbourg" },
            new() { Id = Guid.NewGuid(), Reference = "CD-1234-5678", Address = "20 Chemin de Bretagne 68000 Marseilles" },
            new() { Id = Guid.NewGuid(), Reference = "EF-1234-5678", Address = "82 Impasse de Bretagne 71000 Bordeaux" },
            new() { Id = Guid.NewGuid(), Reference = "GH-1234-5678", Address = "87 Place Bretagne 75000 Paris" }
        };
        RunSimpleFilterTest("AB", data, d => d.Reference, 1);
    }

    [Fact]
    public void Filter_FirstName_Edou()
    {
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Reference = "AB-76100-2305696", Address = "2 rue Péri Montélan", FirstName = "Georges", LastName = "Bale" },
            new() { Id = Guid.NewGuid(), Reference = "FE-761920-2305696", Address = "11 rue Jules Verne", FirstName = "Edouard", LastName = "Lopez" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = "5 rue parc Gustave Flaubert", FirstName = "Bernard", LastName = "LeRenard" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = string.Empty },
            new() { Id = Guid.NewGuid(), Reference = "AB-76100-2305896", Address = string.Empty }
        };
        RunSimpleFilterTest("Edou", data, d => d.FirstName, 1);
    }

    [Fact]
    public void Filter_LastName_Lope()
    {
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Reference = "AB-76100-2305696", Address = "2 rue Péri Montélan", FirstName = "Georges", LastName = "Bale" },
            new() { Id = Guid.NewGuid(), Reference = "FE-761920-2305696", Address = "11 rue Jules Verne", FirstName = "Edouard", LastName = "Lopez" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = "5 rue parc Gustave Flaubert", FirstName = "Bernard", LastName = "LeRenard" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = string.Empty },
            new() { Id = Guid.NewGuid(), Reference = "AB-76100-2305896", Address = string.Empty }
        };
        RunSimpleFilterTest("Lope", data, d => d.LastName, 1);
    }

    [Fact]
    public void Filter_AllTargets_AR()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Reference = "AB-76100-2305696", Address = "2 rue Péri Montélan", FirstName = "Georges", LastName = "LeRenard" },
            new() { Id = Guid.NewGuid(), Reference = "FE-761920-2305696", Address = "5 rue parc Gustave Flaubert", FirstName = "Edouard", LastName = "Lopez" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = "11 rue Jules Verne", FirstName = "Bernard", LastName = "Bale" },
            new() { Id = Guid.NewGuid(), Reference = "PE-76100-2305896", Address = string.Empty },
            new() { Id = Guid.NewGuid(), Reference = "AR-76100-2305896", Address = string.Empty }
        };

        var cut = RenderWithFake(data, q => FilterText(
            q,
            data,
            d => d.Reference,
            true,
            d => d.FirstName,
            d => d.LastName,
            d => d.Address));

        cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList("AR"));
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(4);
    }

    [Fact]
    public void List_WithThreeElements()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };

        var cut = RenderWithFake(data);
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }

    [Fact]
    public void Empty_List_ShouldShowMessage()
    {
        SetupContext();
        var cut = RenderWithFake([]);
        cut.FindAll("span").FirstOrDefault(s => s.InnerHtml.Contains(Labels.NoAccompanyingFileMatchCriteria))
            .Should().NotBeNull();
    }

    [Fact]
    public void Empty_List_ShouldReturnEmptyCollection()
    {
        SetupContext();
        var cut = RenderWithFake([]);
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().BeEmpty();
    }

    [Fact]
    public async Task NoMatch_ShouldShowMessage()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Reference = "AB-1234-5678" },
            new() { Id = Guid.NewGuid(), Reference = "CD-1234-5678" },
            new() { Id = Guid.NewGuid(), Reference = "EF-1234-5678" }
        };

        var cut = RenderWithFake(data, q => FilterText(
            q,
            data,
            d => d.Reference,
            true,
            d => d.FirstName,
            d => d.LastName,
            d => d.Address));

        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList("MNO"));

        cut.FindAll("span").Any(s => s.InnerHtml.Contains(Labels.NoAccompanyingFileMatchCriteria))
            .Should().BeTrue();
    }

    [Fact]
    public async Task Sort_By_LastModification_Ascending()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), LastModificationDateUtc = new DateTime(2024, 12, 23, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), LastModificationDateUtc = new DateTime(2023, 12, 25, 0, 0, 0, DateTimeKind.Utc) },
            new() { Id = Guid.NewGuid(), LastModificationDateUtc = new DateTime(2023, 02, 23, 0, 0, 0, DateTimeKind.Utc) }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions.SortingState == SortingState.Ascending
                ? data.OrderBy(d => d.LastModificationDateUtc)
                : data);

        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList(sortingState: SortingState.Ascending));

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles
            .Select(a => a.LastModificationDateUtc)
            .Should().BeInAscendingOrder();
    }

    [Fact]
    public async Task Filter_Stage_Identify()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.OrganizingAndFinancing },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.RealisationAndFollowing }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions?.AccompanyingFileStages.Count == 0
                ? data
                : data.Where(d => d.Stage == AccompanyingFileStage.Identify));

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStages = [AccompanyingFileStage.Identify];
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(2);
    }

    [Fact]
    public async Task Filter_Stage_Identify_Or_RealizeAndFollow()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.OrganizingAndFinancing },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify },
            new() { Id = Guid.NewGuid(), Stage = AccompanyingFileStage.RealisationAndFollowing }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions?.AccompanyingFileStages.Count == 0
                ? data
                : data.Where(d => d.Stage is AccompanyingFileStage.Identify or AccompanyingFileStage.RealisationAndFollowing));

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStages = [
            AccompanyingFileStage.Identify,
            AccompanyingFileStage.RealisationAndFollowing
        ];
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }

    [Fact]
    public async Task Filter_Status_WaitingForApproval()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), Status = AccompanyingFileStatus.WaitingForApproval },
            new() { Id = Guid.NewGuid(), Status = AccompanyingFileStatus.InProgress },
            new() { Id = Guid.NewGuid(), Status = AccompanyingFileStatus.Rejected },
            new() { Id = Guid.NewGuid(), Status = AccompanyingFileStatus.WaitingForApproval }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions?.AccompanyingFileStatuses.Count == 0
                ? data
                : data.Where(d => d.Status == AccompanyingFileStatus.WaitingForApproval));

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStatuses = [AccompanyingFileStatus.WaitingForApproval];
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(2);
    }

    [Fact]
    public async Task Filter_StageIdentify_StatusInProgress()
    {
        SetupContext();
        var data = new List<UserAccompanyingFileResume>
        {
            new() {Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify, Status = AccompanyingFileStatus.WaitingForApproval},
            new() {Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify, Status = AccompanyingFileStatus.InProgress},
            new() {Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify, Status = AccompanyingFileStatus.InProgress},
            new() {Id = Guid.NewGuid(), Stage = AccompanyingFileStage.RealisationAndFollowing, Status = AccompanyingFileStatus.InProgress},
            new() {Id = Guid.NewGuid(), Stage = AccompanyingFileStage.Identify, Status = AccompanyingFileStatus.Rejected}
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions?.AccompanyingFileStatuses.Count == 0
                ? data
                : data.Where(d => d.Stage == AccompanyingFileStage.Identify && d.Status == AccompanyingFileStatus.InProgress));

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStages = [AccompanyingFileStage.Identify];
        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.AccompanyingFileStatuses = [AccompanyingFileStatus.InProgress];
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(2);
    }

    [Fact]
    public void DiffuseCoordinator_ListHasThreeElements()
    {
        SetupContext(Constants.DiffuseCoordinatorRole);
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };
        var cut = RenderWithFake(data);
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }

    [Fact]
    public async Task FilterContext_MyReportingStructure()
    {
        SetupContext();
        var rs = Guid.NewGuid();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), ReportingStructureId = rs },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions.FilterContext == FilterContext.MyReportingStructure
                ? data.Where(d => d.ReportingStructureId == rs)
                : data);

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.FilterContext = FilterContext.MyReportingStructure;
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }

    [Fact]
    public async Task DiffuseCoordinator_AllFiles_Context()
    {
        SetupContext(Constants.DiffuseCoordinatorRole);
        var diffuseId = Guid.NewGuid();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), DiffuseCoordinatorId = diffuseId },
            new() { Id = Guid.NewGuid(), DiffuseCoordinatorId = diffuseId },
            new() { Id = Guid.NewGuid(), DiffuseCoordinatorId = diffuseId },
            new() { Id = Guid.NewGuid(), DiffuseCoordinatorId = diffuseId },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid() }
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions.FilterContext == FilterContext.AllFiles
                ? data
                : data.Where(d => d.DiffuseCoordinatorId == diffuseId));

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.FilterContext = FilterContext.AllFiles;
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(data.Count);
    }

    [Fact]
    public async Task Admin_Filter_WithoutSolidarBuilders()
    {
        SetupContext(Constants.AdminRole);
        var data = new List<UserAccompanyingFileResume>
        {
            new() {Id = Guid.NewGuid(), HasSolidarBuilderDeletedHisAccount = true, HasSecondSolidarBuilderDeletedHisAccount=true, HasThirdSolidarBuilderDeletedHisAccount=true},
            new() {Id = Guid.NewGuid()},
            new() {Id = Guid.NewGuid(), HasSolidarBuilderDeletedHisAccount = true, HasSecondSolidarBuilderDeletedHisAccount=true, HasThirdSolidarBuilderDeletedHisAccount=true},
            new() {Id = Guid.NewGuid(), HasSolidarBuilderDeletedHisAccount = true, HasSecondSolidarBuilderDeletedHisAccount=true, HasThirdSolidarBuilderDeletedHisAccount=true},
            new() {Id = Guid.NewGuid()},
            new() {Id = Guid.NewGuid(), HasSolidarBuilderDeletedHisAccount = true, HasSecondSolidarBuilderDeletedHisAccount=true, HasThirdSolidarBuilderDeletedHisAccount=true}
        };

        var cut = RenderWithFake(data, q =>
            q?.FilterOptions.FilterContext == FilterContext.WithoutSolidarBuilders
                ? data.Where(d => d.HasSolidarBuilderDeletedHisAccount &&
                                   d.HasSecondSolidarBuilderDeletedHisAccount &&
                                   d.HasThirdSolidarBuilderDeletedHisAccount)
                : data);

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.FilterContext = FilterContext.WithoutSolidarBuilders;
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(4);
    }

    [Fact]
    public async Task TerritorialBuilder_Filter_ReportingStructure()
    {
        SetupContext(Constants.TerritorialBuilderRole);
        var rs = Guid.NewGuid();
        var tb = Guid.NewGuid();

        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), TerritorialBuilder = tb, ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), TerritorialBuilder = tb, ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), TerritorialBuilder = tb, ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), ReportingStructureId = rs },
            new() { Id = Guid.NewGuid(), TerritorialBuilder = tb },
            new() { Id = Guid.NewGuid(), TerritorialBuilder = tb }
        };

        var filterData = new LoadAccompanyingFileListDataQueryObjectResult
        {
            ReportingStructures = [new(rs, "RS", Guid.NewGuid())]
        };
        var cut = RenderWithFake(data, q =>
            q?.FilterOptions?.ReportingStructures?.Count == 0
                ? data.Where(d => d.TerritorialBuilder == tb)
                : data.Where(d => d.TerritorialBuilder == tb && d.ReportingStructureId == rs),
            filterData);

        cut.Instance.CreatedAccompanyingFileByUsersFilterViewModel.ReportingStructureIdList.Add(rs);
        await cut.InvokeAsync(() => cut.Instance.UpdateAccompanyingFilesList());

        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }

    [Fact]
    public void StructuralReferent_Filter_NationalStructure()
    {
        SetupContext(Constants.StructuralReferentRole);
        var ns = Guid.NewGuid();
        var data = new List<UserAccompanyingFileResume>
        {
            new() { Id = Guid.NewGuid(), NationalStructureId = ns },
            new() { Id = Guid.NewGuid(), NationalStructureId = ns },
            new() { Id = Guid.NewGuid() },
            new() { Id = Guid.NewGuid(), NationalStructureId = ns }
        };

        var cut = RenderWithFake(data, _ => data.Where(d => d.NationalStructureId == ns));
        cut.Instance.CreatedAccompanyingFilesListViewModel.AccompanyingFiles.Should().HaveCount(3);
    }
}
