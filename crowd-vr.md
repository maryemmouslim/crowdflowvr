🏟️ CrowdFlow VR
Gestion de Foule Intelligente en Réalité Virtuelle

Projet Final S8 — ISIBD | Unity 6.4 | Meta Quest 2 | Python 3.11


📌 Description du projet
CrowdFlow VR est une simulation immersive de gestion de foule dans une gare ferroviaire en heure de pointe. Des agents virtuels pilotés par une IA comportementale naviguent dans la scène. Un utilisateur en VR (Meta Quest 2) peut interagir avec la foule en temps réel, tandis qu'un utilisateur Desktop a une vue macro de la situation.
Le système détecte automatiquement les anomalies de comportement de foule (paniques, goulots, surcharges) grâce à un modèle Machine Learning entraîné sur de vraies données de piétons.

🎯 Fonctionnalités principales

Agents IA autonomes — navigation NavMesh avec machine à états (Normal / Dense / Panique / Évacuation)
Détection d'anomalies ML — modèle IsolationForest entraîné sur le dataset ETH/UCY (786 piétons réels, ICCV 2007)
Communication temps réel — serveur Python UDP qui analyse les flux de foule toutes les 200ms
Mode VR — Meta Quest 2 avec OpenXR, placement de barrières, overlay de danger
Mode PC sans casque — caméra isométrique top-down, contrôles clavier
Dashboard Big Data — visualisation live des métriques (densité, alertes, heatmap)
Multiplayer asymétrique — VR + Desktop connectés en réseau local


👥 Équipe de développement
MembreRôleBrancheContributionMarouaIA & ML + Design Scène 3Dfeature/ml-engineModèle ML (ETH/UCY), serveur UDP, MLClient.cs, prefabs gareImaneNavMesh IA & Agentsfeature/navmesh-aiCrowdAgent.cs, CrowdSpawner.cs, NavMesh bakeHoudaBig Data & Dashboardfeature/data-pipelineDataLogger.cs, MetricsManager.cs, dashboard StreamlitMeryemVR & Interactionfeature/vr-uxXR Origin, BarrierPlacer.cs, VROverlay.csHasnaaComportements & Paniquefeature/panic-behaviorSocialForce.cs (Helbing 1995), PanicTrigger.cs, BoidsBehavior.csAssilRéseau & Intégrationfeature/networkingNetworkSync.cs, DesktopController.cs, scène finale

🏗️ Structure du dépôt
crowdflowvr/
├── Assets/
│   ├── Scenes/
│   │   └── SampleScene.unity          ← Scène principale (Assil)
│   ├── Scripts/
│   │   ├── ML/                        ← Maroua (MLClient.cs)
│   │   ├── AI/                        ← Imane (CrowdAgent.cs, CrowdSpawner.cs)
│   │   ├── Data/                      ← Houda (DataLogger.cs, MetricsManager.cs)
│   │   ├── VR/                        ← Meryem (BarrierPlacer.cs, VROverlay.cs)
│   │   ├── Behavior/                  ← Hasnaa (SocialForce.cs, PanicTrigger.cs)
│   │   └── Network/                   ← Assil (NetworkSync.cs, DesktopController.cs)
│   └── Prefabs/
│       └── Maroua/                    ← Mobilier de gare (30+ prefabs)
├── Python/
│   ├── ml_engine/                     ← Maroua
│   │   ├── infer_server.py            ← Serveur UDP d'inférence ML
│   │   ├── 02_train_model.py          ← Entraînement IsolationForest
│   │   ├── model.pkl                  ← Modèle entraîné
│   │   └── requirements.txt
│   └── dashboard/                     ← Houda
│       └── app.py                     ← Dashboard Streamlit
├── Docs/
│   ├── ML_metrics.md
│   ├── Performance_report.md
│   └── Network_guide.md
└── README.md

🤖 Intelligence Artificielle & Big Data
Modèle ML (Maroua)

Dataset : ETH/UCY (Lerner et al. ICCV 2007 + Pellegrini et al. ICCV 2009)
Données : 4534 frames réelles, 360 piétons filmés en ville
Algorithme : IsolationForest (détection d'anomalies non supervisée)
Features : nb_agents, vitesse_moyenne, écart_type, percentile_90
Résultat : 227 anomalies détectées (5%) — paniques, goulots, surcharges

Comportements (Hasnaa)

Modèle de Social Force (Helbing 1995) — référence scientifique internationale
Algorithme Boids — séparation, alignement, cohésion
Propagation de panique par contagion comportementale

Big Data Pipeline (Houda)

Logging CSV temps réel depuis Unity (toutes les 500ms)
Dashboard Streamlit avec heatmap de densité, alertes, graphes historiques


🚀 Installation et lancement
Prérequis

Unity 6.4 (2022 LTS)
Python 3.11+
Meta Quest 2 (optionnel — mode PC disponible)
Git

Étape 1 — Cloner le dépôt
bashgit clone https://github.com/maryemmouslim/crowdflowvr.git
cd crowdflowvr
Étape 2 — Installer les dépendances Python
bashcd Python/ml_engine
pip install -r requirements.txt
Étape 3 — Lancer le serveur ML (obligatoire)
bash# Dans Python/ml_engine/
python infer_server.py
# Le serveur écoute sur le port UDP 5005
Étape 4 — Lancer le dashboard (optionnel)
bash# Dans Python/dashboard/
streamlit run app.py
# Dashboard accessible sur http://localhost:8501
Étape 5 — Ouvrir dans Unity

Unity Hub → Add → sélectionner le dossier crowdflowvr
Ouvrir Assets/Scenes/SampleScene.unity
Lancer le Play Mode

Mode VR (Meta Quest 2)
File → Build Settings → Android → Build & Run
Le casque doit être connecté en USB avec le mode développeur activé
Mode PC sans casque
Le projet détecte automatiquement l'absence de VR
→ La caméra isométrique top-down s'active
→ Touche E = déclencher évacuation d'urgence
→ Touche P = déclencher une panique (test)

📊 Exigences techniques — conformité
ExigenceStatutDétailsProjet sur GitHub uniquement✅github.com/maryemmouslim/crowdflowvrREADME détaillé✅Ce fichierIntelligence Artificielle✅ML (IsolationForest) + NavMesh + HelbingBig Data✅Dataset ETH/UCY + pipeline CSV + dashboardMode VR (headset)✅Meta Quest 2 + OpenXRExécutable PC sans headset✅Caméra top-down automatiqueCollaboration VR✅Multiplayer asymétrique VR + Desktop

📁 Livrables

✅ Code source — sur GitHub, branche develop
⏳ Rapport PDF — en cours de rédaction
⏳ Vidéo démonstrative — à enregistrer sur la version finale


🔬 Références scientifiques

Lerner, A. et al. (2007). Crowds by Example. ICCV 2007.
Pellegrini, S. et al. (2009). You'll Never Walk Alone. ICCV 2009.
Helbing, D. & Molnár, P. (1995). Social Force Model for Pedestrian Dynamics. Physical Review E.