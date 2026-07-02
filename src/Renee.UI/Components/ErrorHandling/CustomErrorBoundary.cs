using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;
using Renee.Application.Interfaces;
using System.Runtime.CompilerServices;

namespace Renee.UI.Components.ErrorHandling;

public class CustomErrorBoundary : ErrorBoundary
{
	[Inject]
	public ITelemetryService TelemetryService { get; set; } = default!;

	protected override async Task OnErrorAsync(Exception exception)
	{
		await TelemetryService.TrackExceptionAsync(exception);
	}
}
