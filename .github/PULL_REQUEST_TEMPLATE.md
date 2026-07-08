# Description

<!-- Décrivez clairement le changement et son objectif. Liez l'issue concernée : Closes #___ -->

## Type de changement

- [ ] Correctif (fix)
- [ ] Nouvelle fonctionnalité (feat)
- [ ] Refactorisation / dette technique
- [ ] Documentation
- [ ] Autre :

## Checklist

- [ ] `dotnet build Renee.sln` passe sans erreur
- [ ] `dotnet test Renee.sln` passe (tests ajoutés/mis à jour si nécessaire)
- [ ] `dotnet format` appliqué (respect du `.editorconfig`)
- [ ] **Aucun secret** committé (clé, jeton, mot de passe, chaîne de connexion)
- [ ] **Aucune donnée personnelle réelle** dans le code, les tests ou les fixtures
- [ ] Migrations : si le schéma change, migration chiffrée via `EncryptMigrations`
- [ ] Documentation mise à jour (README / CHANGELOG) si nécessaire

## Notes pour la revue

<!-- Points d'attention, captures d'écran, impacts éventuels. -->
