using System.Globalization;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Renee.Application.Interfaces;
using Renee.Application.Services.Administration.ImportCsvData;
using Renee.Application.Services.Administration.ImportCsvData.OsloData;
using Renee.Domain;
using Renee.Domain.Enums;

namespace SftpCsvImportFunction;

public class ImportCsvFromSftpFunction
{
    private const string _blobConnectionString = "AzureBlobStorageConnectionString";
    private const string _containerName = "ContainerName";

    public List<LineErrorReport> LineErrorReports { get; } = [];
    public List<string> SuccessMessages { get; } = [];
    public List<string> Errors { get; } = [];
    public BlobServiceClient BlobServiceClient { get; set; }
    public BlobContainerClient ContainerClient { get; set; }

    private readonly ILogger _logger;
    public IAccompanyingFileService _accompanyingFileService { get; set; }
    public IEncryptionService _encryptionService { get; set; }
    
    public ImportCsvFromSftpFunction(ILoggerFactory loggerFactory, IAccompanyingFileService accompanyingFileService, IConfiguration Configuration, IEncryptionService encryptionService)
    {
        _accompanyingFileService = accompanyingFileService ?? throw new ArgumentNullException(nameof(accompanyingFileService));
        _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));

        _logger = loggerFactory.CreateLogger<ImportCsvFromSftpFunction>();

        BlobServiceClient = new BlobServiceClient(Configuration[_blobConnectionString]);
        ContainerClient = BlobServiceClient.GetBlobContainerClient(Configuration[_containerName]);
    }

    [Function("ImportCsvFromSftpFunction")]
    public async Task Run([TimerTrigger("0 0 18 * * *")] TimerInfo myTimer)
    {
        try
        {
            var blobNamePrefix = $"sftp/in/{DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}";

            await ContainerClient.CreateIfNotExistsAsync();

            await foreach (BlobItem blobItem in ContainerClient.GetBlobsAsync(prefix: blobNamePrefix))
            {
                try
                {
                    var blobClient = ContainerClient.GetBlobClient(blobItem.Name);
                    var fileName = blobItem.Name.Split('/')[^1];

                    BlobDownloadInfo download = await blobClient.DownloadAsync();

                    await using var memoryStream = new MemoryStream();

                    await download.Content.CopyToAsync(memoryStream);

                    memoryStream.Position = 0;
                    await FileTreatment(memoryStream, fileName);

                    await using var uploadStream = new MemoryStream(memoryStream.ToArray());
                    uploadStream.Position = 0;
                    var uploadBlobName = $"sftp/proceed/{DateTime.UtcNow}/{fileName}";
                    await MoveFileDirectoryOnServerAsync(blobItem.Name, uploadBlobName, uploadStream);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing blob item {BlobItemName}: {ExceptionMessage}", blobItem.Name, ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ImportCsvFromSftpFunction failed with exception: {ExceptionMessage}", ex.Message);
        }
    }

    [Function("ImportCsvFromSftpUrbanisVersionFunction")]
    public async Task RunUrbanisVersion([TimerTrigger("0 0 4 * * *")] TimerInfo myTimer)
    {
        var blobNamePrefix = $"sftp/urbanis/{DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture)}";

        await ContainerClient.CreateIfNotExistsAsync();

        await foreach (BlobItem blobItem in ContainerClient.GetBlobsAsync(prefix: blobNamePrefix))
        {
            try
            {
                var blobClient = ContainerClient.GetBlobClient(blobItem.Name);
                var fileName = blobItem.Name.Split('/')[^1];

                BlobDownloadInfo download = await blobClient.DownloadAsync();
                await using var memoryStream = new MemoryStream();

                await download.Content.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                await UrbanisFileTreatment(memoryStream, fileName);
                
                await using var uploadStream = new MemoryStream(memoryStream.ToArray());
                uploadStream.Position = 0;
                var uploadBlobName = $"sftp/proceed/{DateTime.UtcNow}/{fileName}";
                await MoveFileDirectoryOnServerAsync(blobItem.Name, uploadBlobName, uploadStream);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing blob item {BlobItemName}: {ExceptionMessage}", blobItem.Name, ex.Message);
            }
        }
    }

    private async Task FileTreatment(MemoryStream fileStream, string fileName)
    {
        var result = await _accompanyingFileService.ImportAccompanyingFileDataCsv(fileStream, new Guid("f3c2561c-b788-46d9-b97b-c49c4063bb9f"));

        if (result.SuccessMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.Success or ImportCsvResultStatus.PartialSuccess)
        {
            LogMessages(LogLevel.Warning, $"Log for File {fileName}");

            SuccessMessages.AddRange(result.SuccessMessages);
            var logMessage = string.Join(", ", SuccessMessages);
            LogMessages(LogLevel.Information, logMessage);

            SuccessMessages.Clear();
        }

        if (result.ErrorMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.PartialSuccess or ImportCsvResultStatus.Failure)
        {

            LineErrorReports.AddRange(result.ErrorMessages);

            Errors.AddRange(GetErrorMessagesAsString(LineErrorReports));

            LogMessages(LogLevel.Warning, $"Log for File {fileName}");
            var logMessage = string.Join(", ", Errors);
            LogMessages(LogLevel.Error, logMessage);

            Errors.Clear();
            LineErrorReports.Clear();
        }
    }

    private async Task UrbanisFileTreatment(MemoryStream fileStream, string fileName)
    {
        var processedStream = ValidateAndTransformCsvDataAsync(fileStream);
        processedStream.Position = 0;

        var result = await _accompanyingFileService.ImportAccompanyingFileDataCsv(processedStream, new Guid("f3c2561c-b788-46d9-b97b-c49c4063bb9f"));

        if (result.SuccessMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.Success or ImportCsvResultStatus.PartialSuccess)
        {
            LogMessages(LogLevel.Warning, $"Log for File {fileName}");

            SuccessMessages.AddRange(result.SuccessMessages);
            var logMessage = string.Join(", ", SuccessMessages);
            LogMessages(LogLevel.Information, logMessage);

            SuccessMessages.Clear();
        }

        if (result.ErrorMessages is not null &&
            result.ImportCsvResultStatus is ImportCsvResultStatus.PartialSuccess or ImportCsvResultStatus.Failure)
        {

            LineErrorReports.AddRange(result.ErrorMessages);

            Errors.AddRange(GetErrorMessagesAsString(LineErrorReports));

            LogMessages(LogLevel.Warning, $"Log for File {fileName}");
            var logMessage = string.Join(", ", Errors);
            LogMessages(LogLevel.Error, logMessage);

            Errors.Clear();
            LineErrorReports.Clear();
        }
    }
    
    private MemoryStream ValidateAndTransformCsvDataAsync(MemoryStream fileStream)
    {
        try
        {
            var processor = new CsvProcessor();
            fileStream.Position = 0;
            var validRecords = processor.ReadAndTransformCsv(fileStream, out var errors);

            LineErrorReports.AddRange(errors);
            Errors.AddRange(GetErrorMessagesAsString(errors));

            var csvStream = processor.WriteCsvToStream(validRecords);

            csvStream.Position = 0;
            return csvStream;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during CSV validation and transformation in ValidateAndTransformCsvDataAsync: {ExceptionMessage}", ex.Message);
            throw new InvalidOperationException("Exception occurred in ValidateAndTransformCsvDataAsync.", ex);
        }
    }

    private async Task MoveFileDirectoryOnServerAsync(string blobSourceName, string blobDestinationName, MemoryStream stream)
    {
        await UploadFileAsync(blobDestinationName, stream);
        await DeleteFileAsync(blobSourceName);
    }

    private async Task UploadFileAsync(string blobName, MemoryStream memoryStream)
    {
        using MemoryStream outputFileStream = new();
        await _encryptionService.EncryptFile(memoryStream, outputFileStream);

        await ContainerClient.CreateIfNotExistsAsync();
        var blobClient = ContainerClient.GetBlobClient(blobName);
        await blobClient.UploadAsync(outputFileStream, true);
    }

    private async Task DeleteFileAsync(string blobName)
	{
		var blobClient = ContainerClient.GetBlobClient(blobName);
        
        await blobClient.DeleteIfExistsAsync();
	}
    
    private void LogMessages(LogLevel level, string messages)
        => _logger.Log(level, messages);

	private static string FormatErrorMessage(string message, int? lineNumber, string? lineId)
	{
		if (lineNumber is null)
			return message;

		return !string.IsNullOrWhiteSpace(lineId)
			? string.Format(CsvDataLabel.Errors.ErrorOnLine, lineNumber, lineId, message)
			: string.Format(CsvDataLabel.Errors.ErrorOnLineWithoutId, lineNumber, message);
	}

	private static List<string> GetErrorMessagesAsString(List<LineErrorReport> lineErrorReports)
    {
        var errorsList = new List<string>();

        foreach (var report in lineErrorReports)
        {
            foreach (var error in report.GetErrors())
            {
                errorsList.Add(FormatErrorMessage(error, report.LineNumber, report.LineId?.ToString()));
            }
        }

        return errorsList;
    }
}
