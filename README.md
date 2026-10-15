# Renée

**Renée** est une application métier d'accompagnement des ménages en situation de
précarité énergétique : suivi des dossiers de rénovation (workflow, gestion
documentaire, import CSV, enrichissement par IA, intégrations externes), avec une
interface web, une couche applicative structurée (CQRS) et des services
d'infrastructure.

Développée dans le cadre du programme **CEE / Territoires Zéro Exclusion Énergétique**
par l'association **Stop Exclusion Énergétique**, Renée est publiée en open source
sous licence **Apache 2.0**.

> ⚠️ **Données personnelles.** Renée traite des données à caractère personnel
> sensibles (identité, adresse, situation sociale et financière de ménages).
> Toute contribution doit respecter le RGPD : **aucune donnée réelle** dans le code,
> les tests ou les fixtures. Voir [`CONTRIBUTING.md`](CONTRIBUTING.md) et
> [`SECURITY.md`](SECURITY.md). Les obligations de l'organisation qui déploie Renée
> sont décrites dans [`docs/conformite-rgpd.md`](docs/conformite-rgpd.md).

## Sommaire

- [Objectif du projet](#objectif-du-projet)
- [Technologies principales](#technologies-principales)
- [Organisation du code](#organisation-du-code)
- [Démarrage rapide](#démarrage-rapide)
- [Configuration & variables d'environnement](#configuration--variables-denvironnement)
- [Base de données (migrations chiffrées)](#base-de-données-migrations-chiffrées)
- [Tests](#tests)
- [CI/CD & qualité](#cicd--qualité)
- [Contribuer](#contribuer)
- [Licence](#licence)

## Objectif du projet

- Centraliser le suivi des dossiers d'accompagnement des ménages.
- Structurer les traitements métier par cas d'usage (commandes / requêtes).
- Intégrer des services externes (Azure, Microsoft Graph, Airtable, services IA…).
- Exposer certaines capacités via un serveur MCP (Model Context Protocol), voir
  [`mcpserver/Renee.McpServer/README.md`](mcpserver/Renee.McpServer/README.md).

## Technologies principales

- **.NET 10**
- **ASP.NET Core / Blazor Server** (UI)
- **Entity Framework Core** (SQL Server)
- **MediatR** (pattern CQRS)
- **Azure Functions** (import CSV planifié via SFTP)
- **MCP** (serveur d'outils)
- **Application Insights** (télémétrie)
- **Qodana** (analyse statique, à lancer en local)

## Organisation du code

```text
src/
  Renee.UI/              -> Interface web (Blazor Server)
  Renee.Application/     -> Cas d'usage (CQRS), handlers, services, DTOs
  Renee.Domain/          -> Entités, enums, contrats de domaine, prompts IA
  Renee.Infrastructure/  -> Accès données (EF Core), providers externes, IA, email
  Renee.MigrationTool/   -> Outil de chiffrement/déchiffrement des migrations
mcpserver/
  Renee.McpServer/       -> Serveur MCP (outils, prompts, ressources)
functions/
  SftpCsvImportFunction/ -> Azure Function d'import CSV planifié
tests/
  CommandHandlerTests/   -> Tests des commandes (xUnit)
  QueryHandlerTests/     -> Tests des requêtes (xUnit)
  UITests/               -> Tests de composants UI
```

Architecture en couches (Clean Architecture) : `Domain` ne dépend de rien ;
`Application` dépend de `Domain` ; `Infrastructure` et `UI` implémentent/consomment.

## Démarrage rapide

Une version étiquetée est téléchargeable dans l'onglet *Releases* du dépôt. Le code est fourni en
l'état, sous licence Apache 2.0.

### Prérequis

- SDK **.NET 10** installé
- **SQL Server** accessible
- Variables de configuration renseignées (voir ci-dessous)
- Variable d'environnement `ENCRYPTION_KEY_RENEE` (pour les migrations)

### Build

```bash
dotnet build Renee.sln
```

### Lancer l'application UI

```bash
dotnet run --project src/Renee.UI
```

### Lancer le serveur MCP

```bash
dotnet run --project mcpserver/Renee.McpServer
```

## Configuration & variables d'environnement

Les fichiers `appsettings.json` du dépôt sont des **gabarits aux valeurs vides** :
ne jamais y écrire de secret. Renseignez les valeurs sensibles via les
**user secrets .NET** (`dotnet user-secrets`) ou des **variables d'environnement**.

Principales clés de configuration attendues (voir `appsettings.json`) :

| Section                | Rôle                                                        |
| ---------------------- | ----------------------------------------------------------- |
| `AzureADB2C`           | Authentification (Instance, TenantId, ClientId/Secret…)     |
| `ConnectionStrings`    | `ReneeDbContext` (SQL Server), `AzureBlobStorage`           |
| `BlobStorageAzure`     | Stockage documentaire + noms des secrets de chiffrement     |
| `SendGrid` / `Brevo`   | Envoi d'emails (clés API)                                   |
| `Airtable`             | Intégration Airtable (BaseId, TableId, jeton bearer)        |
| `AzureFoundryResources`| Services IA (endpoint, clé, déploiement)                    |
| `MicrosoftGraph`       | Accès Microsoft Graph                                       |
| `Jwt` / `JwtGenerationKey` | Génération / validation des jetons                     |
| `ApplicationInsights`  | Télémétrie                                                  |

Variable d'environnement requise hors `appsettings` :

| Variable               | Rôle                                                        |
| ---------------------- | ----------------------------------------------------------- |
| `ENCRYPTION_KEY_RENEE` | Clé AES de (dé)chiffrement des migrations EF Core           |

## Base de données (migrations chiffrées)

⚠️ **Spécificité importante du projet.** Pour protéger l'historique détaillé du
schéma de données, les migrations EF Core sont stockées **chiffrées** (`*.cs.enc`)
dans `src/Renee.Infrastructure/Migrations/`. Elles ne sont **pas lisibles ni
utilisables telles quelles** : il faut les déchiffrer localement avec la clé
`ENCRYPTION_KEY_RENEE` avant toute opération EF Core. Le fichier
`ReneeDbContextModelSnapshot.cs` reste en clair : seul l'historique des migrations
est protégé, pas la structure actuelle du modèle.

Outils fournis dans `src/Renee.Infrastructure/MigrationScripts/` :

```bash
# macOS/Linux
./MigrationScripts/Mac/DecryptMigrations.sh   # déchiffre les migrations
./MigrationScripts/Mac/UpdateDatabase.sh      # déchiffre -> update DB -> re-nettoie
./MigrationScripts/Mac/EncryptMigrations.sh   # rechiffre après ajout d'une migration
```

```powershell
# Windows
./MigrationScripts/Windows/DecryptMigrations.ps1
./MigrationScripts/Windows/UpdateDatabase.ps1
./MigrationScripts/Windows/EncryptMigrations.ps1
```

> Sans la clé `ENCRYPTION_KEY_RENEE`, il n'est pas possible de reconstruire la base
> à partir des migrations. Ce choix est un compromis assumé entre ouverture du code
> et protection du schéma ; il est documenté ici pour éviter toute confusion.

Ce que l'on peut faire **sans la clé** : compiler la solution, lancer les tests
(`dotnet test Renee.sln`, qui n'ont pas besoin de base de données) et relire le code.
Le lancement complet de l'application suppose une base SQL Server déjà créée, fournie
par l'organisation qui exploite Renée. La clé est détenue par les mainteneurs et ne se
demande pas dans un espace public (issue, pull request, commentaire).

## Tests

```bash
dotnet test Renee.sln
```

Suite de tests **xUnit** (commandes, requêtes, composants UI). Les tests n'utilisent
que des **données synthétiques**.

## CI/CD & qualité

- Intégration continue **GitHub Actions** : build et tests à chaque pull request
  (voir [`.github/workflows/controles-build.yml`](.github/workflows/controles-build.yml)),
  et mises à jour hebdomadaires des dépendances par Dependabot.
- Analyse statique **Qodana** configurée via [`qodana.yaml`](qodana.yaml), à lancer en
  local : elle ne fait pas partie de l'intégration continue.

> Note : ce dépôt public ne contient pas les pipelines de déploiement internes
> (Azure DevOps), qui restent gérés hors du périmètre open source.

## Contribuer

Voir [`CONTRIBUTING.md`](CONTRIBUTING.md), [`CODE_OF_CONDUCT.md`](CODE_OF_CONDUCT.md),
[`SECURITY.md`](SECURITY.md) et [`docs/conformite-rgpd.md`](docs/conformite-rgpd.md).

## Licence

Distribué sous licence **Apache 2.0**. Voir [`LICENSE`](LICENSE).
