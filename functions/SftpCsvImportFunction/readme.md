# Azure Function d'import CSV

Cette fonction importe dans Renée des fichiers CSV de dossiers d'accompagnement déposés
sur un stockage Azure Blob (alimenté par un serveur SFTP). Elle réutilise les cas d'usage
de l'application (`ImportAccompanyingFileDataCsv`) et la même base de données.

## Fonctions et planification

Les horaires sont exprimés en [expression cron](https://learn.microsoft.com/azure/azure-functions/functions-bindings-timer#ncrontab-expressions)
à six champs, dans le fuseau du plan d'hébergement.

| Fonction | Horaire | Dossier du conteneur lu |
| --- | --- | --- |
| `ImportCsvFromSftpFunction` | tous les jours à 18 h (`0 0 18 * * *`) | `sftp/in/<jj-MM-aaaa>` |
| `ImportCsvFromSftpUrbanisVersionFunction` | tous les jours à 4 h (`0 0 4 * * *`) | `sftp/urbanis/<jj-MM-aaaa>` |

Pour chaque fichier du jour, la fonction l'importe, journalise un compte rendu (succès,
succès partiel ou échec ligne par ligne), puis le range chiffré sous `sftp/proceed/`.
Les fichiers traités ne sont pas purgés par la fonction : la durée de conservation est à
définir par l'exploitant (voir [`docs/conformite-rgpd.md`](../../docs/conformite-rgpd.md)).

La version « Urbanis » transforme d'abord un fichier au format du partenaire vers le
format d'import de Renée.

## Configuration

Aucune valeur n'est fournie dans le dépôt. Renseignez-les dans les paramètres de
l'application de fonctions ou, en local, dans un fichier `local.settings.json` non versionné.

| Paramètre | Rôle |
| --- | --- |
| `AzureBlobStorageConnectionString` | Compte de stockage contenant les fichiers déposés |
| `ContainerName` | Nom du conteneur |
| Base de données et chiffrement | Mêmes paramètres que l'application principale (voir le `README.md` à la racine) |

## Lancer en local

Avec les outils [Azure Functions Core Tools](https://learn.microsoft.com/azure/azure-functions/functions-run-local) :

```bash
cd functions/SftpCsvImportFunction
func start
```

Utilisez uniquement des fichiers CSV synthétiques en local : aucune donnée réelle de
ménage ne doit quitter l'environnement d'exploitation.
