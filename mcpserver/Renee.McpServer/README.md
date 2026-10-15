# Serveur MCP de Renée

Ce projet expose une partie des données et des actions de Renée à des assistants et
agents compatibles avec le protocole [Model Context Protocol](https://modelcontextprotocol.io/)
(MCP), par un point d'accès HTTP authentifié. Il réutilise le domaine et l'infrastructure
de l'application (`src/Renee.Domain`, `src/Renee.Infrastructure`) et la même base de données.

Le serveur donne accès à des données de ménages. Il ne doit pas être exposé sur Internet
sans mesure de protection adaptée : voir la section « Exploitation » et le document
[`docs/conformite-rgpd.md`](../../docs/conformite-rgpd.md).

## Outils exposés

Tous les outils exigent un jeton valide (`[Authorize]`).

| Outil | Rôle |
| --- | --- |
| `GetAccompanyingFilesHouseholdDataAsJson` | Données du ménage d'un dossier |
| `GetAccompanyingFilesHousingDataAsJson` | Données du logement |
| `GetAccompanyingFilesFinancingPlanDataAsJson` | Plan de financement |
| `GetAccompanyingFilesWorkPlanDataAsJson` | Plan de travaux |
| `GetAccompanyingFileBillingInfo` | Informations de facturation |
| `GetRenovationAidEligibility` | Éligibilité aux aides de rénovation |
| `CreateAccompanyingFile` | Création d'un dossier à partir d'informations minimales |
| `GetAccompanyingFileEnrichmentContext` | Contexte d'un dossier pour son enrichissement |
| `ApplyAccompanyingFileEnrichment` | Application des modifications d'enrichissement |
| `AnalyzeDossierDocumentContent` | Analyse d'un document joint (Azure Document Intelligence) |
| `ResolvePostalCode` | Résolution d'un code postal via une API publique |

Les descriptions de chaque outil, lues par l'assistant, se trouvent dans `Tools/`.

## Configuration

Le fichier `appsettings.json` est un gabarit aux valeurs vides. Les valeurs sensibles se
renseignent par `dotnet user-secrets` ou par variables d'environnement, jamais dans le dépôt.

| Clé | Rôle |
| --- | --- |
| `ConnectionStrings:ReneeDbContext` | Base de données SQL Server de Renée |
| `JwtGenerationKey:SecretKey`, `issuer`, `audience` | Clé (encodée en Base64), émetteur et audience des jetons acceptés |
| `Cors:AllowedOrigins` | Origines autorisées à appeler le serveur depuis un navigateur |
| `RenoHelp:BearerToken` | Jeton du service RenoHelp appelé par le serveur |
| `ApplicationInsights:ConnectionString` | Télémétrie |

Les clés de configuration du stockage documentaire et du chiffrement des pièces jointes
sont celles de l'application principale (voir le `README.md` à la racine).

## Lancer en local

```bash
dotnet run --project mcpserver/Renee.McpServer
```

Le serveur écoute en HTTP et publie le point d'accès MCP à la racine de l'application.
Pour le tester, déclarez-le dans un client MCP compatible comme serveur de type HTTP, avec
un jeton émis par l'application avec la clé `JwtGenerationKey`.

## Exploitation

- Les jetons sont émis par l'application principale et ont une durée de validité limitée.
  La clé de signature est partagée entre l'application et ce serveur : protégez-la comme
  un secret de production et prévoyez sa rotation.
- Limitez l'accès réseau au serveur (réseau privé, pare-feu ou passerelle) et renseignez
  `Cors:AllowedOrigins` avec les seules origines nécessaires.
- Les outils d'écriture (`CreateAccompanyingFile`, `ApplyAccompanyingFileEnrichment`)
  modifient la base. La confirmation de l'utilisateur avant écriture repose sur les
  consignes données à l'assistant dans la description de l'outil, pas sur un contrôle
  technique : prévoyez-la aussi dans le client.
- Un document analysé ou une donnée de dossier peut contenir du texte qui cherche à
  influencer l'assistant. Traitez le contenu renvoyé comme une donnée, jamais comme une
  instruction.
- Ne journalisez pas le contenu des dossiers (voir `CONTRIBUTING.md`).
