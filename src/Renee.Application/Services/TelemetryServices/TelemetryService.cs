using System.Diagnostics;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Renee.Application.Interfaces;

namespace Renee.Application.Services.TelemetryServices;

public class TelemetryService(TelemetryClient telemetryClient) : ITelemetryService
{
	public async Task TrackExceptionAsync(Exception ex, CancellationToken? token = null)
	{
		var telemetry = new ExceptionTelemetry(ex) { SeverityLevel = SeverityLevel.Error };

		var frame = new StackTrace(ex, true).GetFrame(0);

		if (frame is not null)
		{
			telemetry.Properties["Method"] = frame.GetMethod()?.Name ?? "Unknown Method";
			telemetry.Properties["LineNumber"] = frame.GetFileLineNumber().ToString();
			telemetry.Properties["FileName"] = frame.GetFileName() ?? "Unknown File";
		}

		telemetry.Properties["ExceptionType"] = ex.GetType().FullName;
		telemetry.Properties["Message"] = ex.Message;

		telemetryClient.TrackException(telemetry);
		await telemetryClient.FlushAsync(token ?? CancellationToken.None);
	}

	public void TrackAiChatMessageSent(string userIdentifier, int messageLength)
	{
		telemetryClient.TrackEvent("AiChatMessageSent", new Dictionary<string, string>
		{
			["UserIdentifier"] = userIdentifier,
			["MessageLength"] = messageLength.ToString()
		});
	}
}