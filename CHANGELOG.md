# Journal des modifications (Changelog)

Le format s'inspire de [Keep a Changelog](https://keepachangelog.com/fr/1.1.0/)
et le projet suit le [versionnage sémantique](https://semver.org/lang/fr/).

## [Non publié]

### Ajouté

- `docs/conformite-rgpd.md` : ce que le logiciel fournit, services tiers pouvant recevoir
  des données, obligations de l'organisation qui déploie Renée.
- `README.md` du serveur MCP et de l'Azure Function d'import réécrits, en remplacement des
  textes des gabarits d'origine.
- `.gitignore` : fichiers `.env`, clés et secrets locaux.

### Modifié

- `CONTRIBUTING.md` : règles de prudence complétées (migrations chiffrées, infrastructure
  privée, journaux, captures et PDF, numéros de téléphone fictifs).
- `CODEOWNERS` : comptes réels en remplacement des identifiants provisoires.
- `README.md` : lien vers le workflow de build corrigé, section sur ce qui reste possible
  sans la clé de chiffrement, précision sur la protection des migrations.
- Documentation technique et fonctionnelle : lien cassé et chemin d'image corrigés,
  description des droits d'accès alignée sur le code.
- Journalisation : suppression de traces de débogage et de contenus de dossier dans les
  journaux.

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

[Non publié]: https://github.com/Stop-Exclusion-Energetique/renee/compare/v1.0.1...HEAD
[1.0.1]: https://github.com/Stop-Exclusion-Energetique/renee/releases/tag/v1.0.1
