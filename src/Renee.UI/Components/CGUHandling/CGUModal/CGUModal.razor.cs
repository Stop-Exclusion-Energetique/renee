using Microsoft.AspNetCore.Components;
using Renee.Application.Interfaces;

namespace Renee.UI.Components.CGUHandling.CGUModal;

public partial class CGUModal
{
    [Inject] public IFileViewerService FileViewer { get; set; } = null!;
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback OnAccepted { get; set; }
    [Parameter] public string? LabelCGU { get; set; }


    private bool IsChecked = false;

    private async Task Accept()
    {
        if (IsChecked)
        {
            await OnAccepted.InvokeAsync();
        }
    }

    public async Task ViewFileAsync()
    {
        await FileViewer.ViewFileAsync(LabelCGU);
    }
}
