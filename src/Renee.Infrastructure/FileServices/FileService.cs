using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;
using Renee.Application.Interfaces;
using Renee.Domain;
using Renee.Domain.Documents;
using Renee.Domain.Enums;
using System.Text;

namespace Renee.Infrastructure.FileServices;

public class FileService(
	IConfiguration configuration,
	IEncryptionService encryptionService,
	ITelemetryService telemetryService,
	IUserService userService) : IFileService
{
	private const string MetadataEncodingPrefix = "b64:";

	public async Task<(MemoryStream?, string)> DownloadFileForSynthesisAsync(string blobName)
	{
		try
		{
			var actualBlobName = string.Empty;
			var resolvedName = await ResolveBlobNameByPrefixAsync(blobName);

			if (resolvedName is null)
				return (null, string.Empty);

			actualBlobName = resolvedName;

			var downloadResult = await DownloadAndDecryptBlobAsync(actualBlobName);

			if (downloadResult is null)
				return (null, string.Empty);

			return (downloadResult.FileStream, downloadResult.Name);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return (null, string.Empty);
		}
	}

	public async Task<Document?> DownloadDocumentForAccompanyingFileMenuAsync(
		string blobName,
		AccompanyingFileStage? stage = null)
	{
		try 
		{
			var actualBlobName = blobName;

			if (!blobName.Contains('.') || blobName.Contains(Labels.AnahNotification))
			{
				var resolvedName = await ResolveBlobNameByPrefixAsync(blobName);

				if (resolvedName is null)
					return null;

				actualBlobName = resolvedName;
			}

			return await DownloadAndDecryptBlobAsync(actualBlobName, stage);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
			return null;
		}
	}

	public async Task UploadFileAsync(
		string blobName, 
		string fileName, 
		MemoryStream memoryStream,
		Guid? userId = null)
	{
		try
		{
			memoryStream.Position = 0;
			var outputFileStream = new MemoryStream();
			await encryptionService.EncryptFile(memoryStream, outputFileStream);
			outputFileStream.Position = 0;

			var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
			var containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobStorageAzure:ContainerName"]);

			await containerClient.CreateIfNotExistsAsync();
			var blobClient = containerClient.GetBlobClient(blobName);

			var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			if (userId is not null)
			{
				var userResult = await userService.GetRegisteredUserById((Guid)userId);
				var user = userResult is { IsSuccess: true } ? userResult.Value : null;
				if (user is not null)
					metadata["author"] = EncodeMetadataValue($"{user.FirstName} {user.LastName}");
			}

			await blobClient.UploadAsync(
				outputFileStream,
				new BlobUploadOptions
				{
					Metadata = metadata
				},
				cancellationToken: default);
		}
		catch (Exception ex)
		{
			await telemetryService.TrackExceptionAsync(ex);
		}
	}

	public async Task DeleteFileAsync(string blobName)
	{
		var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
		var blobContainerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobStorageAzure:ContainerName"]);
		var blobClient = blobContainerClient.GetBlobClient(blobName);

		if (await blobClient.ExistsAsync())
		{
			await blobClient.DeleteIfExistsAsync();
		}
	}

	public async Task<List<Document>> GetAllDocumentsAsync(string accompanyingFileReference)
	{
		var required = await GetRequiredDocumentsForMilestonesAsync(accompanyingFileReference);
		var optional = await GetOptionalDocumentsAsync(accompanyingFileReference);

		return [.. required.Concat(optional).OrderBy(d => d.UploadedAt)];
	}

	private async Task<string?> ResolveBlobNameByPrefixAsync(string prefix)
	{
		var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
		var containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobStorageAzure:ContainerName"]);

		await foreach (var blobItem in containerClient.GetBlobsAsync(prefix: prefix))
		{
			return blobItem.Name;
		}

		return null;
	}

	private async Task<Document?> DownloadAndDecryptBlobAsync(string blobName, AccompanyingFileStage? accompanyingFileStage = null)
	{
		var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
		var containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobStorageAzure:ContainerName"]);
		var blobClient = containerClient.GetBlobClient(blobName);

		if (!await blobClient.ExistsAsync())
			return null;

		var properties = (await blobClient.GetPropertiesAsync()).Value;
		var download = (await blobClient.DownloadAsync()).Value;

		var encryptedStream = new MemoryStream();
		await download.Content.CopyToAsync(encryptedStream);
		encryptedStream.Position = 0;

		var decryptedStream = new MemoryStream();
		await encryptionService.DecryptFile(encryptedStream, decryptedStream);
		decryptedStream.Position = 0;

		properties.Metadata.TryGetValue("author", out var author);
		var decodedAuthor = DecodeMetadataValue(author);

		return new Document
		{
			Name = blobClient.Name,
			FileStream = decryptedStream,
			UploadedAt = properties.LastModified.UtcDateTime,
			Data = decryptedStream.ToArray(),
			AccompanyingFileStage = accompanyingFileStage,
			Author = decodedAuthor
		};
	}

	private static string EncodeMetadataValue(string value)
	{
		if (string.IsNullOrEmpty(value))
			return value;

		var bytes = Encoding.UTF8.GetBytes(value);
		var base64 = Convert.ToBase64String(bytes);
		var base64Url = base64.TrimEnd('=').Replace('+', '-').Replace('/', '_');

		return $"{MetadataEncodingPrefix}{base64Url}";
	}

	private static string? DecodeMetadataValue(string? value)
	{
		if (string.IsNullOrEmpty(value))
			return value;

		if (!value.StartsWith(MetadataEncodingPrefix, StringComparison.Ordinal))
			return value;

		var base64Url = value[MetadataEncodingPrefix.Length..];
		var base64 = base64Url.Replace('-', '+').Replace('_', '/');
		var padding = base64.Length % 4;

		if (padding is 2)
			base64 += "==";
		else if (padding is 3)
			base64 += "=";
		else if (padding is 1)
			throw new FormatException("Invalid Base64Url metadata value.");

		var bytes = Convert.FromBase64String(base64);

		return Encoding.UTF8.GetString(bytes);
	}

	private async Task<List<Document>> GetRequiredDocumentsForMilestonesAsync(string accompanyingFileReference)
	{
		var result = new List<Document>();

		foreach (var doc in RequiredMilestonesDocuments.Documents)
		{
			var blobName = $"{accompanyingFileReference}_{doc.Name}";
			var document = await DownloadDocumentForAccompanyingFileMenuAsync(blobName, doc.AccompanyingFileStage);

			result.Add(document ?? new Document
			{
				Name = doc.Name,
				AccompanyingFileStage = doc.AccompanyingFileStage
			});
		}

		return result;
	}

	private async Task<List<Document>> GetOptionalDocumentsAsync(string accompanyingFileReference)
	{
		var blobServiceClient = new BlobServiceClient(configuration.GetConnectionString("AzureBlobStorage"));
		var containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobStorageAzure:ContainerName"]);
		var prefix = $"{accompanyingFileReference}_";
		var result = new List<Document>();

		await foreach (var blobName in containerClient.GetBlobsAsync(prefix: prefix).Select(b => b.Name))
		{
			if (!blobName.Contains("_optionnel", StringComparison.OrdinalIgnoreCase))
				continue;

			var document = await DownloadDocumentForAccompanyingFileMenuAsync(blobName);

			if (document == null)
				continue;

			document.IsOptionnal = true;

			result.Add(document);
		}

		return [.. result.OrderBy(d => d.UploadedAt ?? DateTime.MinValue)];
	}
}
