# Python/dashboard/app.py
import streamlit as st
import pandas as pd
import numpy as np
import glob
import os
import time
import platform
import random
from datetime import datetime

st.set_page_config(page_title='CrowdFlow VR Dashboard', layout='wide')
st.title('🏟 CrowdFlow VR - Tableau de Bord Temps Réel')

# ─── Recherche du fichier CSV Unity ──────────────────────────────────────────
if platform.system() == 'Windows':
    base_dir = os.path.expandvars(r'%APPDATA%/../LocalLow/DefaultCompany')
else:
    base_dir = os.path.expanduser('~/Library/Application Support/DefaultCompany')

search_pattern = os.path.join(base_dir, '**', 'session_*.csv')
csvs = sorted(glob.glob(search_pattern, recursive=True))
is_live = len(csvs) > 0

# ─── Mode simulation si pas de CSV ───────────────────────────────────────────
def generate_sim_frame(frame_num, prev_n=50):
    n = max(10, min(120, prev_n + random.randint(-2, 4)))
    panic  = frame_num > 60
    evac   = frame_num > 90
    rows = []
    for i in range(n):
        x = np.clip(random.gauss(0, 6) if not panic else random.gauss(random.choice([-7, 7]), 3), -10, 10)
        z = np.clip(random.gauss(0, 6) if not panic else random.gauss(random.choice([-7, 7]), 3), -10, 10)
        if evac and i < n * 0.4:
            state, speed = 'Evacuation', max(0, random.gauss(2.0, 0.4))
        elif panic and i < n * 0.5:
            state, speed = 'Panic',      max(0, random.gauss(3.2, 0.8))
        else:
            state, speed = 'Normal',     max(0, random.gauss(1.4, 0.3))
        rows.append({'frame': frame_num, 'time': round(frame_num * 0.5, 2),
                     'ped_id': i, 'x': round(x, 2), 'z': round(z, 2),
                     'speed': round(speed, 2), 'state': state})
    return pd.DataFrame(rows)

if 'sim_frame'   not in st.session_state: st.session_state.sim_frame   = 0
if 'sim_history' not in st.session_state: st.session_state.sim_history = pd.DataFrame()
if 'prev_n'      not in st.session_state: st.session_state.prev_n      = 50

# ─── Chargement des données ───────────────────────────────────────────────────
if is_live:
    df_all = pd.read_csv(csvs[-1])
    st.success(f"✅ LIVE — {os.path.basename(csvs[-1])}")
else:
    st.warning("⚙️ **Mode simulation** — Unity non connecté. Le dashboard bascule automatiquement en LIVE dès que Unity est en Play Mode.")
    st.session_state.sim_frame += 1
    new_frame = generate_sim_frame(st.session_state.sim_frame, st.session_state.prev_n)
    st.session_state.prev_n = len(new_frame)
    st.session_state.sim_history = pd.concat([st.session_state.sim_history, new_frame], ignore_index=True)
    df_all = st.session_state.sim_history

last = df_all[df_all.frame == df_all.frame.max()]

# ─── Métriques ────────────────────────────────────────────────────────────────
n_active  = len(last)
speed_avg = last.speed.mean() if n_active > 0 else 0
n_panic   = len(last[last.state == 'Panic'])
n_evac    = df_all[df_all.state == 'Evacuation'].ped_id.nunique()
is_danger = last.speed.std() > 1.2 or n_panic > n_active * 0.3

c1, c2, c3, c4, c5 = st.columns(5)
c1.metric('Agents actifs',  n_active)
c2.metric('Vitesse moy.',   f"{speed_avg:.2f} m/s")
c3.metric('Zone danger',    '⚠ OUI' if is_danger else '✓ NON')
c4.metric('En panique',     n_panic)
c5.metric('Évacués',        n_evac)

st.markdown("---")

# ─── Graphes ──────────────────────────────────────────────────────────────────
col_left, col_right = st.columns(2)

with col_left:
    st.subheader('🗺 Heatmap de densité')
    if len(last) > 0:
        h, xe, ze = np.histogram2d(last.x, last.z, bins=12, range=[[-10,10],[-10,10]])
        st.dataframe(
            pd.DataFrame(h.T,
                         index=[f"{v:.0f}" for v in ze[:-1]],
                         columns=[f"{v:.0f}" for v in xe[:-1]]
            ).style.background_gradient('YlOrRd', axis=None).format("{:.0f}"),
            use_container_width=True
        )

with col_right:
    st.subheader('📈 Agents au fil du temps')
    timeline = df_all.groupby('frame').ped_id.nunique().reset_index()
    timeline.columns = ['Frame', 'Agents']
    st.line_chart(timeline.set_index('Frame'), use_container_width=True)

# ─── États + distribution vitesses ───────────────────────────────────────────
col_a, col_b = st.columns(2)

with col_a:
    st.subheader('🚦 États des agents')
    st.dataframe(last.state.value_counts().reset_index().rename(
        columns={'state': 'État', 'count': 'Nombre'}), hide_index=True, use_container_width=True)

with col_b:
    st.subheader('⚡ Distribution des vitesses')
    hist = pd.cut(last.speed,
                  bins=[0, 0.5, 1.0, 1.5, 2.0, 2.5, 3.0, 5.0],
                  labels=['0-0.5','0.5-1','1-1.5','1.5-2','2-2.5','2.5-3','3+']).value_counts().sort_index()
    st.bar_chart(hist, use_container_width=True)

# ─── Footer + refresh ────────────────────────────────────────────────────────
st.caption(f"Dernière mise à jour : {datetime.now().strftime('%H:%M:%S')} | {'LIVE' if is_live else 'SIMULATION'}")
time.sleep(1)
st.rerun()