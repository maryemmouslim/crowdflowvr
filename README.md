\# CrowdFlow VR

Simulation de gestion de foule par IA en Realite Virtuelle

Unity 2022 LTS | Meta Quest 2 | OpenXR | Python 3.11



\## Equipe

| Membre  | Pilier                   | Branche                |

|---------|--------------------------|------------------------|

| Maroua  | Modele ML \& Prediction   | feature/ml-engine      |

| Imane   | NavMesh IA \& Pathfinding | feature/navmesh-ai     |

| Houda   | Big Data \& Dashboard     | feature/data-pipeline  |

| Meryem  | VR \& Interaction         | feature/vr-ux          |

| Hasnaa  | Comportements \& Panique  | feature/panic-behavior |

| Assil   | Reseau \& Multiplayer     | feature/networking     |



\## Structure du projet

\- Assets/Scripts/ML       <- Maroua

\- Assets/Scripts/AI       <- Imane

\- Assets/Scripts/Data     <- Houda

\- Assets/Scripts/VR       <- Meryem

\- Assets/Scripts/Behavior <- Hasnaa

\- Assets/Scripts/Network  <- Assil

\- Python/ml\_engine        <- Maroua

\- Python/dashboard        <- Houda



\## Lancement

1\. Cloner le repo

2\. Ouvrir dans Unity 2022 LTS

3\. cd Python/ml\_engine \&\& pip install -r requirements.txt

4\. Lancer infer\_server.py avant de jouer la scene Unity

