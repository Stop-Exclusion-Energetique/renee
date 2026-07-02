using Microsoft.AspNetCore.Components;

namespace Renee.UI.Components.DisplayComponents;

public partial class ZeePager
{
    [Parameter, EditorRequired] public int TotalCount { get; set; }

    [Parameter] public int PageIndex { get; set; } = 1;
    [Parameter] public EventCallback<int> PageIndexChanged { get; set; }

    [Parameter] public int PageSize { get; set; } = 25;
    [Parameter] public EventCallback<int> PageSizeChanged { get; set; }

    [Parameter] public IReadOnlyList<int> PageSizeOptions { get; set; } = [25, 50, 100];
    [Parameter] public bool ShowPageSizeSelector { get; set; } = true;
    [Parameter] public string PageSizeLabel { get; set; } = "Résultats par page";

    [Parameter] public EventCallback<PagingChangedArgs> PagingChanged { get; set; }

    private int PageCount => Math.Max(1, (int)Math.Ceiling(TotalCount / (double)Math.Max(1, PageSize)));
    private int ElementsToSkip => (PageIndex - 1) * PageSize;
    private bool IsFirstPage => PageIndex == 1;
    private bool IsLastPage => PageIndex == PageCount;
	private IEnumerable<int> VisiblePages
	{
		get
		{
			var total = PageCount;
			const int maxVisible = 3;
			if (total <= maxVisible)
				return Enumerable.Range(1, total);

			int start = PageIndex - 2;
			int end = start + maxVisible - 1;

			if (start < 1)
			{
				start = 1;
				end = maxVisible;
			}

			if (end > total)
			{
				end = total;
				start = Math.Max(1, end - maxVisible + 1);
			}

			return Enumerable.Range(start, end - start + 1);
		}
	}

	protected override void OnParametersSet()
    {
        PageIndex = Math.Min(Math.Max(PageIndex, 1), PageCount);
    }

    private async Task RaiseChangedAsync()
    {
        await PageSizeChanged.InvokeAsync(PageSize);
        await PageIndexChanged.InvokeAsync(PageIndex);
        await PagingChanged.InvokeAsync(new PagingChangedArgs(PageIndex, ElementsToSkip));
    }

    private async Task SetPage(int newIndex)
    {
        var clamped = Math.Min(Math.Max(newIndex, 1), PageCount);
        if (clamped != PageIndex)
        {
            PageIndex = clamped;
            await RaiseChangedAsync();
        }
    }

    private async Task SetPageSize(int newSize)
    {
        if (newSize <= 0 || newSize == PageSize) return;

        PageSize = newSize;

        PageIndex = Math.Min(PageIndex, PageCount);
        if (TotalCount <= (PageIndex - 1) * PageSize)
            PageIndex = 1;

        await RaiseChangedAsync();
    }
}

public record PagingChangedArgs(int PageIndex, int ElementsToSkip);