using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportCsvData.OsloData;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Domain.Enums;
using Renee.Domain;
using Renee.UI.Components.DisplayComponents;
using System.Security.Claims;
using System.Text;

namespace Renee.UI.Components.Features.Admin.ImportAccompanyingFileCsvData;

public partial class ImportAccompanyingFileCsvData
{
    [Inject] public ITelemetryService TelemetryService { get; set; } = null!;
    [Inject] IJSRuntime JsRuntime { get; set; } = null!;

    private EditContext? _importNewAccompanyingFileDataEditContext;
    private EditContext? _transformOsloData;
    private MemoryStream? _memoryStreamFile;
    private MemoryStream? _memoryStreamOsloFile;
    public List<LineErrorReport> LineErrorReports { get; } = [];
    public List<string> SuccessMessages { get; } = [];
    public List<string> ErrorMessages { get; } = [];



    [CascadingParameter(Name = "ZeeSpinner")]
    public ZeeSpinner? ZeeSpinner { get; set; }

    protected override void OnInitialized()
    {
        _importNewAccompanyingFileDataEditContext = new EditContext(new object());
        _transformOsloData = new EditContext(new object());
    }

    public async Task OnValidSubmitCsv()
    {
        LineErrorReports.Clear();
        SuccessMessages.Clear();
        ErrorMessages.Clear();

        ZeeSpinner?.DisplayLoading();
        if (_importNewAccompanyingFileDataEditContext is null || !_importNewAccompanyingFileDataEditContext.Validate())
            return;

        var authState = await AuthenticationStateProvider.GetAuthenticationStateAsync();
        var userIdString = authState.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdString is null || _memoryStreamFile is null) return;

        var result = await AccompanyingFileService.ImportAccompanyingFileDataCsv(_memoryStreamFile, Guid.Parse(userIdString));

        if (result.SuccessMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.Success or ImportCsvResultStatus.PartialSuccess)
        {
            SuccessMessages.AddRange(result.SuccessMessages);
        }

        if (result.ErrorMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.PartialSuccess or ImportCsvResultStatus.Failure)
        {
            LineErrorReports.AddRange(result.ErrorMessages);
            ErrorMessages.AddRange(GetErrorMessagesAsString(result.ErrorMessages));
        }

        _memoryStreamFile = null;
        ZeeSpinner?.HideLoading();
    }

    public async Task OnValidSubmitOsloCsv()
    {
        LineErrorReports.Clear();
        SuccessMessages.Clear();
        ErrorMessages.Clear();

        ZeeSpinner?.DisplayLoading();
        if (_transformOsloData is null || !_transformOsloData.Validate())
            return;

        if (_memoryStreamOsloFile is null)
        {
            ZeeSpinner?.HideLoading();
            return;
        }

        try
        {
            var processor = new CsvProcessor();
            _memoryStreamOsloFile.Position = 0;
            var validRecords = processor.ReadAndTransformCsv(_memoryStreamOsloFile, out var errors);

            LineErrorReports.AddRange(errors);
            ErrorMessages.AddRange(GetErrorMessagesAsString(errors));

            var csvStream = processor.WriteCsvToStream(validRecords);

            csvStream.Position = 0;
            var streamRef = new DotNetStreamReference(stream: csvStream);

            var fileName = $"Renée-Import_{DateTime.Now:yyyy-MM-dd}.csv";
            await JsRuntime.InvokeVoidAsync("downloadCsvFile", fileName, streamRef);
            if (validRecords.Count > 0)
            {
                SuccessMessages.Add(CsvDataLabel.SuccessMessage);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        _memoryStreamOsloFile = null;
        ZeeSpinner?.HideLoading();
    }

    public async Task HandleCsvFile(InputFileChangeEventArgs e, bool isOsloFile = false)
    {
        var file = e.File;

        if (file is null)
            return;

        try
        {
            LineErrorReports.Clear();
            SuccessMessages.Clear();

            var stream = new MemoryStream();
            await file.OpenReadStream(long.MaxValue).CopyToAsync(stream);
            stream.Position = 0;

            if (isOsloFile)
                _memoryStreamOsloFile = stream;
            else
                _memoryStreamFile = stream;
        }
        catch (Exception ex)
        {
            await TelemetryService.TrackExceptionAsync(ex);
            throw;
        }
    }

    private async Task DownloadErrorsFile()
    {
        var errorStream = WriteErrorsToTextFile(ErrorMessages);
        var streamRef = new DotNetStreamReference(stream: errorStream);

        var fileName = $"Erreurs_csv_{DateTime.Now:yyyy-MM-dd_HH-mm}.txt";
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
    }


    public static MemoryStream WriteErrorsToTextFile(List<string> errors)
    {
        var stream = new MemoryStream();
        using (var writer = new StreamWriter(stream, new UTF8Encoding(encoderShouldEmitUTF8Identifier: true), leaveOpen: true))
        {
            foreach (var error in errors)
            {
                writer.WriteLine(error);
            }
        }

        stream.Position = 0; 
        return stream;
    }

    public static List<string> GetErrorMessagesAsString(List<LineErrorReport> lineErrorReports)
    {
        var errorsList = new List<string>();

        foreach (var report in lineErrorReports)
        {
            foreach (var error in report.GetErrors())
            {
                errorsList.Add(FormatErrorMessage(error, report.LineNumber, report.LineId));
            }
        }

        return errorsList;
    }

    public static string FormatErrorMessage(string message, int? lineNumber, int? lineId)
    {
        if (lineNumber is null)
            return message;

        return !string.IsNullOrWhiteSpace(lineId.ToString()) 
            ? string.Format(CsvDataLabel.Errors.ErrorOnLine, lineNumber, lineId, message) 
            : string.Format(CsvDataLabel.Errors.ErrorOnLineWithoutId, lineNumber, message);
    }
}