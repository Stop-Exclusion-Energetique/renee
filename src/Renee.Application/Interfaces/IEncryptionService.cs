namespace Renee.Application.Interfaces;

public interface IEncryptionService
{
	Task DecryptFile(MemoryStream encryptedFileStream, MemoryStream outputFileStream);
	Task EncryptFile(MemoryStream memoryStream, MemoryStream outputFileStream);
}