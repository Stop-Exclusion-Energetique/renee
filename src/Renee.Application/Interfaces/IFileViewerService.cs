namespace Renee.Application.Interfaces;

public interface IFileViewerService
{
    Task ViewFileAsync(string? fileUrl);
}
