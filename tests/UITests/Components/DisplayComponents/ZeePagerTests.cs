using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Radzen.Blazor;
using Renee.UI.Components.DisplayComponents;

namespace UITests.Components.DisplayComponents;

public class ZeePagerTests : BunitContext
{
	private void SetupContext()
	{
		Services.AddSingleton(A.Fake<NotificationService>());
		JSInterop.Mode = JSRuntimeMode.Loose;
	}

	private IRenderedComponent<ZeePager> CreatePager(
		int total = 120,
		int pageIndex = 1,
		int pageSize = 10,
		Action<int>? onPageIndexChanged = null,
		Action<int>? onPageSizeChanged = null,
		Action<PagingChangedArgs>? onPagingChanged = null,
		bool showPageSizeSelector = true)
	{
		return Render<ZeePager>(parameters => parameters
			.Add(p => p.TotalCount, total)
			.Add(p => p.PageIndex, pageIndex)
			.Add(p => p.PageIndexChanged, EventCallback.Factory.Create(this, onPageIndexChanged ?? (_ => { })))
			.Add(p => p.PageSize, pageSize)
			.Add(p => p.PageSizeChanged, EventCallback.Factory.Create(this, onPageSizeChanged ?? (_ => { })))
			.Add(p => p.PagingChanged, EventCallback.Factory.Create(this, onPagingChanged ?? (_ => { })))
			.Add(p => p.ShowPageSizeSelector, showPageSizeSelector));
	}

	[Fact]
	public void ZeePager_ShouldDisplayCorrectSummaryText()
	{
		// Arrange
		SetupContext();

		var cut = CreatePager(total: 250, pageIndex: 2, pageSize: 25);

		// Act
		var summary = cut.Find(".pager-summary").TextContent;

		// Assert
		summary.Should().Contain("Page 2").And.Contain("250");
	}

	[Fact]
	public void ZeePager_PageIndexOutOfRangeParameters_ShouldBeClamped()
	{
		// Arrange
		SetupContext();

		var cutLow = CreatePager(total: 50, pageIndex: 0, pageSize: 10);
		var cutHigh = CreatePager(total: 50, pageIndex: 999, pageSize: 10);

		// Act
		var currentLow = cutLow.FindAll(".pager-page").First(b => b.ClassList.Contains("pager-active"));
		var currentHigh = cutHigh.FindAll(".pager-page").First(b => b.ClassList.Contains("pager-active"));

		// Assert
		currentLow.TextContent.Should().Be("1");
		currentHigh.TextContent.Should().Be(Math.Ceiling(50 / 10.0).ToString());
	}

	[Fact]
	public void ZeePager_VisiblePages_ShouldAdaptAtEdgesAndMiddle()
	{
		// Arrange
		SetupContext();

		// Act
		var cutStart = CreatePager(total: 120, pageIndex: 1, pageSize: 10);
		var visiblePagesStart = cutStart.FindAll("button.pager-page").Select(b => b.TextContent);

		var cutMiddle = CreatePager(total: 120, pageIndex: 6, pageSize: 10);
		var visiblePagesMiddle = cutMiddle.FindAll("button.pager-page").Select(b => b.TextContent);

		var cutNearEnd = CreatePager(total: 120, pageIndex: 11, pageSize: 10);
		var visiblePagesNearEnd = cutNearEnd.FindAll("button.pager-page").Select(b => b.TextContent);

		var cutEnd = CreatePager(total: 120, pageIndex: 12, pageSize: 10);
		var visiblePagesEnd = cutEnd.FindAll("button.pager-page").Select(b => b.TextContent);

		// Assert
		visiblePagesStart.Should().Equal("1", "2", "3");
		visiblePagesMiddle.Should().Equal("4", "5", "6");
		visiblePagesNearEnd.Should().Equal("9", "10", "11");
		visiblePagesEnd.Should().Equal("10", "11", "12");
	}

	[Fact]
	public void ZeePager_ClickNextPage_ShouldInvokeCallbacksAndUpdatePageIndex()
	{
		// Arrange
		SetupContext();

		int? receivedPageIndex = null;
		PagingChangedArgs? pagingArgs = null;
		var cut = CreatePager(total: 100, pageIndex: 1, pageSize: 10, onPageIndexChanged: i => receivedPageIndex = i, onPagingChanged: a => pagingArgs = a);

		// Act
		var nextBtn = cut.Find("#next-page-btn");
		nextBtn.Click();

		// Assert
		receivedPageIndex.Should().Be(2);
		pagingArgs.Should().NotBeNull();
		pagingArgs.PageIndex.Should().Be(2);
		pagingArgs.ElementsToSkip.Should().Be(10);
		cut.FindAll("button.pager-page").First(b => b.ClassList.Contains("pager-active")).TextContent.Should().Be("2");
	}

