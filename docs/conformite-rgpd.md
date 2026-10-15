# Protection des données personnelles (RGPD)

Ce document s'adresse aux organisations qui déploient Renée, à leurs délégués à la
protection des données et aux personnes qui auditent le code. Il décrit ce que le dépôt
contient, ce que le logiciel prévoit et ce qu'il laisse à la charge de l'exploitant. Il
décrit l'état du code à la version 1.0.1 et ne constitue pas un avis juridique : il est à
relire avec le délégué à la protection des données de chaque organisation.

## Portée du dépôt

Le dépôt ne contient que du code, de la documentation et des jeux d'essai synthétiques.
Il ne contient aucune donnée de ménage réelle ni aucun secret : les fichiers
`appsettings*.json` sont des gabarits vides et les migrations de base de données sont
chiffrées (voir le `README.md`). Les règles imposées aux contributeurs figurent dans
[`CONTRIBUTING.md`](../CONTRIBUTING.md).

Les données personnelles n'existent que dans les déploiements. L'organisation qui déploie
Renée décide des finalités et des moyens du traitement des dossiers qu'elle y enregistre :
elle en est en principe responsable de traitement. Le dépôt public n'est pas un traitement.

Les documents juridiques du dossier `src/Renee.UI/wwwroot/legal` (mentions légales,
conditions générales d'utilisation, politique de confidentialité) sont ceux de
l'association Stop Exclusion Énergétique pour son propre déploiement. Une autre
organisation doit les remplacer par les siens. Même pour l'association, la politique de
confidentialité actuelle s'adresse aux utilisateurs professionnels de la plateforme : elle
ne remplace pas l'information des ménages prévue aux articles 13 et 14 du RGPD.

## Données traitées par l'application

Selon les écrans utilisés, Renée enregistre pour chaque dossier : l'identité et les
coordonnées des occupants, l'adresse et les caractéristiques du logement, la composition
du foyer, le revenu fiscal de référence, la situation sociale et financière, le plan de
financement et le plan de travaux, des indicateurs de santé et d'autonomie (handicap,
maladie de longue durée, perte d'autonomie), des textes libres de contexte social et de
projet familial, ainsi que des documents joints. Elle enregistre aussi des données sur les
utilisateurs professionnels (identité, adresse électronique, rôle, territoire).

Les indicateurs de santé relèvent des catégories particulières de données de l'article 9.
Avec la situation sociale des ménages et le recours à des services d'intelligence
artificielle, ils justifient en général une analyse d'impact relative à la protection des
données avant la mise en production.

## Ce que prévoit le logiciel

- Authentification des utilisateurs par un fournisseur d'identité externe (configuration
  `AzureADB2C`) et rôles applicatifs. L'accès aux dossiers dépend du rôle, du rattachement
  à l'équipe du dossier et du territoire. Les rôles d'administration et d'association
  disposent d'une vue d'ensemble de la liste des dossiers.
- Suppression logique des dossiers et des comptes (indicateur `IsDeleted`).
- Chiffrement des documents joints avant leur dépôt dans le stockage Azure Blob. Le
  chiffrement de la base de données au repos relève de la configuration de l'hébergement.
- Fonction d'usurpation d'identité réservée à l'administration, avec une table qui
  conserve le début et la fin de chaque usurpation.
- Serveur MCP protégé par un jeton de durée limitée (voir
  [`mcpserver/Renee.McpServer/README.md`](../mcpserver/Renee.McpServer/README.md)).
- Configuration sans secret dans le dépôt et migrations chiffrées.

## Services tiers pouvant recevoir des données

Selon la configuration, l'application appelle les services suivants. Pour chacun,
l'exploitant doit disposer d'un contrat de sous-traitance, connaître la localisation du
traitement, la durée de conservation chez le fournisseur et les éventuels transferts hors
de l'Union européenne.

| Service | Données transmises |
| --- | --- |
| Service d'IA générative (`AzureFoundryResources`) | Pour la synthèse d'un dossier : revenu fiscal de référence, indicateurs de santé et d'autonomie, textes libres de contexte social et de projet familial. Ni le nom, ni l'adresse, ni les coordonnées. Dans l'assistant de conversation, les résultats des outils MCP consultés. |
| Azure Document Intelligence | Documents joints analysés à la demande de l'utilisateur |
| Reconnaissance vocale | Enregistrements audio dictés par l'utilisateur |
| Airtable | Export déclenché par l'utilisateur : nom du foyer, code postal, ville, revenu fiscal de référence, endettement, situation sociale, types de ressources et caractéristiques du logement |
| Brevo ou SendGrid | Courriels de notification, qui peuvent contenir le nom du ménage |
| Microsoft Graph | Gestion des comptes des utilisateurs professionnels |
| Application Insights | Télémétrie et journaux applicatifs, dont l'identifiant professionnel de l'utilisateur |
| API de référentiels (adresses, INSEE) et RenoHelp | Données d'adresse ou de référence nécessaires aux vérifications d'éligibilité |

Les champs de texte libre envoyés à l'IA ne sont pas filtrés. Les utilisateurs doivent être
informés qu'ils ne doivent pas y saisir d'identité, de coordonnées ni d'information de
santé détaillée.

## Ce que l'exploitant doit établir

Ces éléments ne peuvent pas figurer dans un dépôt public, car ils dépendent de chaque
déploiement.

1. Le registre des traitements, les finalités et les bases légales, et l'analyse d'impact.
2. L'information des ménages concernés, et la procédure d'exercice de leurs droits (accès,
   rectification, effacement, opposition, limitation).
3. Les contrats de sous-traitance et le suivi des transferts hors de l'Union européenne
   pour chacun des services de la section précédente.
4. Les durées de conservation et une procédure de purge ou d'anonymisation. Le logiciel ne
   fait que de la suppression logique : les données d'un dossier supprimé, ses pièces
   jointes et les fichiers CSV importés (rangés sous `sftp/proceed/`) restent stockés
   jusqu'à une action de l'exploitant.
5. La conservation et la durée de rétention des journaux, le niveau de journalisation en
   production et la configuration d'Application Insights.
6. La sécurité de l'hébergement : chiffrement de la base au repos, sauvegardes, gestion et
   rotation des clés (clé de chiffrement des documents, clé de signature des jetons), accès
   réseau restreint au serveur MCP.
7. La revue périodique des comptes et des rôles, et le contrôle de l'usage de la fonction
   d'usurpation d'identité.
8. La procédure de gestion des violations de données, avec notification à l'autorité de
   contrôle dans les 72 heures lorsqu'elle est requise.
9. La cohérence des documents légaux publiés avec les cookies réellement déposés par
   l'application.

## Signaler un problème

Une faille de sécurité ou une fuite de donnée se signale de façon confidentielle, selon
[`SECURITY.md`](../SECURITY.md), jamais par une issue publique.
