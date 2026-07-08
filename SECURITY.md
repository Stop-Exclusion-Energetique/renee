# Politique de sécurité

## Contexte

Renée est une application qui traite des **données à caractère personnel de ménages
en situation de précarité énergétique** (identité, coordonnées, adresse, situation
sociale et financière). La sécurité et la protection de ces données sont une priorité
absolue. Merci de contribuer à la sécurité du projet en signalant les vulnérabilités
de manière responsable.

## Signaler une vulnérabilité

**Ne créez pas d'issue publique GitHub pour une faille de sécurité.**

Merci de signaler toute vulnérabilité de façon confidentielle :

- Via la fonction **GitHub Security Advisories** du dépôt
  (onglet *Security* → *Report a vulnerability*), ou
- Par courriel à **securite@stopexclusionenergetique.org** *(à créer / adresse à confirmer)*.

Merci d'inclure, dans la mesure du possible :

- une description de la vulnérabilité et de son impact potentiel ;
- les étapes de reproduction (proof of concept) ;
- les versions / composants concernés ;
- toute suggestion de correctif.

## Engagement de réponse

| Étape                              | Délai indicatif       |
| ---------------------------------- | --------------------- |
| Accusé de réception                | sous 3 jours ouvrés   |
| Première évaluation de criticité   | sous 10 jours ouvrés  |
| Correctif ou plan de remédiation   | selon la criticité    |

Nous nous engageons à ne pas engager de poursuites à l'encontre des personnes qui
signalent des vulnérabilités de bonne foi, dans le respect de cette politique
(pas d'exfiltration de données, pas de dégradation de service, pas d'accès à des
données de tiers).

## Périmètre

Sont concernés : le code de ce dépôt (UI Blazor, couche applicative, infrastructure,
serveur MCP, Azure Function d'import). Les services hébergés (production) et les
dépendances tierces relèvent de leurs propres politiques.

## Bonnes pratiques attendues des contributeurs

- **Aucun secret** (chaîne de connexion, clé API, jeton, mot de passe) ne doit être
  committé. Utiliser les *user secrets* .NET ou des variables d'environnement.
- **Aucune donnée personnelle réelle** ne doit figurer dans le code, les tests, les
  fixtures ou les jeux de données d'exemple. Utiliser exclusivement des données
  synthétiques (voir `CONTRIBUTING.md`).
- Les migrations de base de données sont **chiffrées** dans le dépôt : ne jamais
  committer de migration en clair ni la clé `ENCRYPTION_KEY_RENEE`.
- Signaler immédiatement toute fuite accidentelle de secret ou de donnée personnelle
  afin de déclencher la procédure de rotation / purge d'historique.
