# CrowdFlow VR
### Gestion de Foule Intelligente en Réalité Virtuelle

**Projet Final S8 — ISIBD | Unity 6.4 | Meta Quest 2 | Python 3.11**

---

## Description

CrowdFlow VR est une simulation immersive de gestion de foule dans une gare ferroviaire. Des agents virtuels pilotés par une IA naviguent dans la scène. Un utilisateur en VR (Meta Quest 2) interagit avec la foule en temps réel, tandis qu'un utilisateur Desktop a une vue macro de la situation.

Le système détecte automatiquement les anomalies de comportement de foule grâce à un modèle Machine Learning entraîné sur de vraies données de piétons.

---

## Fonctionnalités

- **Agents IA autonomes** — Navigation NavMesh avec états Normal / Dense / Panique / Évacuation
- **Détection ML temps réel** — IsolationForest entraîné sur 786 piétons réels (ETH/UCY Dataset, ICCV 2007)
- **Communication UDP** — Serveur Python analyse les flux toutes les 200ms
- **Mode VR** — Meta Quest 2 + OpenXR, placement de barrières, overlay de danger
- **Mode PC sans casque** — Caméra isométrique top-down, fonctionne sans casque
- **Dashboard live** — Heatmap de densité, alertes, graphes en temps réel (Streamlit)
- **Multiplayer asymétrique** — VR + Desktop connectés en réseau local

---

## Équipe

| Membre | Rôle | Branche |
|--------|------|---------|
| Maroua | IA & ML + Organisation dépôt Git + Design Scène 3D | `feature/ml-engine` |
| Imane | NavMesh IA & Agents autonomes | `feature/navmesh-ai` |
| Houda | Big Data & Dashboard analytique | `feature/data-pipeline` |
| Meryem | Réalité Virtuelle & Interaction | `feature/vr-ux` |
| Hasnaa | Comportements & Dynamique de foule | `feature/panic-behavior` |
| Assil | Réseau & Intégration finale | `feature/networking` |

---

## Structure du projet

```
crowdflowvr/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity        ← Scène principale
│   ├── Scripts/
│   │   ├── ML/                      ← Maroua
│   │   ├── AI/                      ← Imane
│   │   ├── Data/                    ← Houda
│   │   ├── VR/                      ← Meryem
│   │   ├── Behavior/                ← Hasnaa
│   │   └── Network/                 ← Assil
│   └── Prefabs/
│       └── Maroua/                  ← Mobilier de gare (30+ prefabs)
├── Python/
│   ├── ml_engine/                   ← Maroua
│   │   ├── infer_server.py
│   │   ├── 02_train_model.py
│   │   ├── model.pkl
│   │   └── requirements.txt
│   └── dashboard/                   ← Houda
│       └── app.py
└── Docs/
    ├── ML_metrics.md
    ├── Performance_report.md
    └── Network_guide.md
```

---

## Intelligence Artificielle & Big Data

### Modèle ML — Maroua
- **Dataset** : ETH/UCY (Lerner et al. ICCV 2007 + Pellegrini et al. ICCV 2009)
- **Données** : 4534 frames réelles, 360 piétons filmés en ville
- **Algorithme** : IsolationForest — détection d'anomalies non supervisée
- **Features** : nb_agents, vitesse_moyenne, ecart_type, percentile_90
- **Résultat** : 227 anomalies détectées (5%) — paniques, goulots, surcharges

### Comportements de foule — Hasnaa
- **Social Force Model** (Helbing & Molnár, 1995) — référence scientifique internationale
- **Boids** — séparation, alignement, cohésion
- **Propagation de panique** par contagion comportementale

### Big Data Pipeline — Houda
- Logging CSV temps réel depuis Unity toutes les 500ms
- Dashboard Streamlit avec heatmap de densité, alertes, graphes historiques

---

## Organisation du dépôt Git — Maroua

La configuration complète du dépôt a été réalisée par Maroua avant le démarrage du projet :

- Initialisation Git et connexion à GitHub
- Création du `.gitignore` Unity (exclusion Library/, Temp/, Logs/, fichiers Visual Studio)
- Création de la structure de dossiers pour les 6 membres (Assets/Scripts/ML, AI, Data, VR, Behavior, Network)
- Création des 8 branches : master, develop + une branche feature/ par membre
- Résolution des conflits Git complexes
- Push de la scène Unity principale pour toute l'équipe

---

## Installation et lancement

### Prérequis
- Unity 6.4 (2022 LTS)
- Python 3.11+
- Meta Quest 2 (optionnel — mode PC disponible)

### 1. Cloner le dépôt
```bash
git clone https://github.com/maryemmouslim/crowdflowvr.git
cd crowdflowvr
```

### 2. Installer les dépendances Python
```bash
cd Python/ml_engine
pip install -r requirements.txt
```

### 3. Lancer le serveur ML (obligatoire avant Unity)
```bash
python infer_server.py
```

### 4. Lancer le dashboard (optionnel)
```bash
cd Python/dashboard
streamlit run app.py
```

### 5. Ouvrir dans Unity
```
Unity Hub → Add → sélectionner le dossier crowdflowvr
Ouvrir Assets/Scenes/SampleScene.unity
Lancer le Play Mode
```

---

## Modes de jeu

### Mode VR (Meta Quest 2)
```
File → Build Settings → Android → Build & Run
Casque connecté en USB avec mode développeur activé
```

### Mode PC sans casque
```
La caméra isométrique top-down s'active automatiquement
Touche E = déclencher évacuation d'urgence
Touche P = déclencher une panique (test)
```

---

## Conformité aux exigences

| Exigence | Statut | Détails |
|----------|--------|---------|
| Projet sur GitHub uniquement | OK | github.com/maryemmouslim/crowdflowvr |
| README détaillé | OK | Ce fichier |
| Intelligence Artificielle | OK | IsolationForest + NavMesh + Helbing |
| Big Data | OK | Dataset ETH/UCY + pipeline CSV + dashboard |
| Mode VR headset | OK | Meta Quest 2 + OpenXR |
| Exécutable PC sans headset | OK | Caméra top-down automatique |
| Collaboration VR | OK | Multiplayer asymétrique VR + Desktop |

---

## Références scientifiques

- Lerner, A. et al. (2007). *Crowds by Example*. ICCV 2007.
- Pellegrini, S. et al. (2009). *You'll Never Walk Alone*. ICCV 2009.
- Helbing, D. & Molnár, P. (1995). *Social Force Model for Pedestrian Dynamics*. Physical Review E.

---

*ISIBD — Projet Final Semestre 8 — Mai 2026*
