namespace Renee.Infrastructure.FileServices;

using Microsoft.JSInterop;
using Renee.Application.Interfaces;

public class FileViewerService : IFileViewerService
{
    private readonly IFileService _fileService;
    private readonly IJSRuntime _jsRuntime;

    public FileViewerService(IFileService fileService, IJSRuntime jsRuntime)
    {
        _fileService = fileService;
        _jsRuntime = jsRuntime;
    }

    public async Task ViewFileAsync(string? fileUrl)
    {
        if (string.IsNullOrWhiteSpace(fileUrl))
            return;

        try
        {
            (var memoryStream, var fileName) = await _fileService.DownloadFileForSynthesisAsync(fileUrl);
            if (memoryStream != null && !string.IsNullOrEmpty(fileName))
            {
                var fileData = memoryStream.ToArray();
                var base64 = Convert.ToBase64String(fileData);
                var mimeType = GetMimeType(fileName);

                await _jsRuntime.InvokeVoidAsync("viewFile", base64, mimeType, fileName);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erreur lors de la lecture du fichier : {ex.Message}");
        }
    }

    private static string GetMimeType(string fileName)
    {
        if (fileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase)) return "application/pdf";
        if (fileName.EndsWith(".jpeg", StringComparison.OrdinalIgnoreCase)) return "image/jpeg";
        if (fileName.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)) return "image/jpeg";
        if (fileName.EndsWith(".png", StringComparison.OrdinalIgnoreCase)) return "image/png";
        if (fileName.EndsWith(".webp", StringComparison.OrdinalIgnoreCase)) return "image/webp";
        return "application/octet-stream";
    }
}
