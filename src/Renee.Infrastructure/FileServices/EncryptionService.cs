using Microsoft.Extensions.Configuration;
using Renee.Application.Helpers;
using Renee.Application.Interfaces;

namespace Renee.Infrastructure.FileServices;

public class EncryptionService(IConfiguration configuration) : IEncryptionService
{
	public string EncryptSecretIVName => configuration["BlobStorageAzure:Encrypt_SecretIVName"] ??
		configuration["Encrypt_SecretIVName"] ??
		throw new InvalidOperationException("Encrypt_SecretIVName not found");
	public string EncryptSecretKeyName => configuration["BlobStorageAzure:Encrypt_SecretKeyName"] ??
		configuration["Encrypt_SecretKeyName"] ??
		throw new InvalidOperationException("Encrypt_SecretKeyName not found");
	
	public async Task DecryptFile(MemoryStream encryptedFileStream, MemoryStream outputFileStream)
	{
		var key = Convert.FromBase64String(
			EncryptSecretKeyName);
		var iv = Convert.FromBase64String(
			EncryptSecretIVName);

		await EncryptionHelper.DecryptFile(encryptedFileStream, key, iv, outputFileStream);
	}

	public async Task EncryptFile(MemoryStream memoryStream, MemoryStream outputFileStream)
	{
		var key = Convert.FromBase64String(
			EncryptSecretKeyName);
		var iv = Convert.FromBase64String(
			EncryptSecretIVName);

		await EncryptionHelper.EncryptFile(memoryStream, key, iv, outputFileStream);
	}
}