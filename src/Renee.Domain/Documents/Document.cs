using Renee.Domain.Enums;

namespace Renee.Domain.Documents;

public class Document
{
	public string Name { get; set; } = default!;
	public AccompanyingFileStage? AccompanyingFileStage { get; set; }
	public MemoryStream? FileStream { get; set; }
	public DateTime? UploadedAt { get; set; }
	public bool IsOptionnal { get; set; }
	public byte[] Data { get; set; } = [];
	public string? Author { get; set; }
}