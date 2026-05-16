# Python/ml_engine/infer_server.py
import socket, json, pickle
import numpy as np

# Charger le modèle
data = pickle.load(open('model.pkl', 'rb'))
model = data['model']
scaler = data['scaler']

# Serveur UDP port 5005
sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind(('0.0.0.0', 5005))
print("✅ Serveur ML en écoute sur port 5005...")

while True:
    raw, addr = sock.recvfrom(65535)
    try:
        payload = json.loads(raw.decode('utf-8'))
        agents = payload['agents']
        
        if len(agents) == 0:
            continue
            
        # Extraire features
        speeds = [a['spd'] for a in agents]
        nb     = len(agents)
        mu     = np.mean(speeds)
        sd     = np.std(speeds) if len(speeds) > 1 else 0
        p90    = np.percentile(speeds, 90)
        
        X = scaler.transform([[nb, mu, sd, p90]])
        pred = model.predict(X)[0]
        danger = pred == -1
        
        response = json.dumps({'danger': danger, 'nb': nb, 'mu': round(mu,2)})
        sock.sendto(response.encode(), addr)
        
        if danger:
            print(f"🚨 DANGER détecté ! {nb} agents, vitesse moy={mu:.2f}")
        else:
            print(f"✅ Normal — {nb} agents, vitesse moy={mu:.2f}")
            
    except Exception as e:
        print(f"Erreur : {e}")