using FluentAssertions;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using Renee.Application.Interfaces;
using Renee.UI.Components.CGUHandling.CGUModal;

namespace UITests.Components.CGUHandling;

public class CGUModalTests
{
    [Fact]
    public void CGUModal_WhenShowIsTrue_ShouldDisplayModal()
    {
        using var ctx = new BunitContext();
        ctx.Services.AddSingleton(A.Fake<IFileViewerService>());
        ctx.Services.AddSingleton<NotificationService>();

        var cut = ctx.Render<CGUModal>(parameters => parameters
            .Add(p => p.Show, true)
            .Add(p => p.LabelCGU, "CGU_v1.pdf")
        );

        cut.Markup.Should().Contain("modal-title");
        cut.Markup.Should().Contain("CGU_v1.pdf");
    }

    [Fact]
    public void CGUModal_WhenCheckedAndAcceptClicked_ShouldInvokeOnAccepted()
    {
        using var ctx = new BunitContext();
        ctx.Services.AddSingleton(A.Fake<IFileViewerService>());
        ctx.Services.AddSingleton<NotificationService>();
        bool accepted = false;

        var cut = ctx.Render<CGUModal>(parameters => parameters
            .Add(p => p.Show, true)
            .Add(p => p.LabelCGU, "CGU_v1.pdf")
            .Add(p => p.OnAccepted, EventCallback.Factory.Create(this, () => accepted = true))
        );

        cut.Find("input[type=checkbox]").Change(true);
        cut.Find("button").Click();

        accepted.Should().BeTrue();
    }
}
