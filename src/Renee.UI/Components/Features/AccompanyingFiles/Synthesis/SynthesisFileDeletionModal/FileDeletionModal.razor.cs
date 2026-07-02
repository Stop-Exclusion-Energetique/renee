using Azure;
using Blazored.Modal;
using Blazored.Modal.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Renee.Application.Interfaces;
using Renee.Domain;

namespace Renee.UI.Components.Features.AccompanyingFiles.Synthesis.SynthesisFileDeletionModal;

public partial class FileDeletionModal
{
	[Inject] public IFileService FileService { get; set; } = null!;
	[Inject] public ITelemetryService telemetryService { get; set; } = null!;

	[CascadingParameter] public BlazoredModalInstance BlazoredModal { get; set; } = default!;
	[Parameter] public string? FileName { get; set; }

	public async Task DeleteFile()
	{
		try
		{
			if (!string.IsNullOrEmpty(FileName))
			{
				await FileService.DeleteFileAsync(FileName);
				await BlazoredModal.CloseAsync(ModalResult.Ok());

				NotificationService.Notify(new NotificationMessage
				{
					Severity = NotificationSeverity.Success,
					Summary = "",
					Detail = Labels.FileDeletedSuccessfully,
					Duration = 4000
				});

				return;
			}

			NotificationService.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Error,
				Summary = Labels.Errors.FileErrorNotificationSummary,
				Detail = Labels.Errors.FileErrorNotificationDetails,
				Duration = 4000
			});

			await BlazoredModal.CancelAsync();
		}
		catch (Exception ex) {

			NotificationService.Notify(new NotificationMessage
			{
				Severity = NotificationSeverity.Error,
				Summary = Labels.Errors.FileErrorNotificationSummary,
				Detail = Labels.Errors.FileErrorNotificationDetails,
				Duration = 4000
			});
			await BlazoredModal.CancelAsync();

			await telemetryService.TrackExceptionAsync(ex);
		}
	}
}
