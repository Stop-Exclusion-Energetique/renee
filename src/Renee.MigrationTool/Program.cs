using Renee.MigrationTool;

string? input=null;	
string? scope=null;

if (args.Length != 2)
{
	Console.Write("Please enter a command (encrypt|decrypt|clear): ");
	input = Console.ReadLine();

	Console.Write("Please enter a scope (all|<FileName>): ");
	scope = Console.ReadLine();
}
else
{
	input = args[0];
	scope = args[1];
}

if (string.IsNullOrEmpty(input))
{
	return;
}
switch (input)
{
	case "encrypt":
		await (scope == "all" ? EncryptionFileService.EncryptAllMigrations() : EncryptionFileService.EncryptMigration(scope));
		break;
	case "decrypt":
		await (scope == "all" ? Task.Run(EncryptionFileService.DecryptAllMigrations) : Task.Run(() => EncryptionFileService.DecryptMigration(scope)));
		break;
	case "clear":
		EncryptionFileService.ClearDecryptedMigrations();
		break;
	default:
		Console.WriteLine("Usage: EncryptFile <encrypt|decrypt|clear>");
		break;
}
