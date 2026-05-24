# Navigation IA et Agents autonomes — Imane

## Livrables réalisés

- `Assets/Scripts/AI/CrowdAgent.cs`
- `Assets/Scripts/AI/CrowdSpawner.cs`
- `Assets/Prefabs/Imane/CrowdAgent_Prefab.prefab`

## Fonctionnalités réalisées

- Navigation autonome des agents sur NavMesh.
- Génération automatique des agents depuis plusieurs points de départ.
- Déplacement vers plusieurs sorties normales.
- Gestion des états `Normal`, `Dense`, `Panic` et `Evacuation`.
- Méthode publique `TriggerEvacuation()` utilisable lors de l'intégration finale.
- Test local de l'évacuation par la touche `E`.
- Compatibilité avec le nouveau Input System de Unity.
- Compatibilité avec `MLClient` grâce aux méthodes `GetSpeed()` et `SetPanic()`.

## Paramètres du prefab CrowdAgent

| Paramètre | Valeur |
|---|---:|
| Radius | 0.3 |
| Height | 1.8 |
| Base Offset | 0.9 |
| Speed | 1.4 |
| Angular Speed | 240 |
| Acceleration | 8 |
| Stopping Distance | 0.4 |

## Tests réalisés

| Test | Configuration | Résultat | Console |
|---|---|---|---|
| Navigation simple | 1 agent | Déplacement correct vers les sorties | 0 erreur |
| Génération multiple | 10 agents, Spawn Rate = 1 | Déplacement fonctionnel depuis 3 Spawn Points | 0 erreur |
| Évacuation | 10 agents, touche E | Tous les agents dirigés vers `Exit_1` | 0 erreur |
| Performance | 50 agents, Spawn Rate = 2 | Simulation fonctionnelle | 0 erreur |

## Performance observée

Lors du test avec 50 agents, le framerate observé avec plusieurs agents visibles variait approximativement entre 19 et 27 FPS dans l'éditeur Unity. Une capture finale montre une valeur de 20,6 FPS. Un pic de 33,1 FPS a également été observé précédemment.

Ces résultats valident le fonctionnement de la navigation autonome et de l'évacuation. Une optimisation complémentaire pourra être envisagée lors de l'intégration finale, notamment pour améliorer la fluidité avec un grand nombre d'agents.

## Instructions d'intégration pour Assil

1. Utiliser le prefab `Assets/Prefabs/Imane/CrowdAgent_Prefab.prefab`.
2. Créer un objet `Spawner` dans la scène finale.
3. Ajouter le script `CrowdSpawner.cs` à cet objet.
4. Affecter le prefab au champ `Agent Prefab`.
5. Affecter les points de génération au tableau `Spawn Points`.
6. Affecter les sorties normales au tableau `Exit Points`.
7. Affecter une sortie d'urgence au champ `Emergency Exit`.
8. Appeler la méthode publique `TriggerEvacuation()` depuis la logique réseau ou le contrôleur prévu.
9. Effectuer le Bake du NavMesh dans la scène finale avec des paramètres compatibles avec le prefab.

## Remarque

La scène `Test_Imane` a été utilisée uniquement pour les essais locaux. Elle ne doit pas être intégrée à la scène finale du projet.