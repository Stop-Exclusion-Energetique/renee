using Renee.Domain.Enums;

namespace Renee.UI.Components.Features.AccompanyingFiles.Menu.ViewModel;

public class DocumentViewModel
{
	public string Name { get; init; } = string.Empty;
	public AccompanyingFileStage? AccompanyingFileStage { get; init; }
	public MemoryStream? Stream { get; set; }
	public byte[]? Data { get; init; } = [];
	public DateTime? UploadedAt { get; set; }
	public bool IsOptionnal { get; set; }
	public string? Author { get; init; }
}