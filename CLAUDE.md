# Renée (renee-public)

Public open-source (Apache 2.0) .NET 10 app for Stop Exclusion Énergétique: case follow-up for energy-poor households. Blazor Server UI, CQRS (MediatR), EF Core/SQL Server, Azure Function (SFTP CSV import), MCP server. Details: `README.md`, `docs/`.

## Layout
`src/` Renee.UI, Application (use cases), Domain (depends on nothing), Infrastructure, MigrationTool; `mcpserver/Renee.McpServer`; `functions/SftpCsvImportFunction`; `tests/` (xUnit, synthetic data only).

## Run / verify
`dotnet build Renee.sln` · `dotnet test Renee.sln` · `dotnet run --project src/Renee.UI` (launch/tasks in `.vscode/`).

## Hard rules
- Public repo, personal data (RGPD): never real data in code, tests or fixtures; no secret in `appsettings*.json` (empty templates; use user-secrets or env vars).
- EF migrations are stored encrypted (`*.cs.enc`, key `ENCRYPTION_KEY_RENEE`): never read, print or commit decrypted `Migrations/*.cs`; use `src/Renee.Infrastructure/MigrationScripts/`.
- Do not describe private infrastructure in public files.
