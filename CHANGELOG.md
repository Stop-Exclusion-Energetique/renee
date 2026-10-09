# Journal des modifications (Changelog)

Le format s'inspire de [Keep a Changelog](https://keepachangelog.com/fr/1.1.0/)
et le projet suit le [versionnage sémantique](https://semver.org/lang/fr/).

## [Non publié]

## [1.0.1] - 2026-10-09

Première version étiquetée, téléchargeable depuis les Releases GitHub. La version 1.0.0
correspond à la première publication du code, sans étiquette.

### Ajouté

- Documentation fonctionnelle (rôles, parcours, tableaux de bord, import) et documentation
  technique (architecture, modèle de données, configuration, déploiement) dans `docs/`.
- Fichiers de gouvernance d'un projet ouvert : CONTRIBUTING, CODE_OF_CONDUCT, SECURITY,
  CODEOWNERS, modèles d'issues et de demandes de fusion.
- Intégration continue : build et tests à chaque demande de fusion, mises à jour
  hebdomadaires des dépendances.

### Sécurité

- Mise à jour de `System.Security.Cryptography.Xml` en 10.0.12.

### Modifié

- Jeux d'essai : adresses de messagerie sur les domaines réservés example.com et
  example.org, numéros de téléphone sur la plage fictive de l'ARCEP. Aucune donnée réelle.
- `SECURITY.md` : le signalement se fait uniquement par GitHub Security Advisories.

## [1.0.0] - 2026-07-02

### Ajouté

- Première publication open source du code de Renée (licence Apache 2.0), livrable
  dans le cadre du programme CEE / DGEC.
- Application d'accompagnement des ménages : suivi de dossiers, gestion documentaire,
  import CSV, enrichissement IA, serveur MCP, Azure Function d'import SFTP.

> ℹ️ Renseignez les versions suivantes au fil des évolutions. Chaque entrée doit
> préciser : Ajouté / Modifié / Corrigé / Déprécié / Supprimé / Sécurité.
