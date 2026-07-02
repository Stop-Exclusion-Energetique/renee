namespace Renee.Application.Interfaces;

public interface ITelemetryService
{
	Task TrackExceptionAsync(Exception ex, CancellationToken? token = null);
	void TrackAiChatMessageSent(string userIdentifier, int messageLength);
}