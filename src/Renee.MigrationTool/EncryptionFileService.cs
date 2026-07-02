using System.Security.Cryptography;
using System.Text;

namespace Renee.MigrationTool;

public static class EncryptionFileService
{
    private static string NoMigrationPathFound(string path) => $"No migrations found at path: {path}";

    private static byte[] GetEncryptionKey()
    {
        var key = Environment.GetEnvironmentVariable("ENCRYPTION_KEY_RENEE") ?? throw new InvalidOperationException("Encryption key not found.");
        var keyBytes = Encoding.UTF8.GetBytes(key);
        return keyBytes.Length is 16 or 24 or 32 ? keyBytes : SHA256.HashData(keyBytes)[..32];
    }

    private static string GetMigrationsPath() => Path.Combine(Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"../../../../Renee.Infrastructure")), "Migrations");

    private static Aes CreateAes(byte[] key)
    {
        var aes = Aes.Create();
        aes.Key = key;
        return aes;
    }

    private static async Task EncryptFile(string inputFile, byte[] key)
    {
        var outputFile = inputFile + ".enc";
        using var aes = CreateAes(key);
        aes.GenerateIV();

        await using var fileStream = new FileStream(outputFile, FileMode.Create);
        await fileStream.WriteAsync(aes.IV.AsMemory(0, aes.IV.Length));
        await using var cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write);
        await using var inputStream = new FileStream(inputFile, FileMode.Open);
        await inputStream.CopyToAsync(cryptoStream);
        
        inputStream.Close();
        cryptoStream.Close();
        fileStream.Close();

        Console.WriteLine($"File {inputFile} successfully encrypted.");
        File.Delete(inputFile);
    }

    private static void DecryptFile(string encFile, byte[] key)
    {
        var decryptedFile = encFile.Replace(".enc", string.Empty);
        using var aes = CreateAes(key);
        using var inputStream = new FileStream(encFile, FileMode.Open);

        var iv = new byte[aes.IV.Length];
        _ = inputStream.Read(iv, 0, iv.Length);
        aes.IV = iv;

        using var fileStream = new FileStream(decryptedFile, FileMode.Create);
        using var cryptoStream = new CryptoStream(inputStream, aes.CreateDecryptor(), CryptoStreamMode.Read);
        cryptoStream.CopyTo(fileStream);
        
        cryptoStream.Close();
        fileStream.Close();
        inputStream.Close();

        Console.WriteLine($"File {decryptedFile} successfully decrypted.");
    }

    public static async Task EncryptAllMigrations()
    {
        var key = GetEncryptionKey();
        var path = GetMigrationsPath();

        if (!Directory.Exists(path))
        {
            Console.WriteLine(NoMigrationPathFound(path));
            return;
        }

        foreach (var file in Directory.GetFiles(path, "*.cs").Where(f => !f.Contains("ModelSnapshot")))
        {
            await EncryptFile(file, key);
        }
    }

    public static void DecryptAllMigrations()
    {
        var key = GetEncryptionKey();
        var path = GetMigrationsPath();

        if (!Directory.Exists(path))
        {
            Console.WriteLine(NoMigrationPathFound(path));
            return;
        }

        foreach (var file in Directory.GetFiles(path, "*.enc"))
        {
            DecryptFile(file, key);
        }
    }

    public static void ClearDecryptedMigrations()
    {
        var path = GetMigrationsPath();

        if (!Directory.Exists(path))
        {
            Console.WriteLine(NoMigrationPathFound(path));
            return;
        }

        foreach (var file in Directory.GetFiles(path, "*.cs").Where(f => !f.Contains("ModelSnapshot")))
        {
            File.Delete(file);
        }
    }

    public static async Task EncryptMigration(string? fileName)
    {
        var key = GetEncryptionKey();
        var path = GetMigrationsPath();

        if (!Directory.Exists(path))
        {
            Console.WriteLine(NoMigrationPathFound(path));
            return;
        }

        var files = new[] { Directory.GetFiles(path, $"*_{fileName}.cs").FirstOrDefault(), Directory.GetFiles(path, $"*_{fileName}.Designer.cs").FirstOrDefault() };

        foreach (var file in files.Where(f => f != null))
        {
            await EncryptFile(file!, key);
        }
    }

    public static void DecryptMigration(string? fileName)
    {
        var key = GetEncryptionKey();
        var path = GetMigrationsPath();

        if (!Directory.Exists(path))
        {
            Console.WriteLine(NoMigrationPathFound(path));
            return;
        }

        var files = new[] { Directory.GetFiles(path, $"*_{fileName}.cs.enc").FirstOrDefault(), Directory.GetFiles(path, $"*_{fileName}.Designer.cs.enc").FirstOrDefault() };

        foreach (var file in files.Where(f => f != null))
        {
            DecryptFile(file!, key);
        }
    }
}