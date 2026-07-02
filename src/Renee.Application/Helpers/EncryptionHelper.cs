using System.Security.Cryptography;

namespace Renee.Application.Helpers;

public static class EncryptionHelper
{
	public static async Task DecryptFile(MemoryStream stream, byte[] key, byte[] iv, MemoryStream outputFileStream)
	{
		stream.Position = 0;
		using var aes = Aes.Create();
		aes.Key = key;
		aes.IV = iv;
		aes.Padding = PaddingMode.PKCS7;

		await using var cryptoStream = new CryptoStream(stream, aes.CreateDecryptor(), CryptoStreamMode.Read);
		await cryptoStream.CopyToAsync(outputFileStream);
		outputFileStream.Position = 0;
	}

	public static async Task EncryptFile(
		MemoryStream memoryStream,
		byte[] key,
		byte[] iv,
		MemoryStream outputFileStream)
	{
		var outputStream = new MemoryStream();
		using var aes = Aes.Create();
		aes.Key = key;
		aes.IV = iv;
		aes.Padding = PaddingMode.PKCS7;

		memoryStream.Position = 0;

		await using var cryptoStream = new CryptoStream(outputStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
		await memoryStream.CopyToAsync(cryptoStream);
		await cryptoStream.FlushFinalBlockAsync();
		outputStream.Position = 0;
		await outputFileStream.WriteAsync(outputStream.ToArray());
		outputFileStream.Position = 0;
	}
}