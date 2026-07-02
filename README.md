# Renee

Renee est une application metier autour du suivi de dossiers d'accompagnement (workflow, gestion documentaire, import CSV, integrations externes) avec une interface web, une couche applicative structuree et des services d'infrastructure.

## Objectif du projet

- Centraliser le suivi des dossiers d'accompagnement.
- Structurer les traitements metier par cas d'usage (commandes/requetes).
- Integrer des services externes (Azure, Microsoft Graph, Airtable, services IA, etc.).
- Exposer certaines capacites via un serveur MCP.

## Technologies principales

- .NET 10
- ASP.NET Core / Blazor Server (UI)
- Entity Framework Core (SQL Server)
- MediatR (pattern CQRS)
- Azure Functions (traitements planifies)
- MCP (Model Context Protocol) pour le serveur d'outils
- Application Insights (telemetrie)

## Organisation du code

```text
src/
	Renee.UI/              -> Interface web (Blazor Server)
	Renee.Application/     -> Cas d'usage applicatifs, handlers, services
	Renee.Domain/          -> Entites, enums, contrats de domaine
	Renee.Infrastructure/  -> Acces donnees, providers externes, repositories
	Renee.MigrationTool/   -> Outil de migration/utilitaire

mcpserver/
	Renee.McpServer/       -> Serveur MCP (outils, prompts, ressources)

functions/
	SftpCsvImportFunction/ -> Fonction Azure pour import CSV planifie

tests/
	CommandHandlerTests/
	QueryHandlerTests/
	UITests/
```

## Demarrage rapide

### Prerequis

- SDK .NET 10 installe
- SQL Server accessible
- Variables de configuration renseignees (appsettings, secrets utilisateur ou variables d'environnement)

### Build

```bash
dotnet build Renee.sln
```

### Tests

```bash
dotnet test Renee.sln
```

### Lancer l'application UI

```bash
dotnet run --project src/Renee.UI
```

### Lancer le serveur MCP

```bash
dotnet run --project mcpserver/Renee.McpServer
```

## Configuration

- Les fichiers appsettings du depot public doivent rester sanitises (pas de secrets en clair).
- Utiliser de preference les secrets utilisateur et/ou les variables d'environnement pour les valeurs sensibles.

## CI/CD

Le depot contient plusieurs pipelines Azure DevOps (build, PR, functions, MCP server) a la racine.

## Licence

Apache 2.0