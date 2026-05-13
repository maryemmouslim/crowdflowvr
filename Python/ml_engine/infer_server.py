import socket
import json
import random

sock = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
sock.bind(('0.0.0.0', 5005))
print('=== ML Server CrowdFlow VR ===')
print('En attente de donnees Unity sur port 5005...')

while True:
    data, addr = sock.recvfrom(65535)
    payload = json.loads(data.decode())
    agents = payload.get('agents', [])
    
    nb = len(agents)
    speeds = [a['spd'] for a in agents] if agents else [0]
    mean_speed = sum(speeds) / len(speeds)
    
    # Detection de danger : trop d'agents ou vitesse anormale
    is_danger = nb > 30 or mean_speed > 2.5
    density = nb / 50.0
    
    print(f'Agents: {nb} | Vitesse moy: {mean_speed:.2f} | DANGER: {is_danger}')
    
    response = json.dumps({
        'danger': bool(is_danger),
        'density': float(density),
        'score': float(mean_speed)
    })
    sock.sendto(response.encode(), addr)