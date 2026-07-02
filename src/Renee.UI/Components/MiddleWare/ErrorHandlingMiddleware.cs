using Renee.Application.Interfaces;

namespace Renee.UI.Components.MiddleWare;

public class ErrorHandlingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ITelemetryService _telemetryService;

	public ErrorHandlingMiddleware(RequestDelegate next, ITelemetryService telemetryService)
	{
		_next = next;
		_telemetryService = telemetryService;
	}

	public async Task InvokeAsync(HttpContext context)
	{
		try
		{
			await _next(context);

			if (context.Response.StatusCode >= 400 )
			{
				var ex = new Exception("Requête non traitée");
				await _telemetryService.TrackExceptionAsync(ex);

				context.Response.Redirect("/ErrorPage");
			}
		}
		catch (Exception ex)
		{
			await _telemetryService.TrackExceptionAsync(ex);
			context.Response.Redirect("/ErrorPage");
		}
	}
}

public static class ErrorHandlingMiddlewareExtensions
{
	public static IApplicationBuilder UseErrorHandling(this IApplicationBuilder builder)
		=> builder.UseMiddleware<ErrorHandlingMiddleware>();
}
