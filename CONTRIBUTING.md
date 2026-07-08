# Contribuer à Renée

Merci de l'intérêt que vous portez à Renée ! Ce document décrit comment contribuer
au projet dans de bonnes conditions.

Renée est publiée en open source sous licence **Apache 2.0** dans le cadre du
programme CEE porté par l'association **Stop Exclusion Énergétique**.

## Règles d'or (à lire avant toute contribution)

1. **Jamais de secret dans le code.** Pas de chaîne de connexion, clé API, jeton ni
   mot de passe en clair. Utilisez les *user secrets* .NET ou des variables
   d'environnement. Les fichiers `appsettings.*.json` du dépôt doivent rester des
   gabarits aux valeurs vides.
2. **Jamais de donnée personnelle réelle.** Les tests, fixtures et jeux d'exemple
   doivent utiliser uniquement des **données synthétiques** (noms, adresses, emails,
   téléphones inventés ; emails en `@example.com` / `@example.fr`). Toute donnée de
   ménage réelle est interdite.
3. **Respectez l'architecture** (voir plus bas) et le style de code existant.

## Prérequis techniques

- SDK **.NET 10**
- **SQL Server** accessible (local ou conteneur)
- Un IDE : Visual Studio 2022+, JetBrains Rider ou VS Code
- Variable d'environnement `ENCRYPTION_KEY_RENEE` (clé AES) pour manipuler les
  migrations — voir la section « Base de données » du `README.md`

## Mise en route

```bash
git clone <url-du-depot>
cd renee
dotnet restore Renee.sln
dotnet build Renee.sln
dotnet test Renee.sln
```

## Architecture (Clean Architecture)

Respectez le sens des dépendances (le domaine ne dépend de rien) :

```text
Renee.UI ──► Renee.Application ──► Renee.Domain
   │                 │
   └────► Renee.Infrastructure ──► Renee.Domain
```

- **Renee.Domain** : entités, enums, contrats. Aucune dépendance externe.
- **Renee.Application** : cas d'usage (commandes/requêtes CQRS via MediatR), handlers,
  interfaces.
- **Renee.Infrastructure** : implémentations (EF Core, providers externes, IA, email…).
- **Renee.UI** : Blazor Server.

## Workflow de contribution

1. Créez une branche depuis `main` : `feature/<sujet>` ou `fix/<sujet>`.
2. Faites des commits atomiques avec des messages clairs (idéalement
   [Conventional Commits](https://www.conventionalcommits.org/) : `feat:`, `fix:`,
   `docs:`, `test:`, `refactor:`…).
3. Ajoutez ou mettez à jour les **tests** (xUnit) pour tout changement de comportement.
4. Vérifiez que `dotnet build` et `dotnet test` passent, et que l'analyse Qodana ne
   remonte pas de nouveau problème bloquant.
5. Ouvrez une **Pull Request** vers `main` en remplissant le gabarit de PR.
6. Une revue par au moins un mainteneur (voir `CODEOWNERS`) est requise avant fusion.

## Style de code

- Conventions C# standard, `Nullable` activé, *implicit usings* activés.
- Le fichier `.editorconfig` à la racine fait foi pour le formatage
  (`dotnet format` avant de committer).
- Nommage en anglais pour le code ; les libellés métier destinés aux utilisateurs
  restent en français.

## Tests

- Framework : **xUnit**. Les tests vivent dans `tests/`
  (`CommandHandlerTests`, `QueryHandlerTests`, `UITests`).
- Nommez les tests de façon explicite (`Methode_Contexte_ResultatAttendu`).
- N'introduisez jamais de chaîne de connexion réelle ni de donnée personnelle dans
  les tests.

## Signaler un bug / proposer une évolution

- Bug : ouvrez une issue avec le gabarit *Bug report*.
- Faille de sécurité : **n'ouvrez pas d'issue publique**, suivez `SECURITY.md`.
- Évolution : ouvrez une issue avec le gabarit *Feature request*.

## Code de conduite

Toute participation est soumise au `CODE_OF_CONDUCT.md`.
