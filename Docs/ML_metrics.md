\# ML Metrics — CrowdFlow VR



\## Modèle : IsolationForest



\## Données d'entraînement

\- Dataset : ETH (Zürich) + UCY (Chypre)

\- Source : OpenTraj — https://github.com/crowdbotp/OpenTraj

\- Frames réelles : 4534

\- Piétons réels : 360

\- Scènes : seq\_eth, seq\_hotel, zara01, zara02



\## Résultats

\- Anomalies détectées : 227 / 4534 (5%)

\- Contamination : 0.05

\- Estimateurs : 100

\- Random state : 42



\## Features utilisées

\- nb : nombre d'agents par frame

\- mu : vitesse moyenne

\- sd : écart-type des vitesses

\- p90 : percentile 90 des vitesses



\## Communication

\- Protocole : UDP

\- Port : 5005

\- Fréquence : toutes les 200ms

\- Format : JSON