	[Fact]
	public void ZeePager_ClickLastPageButton_ShouldGoToLastPage()
	{
		// Arrange
		SetupContext();

		int? receivedPageIndex = null;
		var cut = CreatePager(total: 95, pageIndex: 1, pageSize: 10, onPageIndexChanged: i => receivedPageIndex = i);

		// Act
		var lastBtn = cut.Find("#last-page-btn");
		lastBtn.Click();

		// Assert
		receivedPageIndex.Should().Be(10);
	}

	[Fact]
	public void ZeePager_DisableButtonsOnFirstAndLastPage()
	{
		// Arrange
		SetupContext();

		var cutFirst = CreatePager(total: 30, pageIndex: 1, pageSize: 10);
		var cutLast = CreatePager(total: 30, pageIndex: 3, pageSize: 10);

		// Act
		var prevFirst = cutFirst.Find("#previous-page-btn");
		var nextLast = cutLast.Find("#next-page-btn");

		// Assert
		prevFirst.HasAttribute("disabled").Should().BeTrue();
		nextLast.HasAttribute("disabled").Should().BeTrue();
	}

	[Fact]
	public void ZeePager_PageNumberClick_ShouldChangePage()
	{
		// Arrange
		SetupContext();

		int? received = null;
		var cut = CreatePager(total: 50, pageIndex: 1, pageSize: 10, onPageIndexChanged: i => received = i);

		// Act
		cut.FindAll("button.pager-page").First(b => b.TextContent == "2").Click();

		// Assert
		received.Should().Be(2);
	}

	[Fact]
	public async Task ZeePager_PageSizeChange_ShouldSetPageIndexOnLastPageIndexIfOutOfRange()
	{
		// Arrange
		SetupContext();

		int? sizeChanged = null;
		int? pageChanged = null;
		PagingChangedArgs? pagingArgs = null;
		var cut = CreatePager(total: 60, pageIndex: 3, pageSize: 25, onPageSizeChanged: s => sizeChanged = s, onPageIndexChanged: p => pageChanged = p, onPagingChanged: a => pagingArgs = a);

		// Act
		var pageSizeSelector = cut.FindComponents<RadzenDropDown<int>>().First(c => (int)c.Instance.Value == 25);
		await pageSizeSelector.InvokeAsync(() => pageSizeSelector.Instance.ValueChanged.InvokeAsync(50));

		// Assert
		sizeChanged.Should().Be(50);
		pageChanged.Should().Be(2);
		pagingArgs.Should().NotBeNull();
		pagingArgs.PageIndex.Should().Be(2);
		pagingArgs.ElementsToSkip.Should().Be(50);
	}

	[Fact]
	public async Task ZeePager_PageSizeChange_ShouldKeepPageIndexIfStillValid()
	{
		// Arrange
		SetupContext();

		int? sizeChanged = null;
		int? pageChanged = null;
		PagingChangedArgs? pagingArgs = null;
		var cut = CreatePager(total: 200, pageIndex: 3, pageSize: 25, onPageSizeChanged: s => sizeChanged = s, onPageIndexChanged: p => pageChanged = p, onPagingChanged: a => pagingArgs = a);

		// Act
		var pageSizeSelector = cut.FindComponents<RadzenDropDown<int>>().First(c => (int)c.Instance.Value == 25);
		await pageSizeSelector.InvokeAsync(() => pageSizeSelector.Instance.ValueChanged.InvokeAsync(50));

		// Assert
		sizeChanged.Should().Be(50);
		pageChanged.Should().Be(3);
		pagingArgs.Should().NotBeNull();
		pagingArgs.PageIndex.Should().Be(3);
		pagingArgs.ElementsToSkip.Should().Be(100);
	}

	[Fact]
	public void ZeePager_WhenShowPageSizeSelectorFalse_ShouldHideSelector()
	{
		// Arrange
		SetupContext();

		var cut = CreatePager(showPageSizeSelector: false);

		// Act
		var selector = cut.Find(".pager-right");

		// Assert
		selector.InnerHtml.Should().BeEmpty();
	}
}
