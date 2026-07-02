using Microsoft.AspNetCore.Components;
using Renee.Application.Abstraction.Query.SendEvent;
using Renee.Application.Queries.AccompanyingFile;
using Renee.Application.Queries.Coproperty;
using Renee.Domain.Enums;

namespace Renee.UI.Components.Layout.StageLayout;

public partial class StageLayout
{
    [Inject] public NavigationManager NavigationManager { get; set; } = null!;
    [Inject] public IStageNavigationStateService StageNavigationStateService { get; set; } = null!;
    [Inject] public ISendEventQuery SendEventQuery { get; set; } = null!;

    private bool IsStateLoaded;
    private Guid _associatedResourceId;
    private bool _isAccompanyingFile { get; set; } = true;

    private AccompanyingFileStage CurrentStage { get; set; }

    private AccompanyingFileStage RequestedStage { get; set; }

    protected override async Task OnInitializedAsync()
    {
        var currentPageUrl = NavigationManager.Uri;

        var urlParts = currentPageUrl.Split('/');
        var associatedResourceId = urlParts[^1];

        _isAccompanyingFile = !currentPageUrl.Contains("coproperty", StringComparison.OrdinalIgnoreCase);

        if (Guid.TryParse(associatedResourceId, out var associatedResourceGuid))
        {
            _associatedResourceId = associatedResourceGuid;


            var endpointSegment = urlParts.Length >= 2 ? urlParts[^2] : string.Empty;

            if (_isAccompanyingFile)
            {
                var result = await SendEventQuery.Send(new GetAccompanyingFileForHeadBandQuery(associatedResourceGuid));
                if (result.IsSuccess && result.Value is not null)
                    ApplyLoadedStage(associatedResourceGuid,
                        result.Value.AccompanyingFileStage,
                        result.Value.AccompanyingFileReference,
                        StageNavigation.StageNavigationAccompanyingFileList,
                        endpointSegment);
            }
            else
            {
                var result = await SendEventQuery.Send(new GetCopropertyProfileForHeadBandQuery(associatedResourceGuid));
                if (result.IsSuccess && result.Value is not null)
                    ApplyLoadedStage(associatedResourceGuid,
                        result.Value.AccompanyingFileStage,
                        result.Value.CopropertyProfileReference,
                        StageNavigation.StateNavigationCopropertyProfileList,
                        endpointSegment);
            }

            if (RequestedStage > CurrentStage)
            {
                StageNavigationStateService.SetSelectedStage(CurrentStage);
                var safeUrl = StageNavigationStateService.BuildUrl(CurrentStage);

                NavigationManager.NavigateTo(safeUrl, replace: true);
                IsStateLoaded = true;
                return;
            }

            StageNavigationStateService.SetSelectedStage(RequestedStage);

            StageNavigationStateService.MilestoneChanged += async (sender, e) => await UpdateStateAfterSave();
            IsStateLoaded = true;
        }
    }

    private void ChangeStage(AccompanyingFileStage stage)
    {
        StageNavigationStateService.SetSelectedStage(stage);
        NavigationManager.NavigateTo(StageNavigationStateService.BuildUrl(stage));
    }

    private async Task UpdateStateAfterSave()
    {
        if (StageNavigationStateService.SelectedStage is null)
            return;

        if (_isAccompanyingFile)
        {
            var result = await SendEventQuery.Send(new GetAccompanyingFileForHeadBandQuery(_associatedResourceId));
            if (result.IsSuccess && result.Value is not null)
                StageNavigationStateService.LoadStageNavigationService(
                    _associatedResourceId,
                    result.Value.AccompanyingFileStage,
                    result.Value.AccompanyingFileReference,
                    StageNavigation.StageNavigationAccompanyingFileList);
        }
        else
        {
            var result = await SendEventQuery.Send(new GetCopropertyProfileForHeadBandQuery(_associatedResourceId));
            if (result.IsSuccess && result.Value is not null)
                StageNavigationStateService.LoadStageNavigationService(
                    _associatedResourceId,
                    result.Value.AccompanyingFileStage,
                    result.Value.CopropertyProfileReference,
                    StageNavigation.StateNavigationCopropertyProfileList);
        }

        StageNavigationStateService.SetSelectedStage((AccompanyingFileStage)StageNavigationStateService.SelectedStage);

        StateHasChanged();
    }

    private void ApplyLoadedStage(
    Guid resourceId,
    AccompanyingFileStage stage,
    string reference,
    List<StageNavigationModel> navigationList,
    string? endpointSegment = null)
    {
        StageNavigationStateService.LoadStageNavigationService(resourceId, stage, reference, navigationList);

        CurrentStage = StageNavigationStateService.CurrentStage;
        if (endpointSegment is not null)
        {
            RequestedStage = StageNavigationStateService.StageNavigationModels
                .FirstOrDefault(m => m.Url.EndsWith(endpointSegment, StringComparison.OrdinalIgnoreCase))
                ?.Stage
                ?? stage;
        }
    }

}